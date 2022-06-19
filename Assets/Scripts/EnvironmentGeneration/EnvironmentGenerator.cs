using System.Collections.Generic;
using System.Linq;
using Controllers;
using ScriptableData;
using UnityEngine;
using Utils;
using View;


namespace EnvironmentGeneration
{
    public class EnvironmentGenerator
    {
        private Dictionary<int, List<Transform>> _allEnvironment;
        private readonly List<PlanetCell> _planetCellsTop;
        private readonly List<PlanetCell> _planetCellsDown;
        private readonly int _environmentObjects;
        private readonly PlanetView _planetView;

        private readonly BuildingAroundPlanetGenerator _buildingAroundPlanetGenerator;
        private readonly BuildingOnPlanetGenerator _buildingOnPlanetGenerator;
        private readonly TreesOnPlanetGenerator _treesOnPlanetGenerator;
        private readonly CheliksOnPlanetGenerator _cheliksOnPlanetGenerator;
        private readonly PaintPlanet _paintPlanet;

        public Dictionary<int, List<Transform>> GetAllEnvironment => _allEnvironment;
        
        public EnvironmentGenerator(StateController stateController, AllData data, PlanetView planetView, 
            Dictionary<int, Dictionary<int, List<Material>>> preparedMaterials)
        {
            _planetView = planetView;
            _planetCellsTop = new List<PlanetCell>();
            _planetCellsDown = new List<PlanetCell>();
            _environmentObjects = data.ObjectsOnPlanetData.buildingsOnPlanet + data.ObjectsOnPlanetData.buildingsOnPlanet + data.ObjectsOnPlanetData.cheliksOnPlanet;
            GenerateCells(data.ObjectsOnPlanetData.maximumBuildingAngleUp, data.ObjectsOnPlanetData.maximumBuildingAngleDown);
            _allEnvironment = new Dictionary<int, List<Transform>>();
            var rootEnvironment = new GameObject("PlanetEnvironment");
            var planetRadius = planetView.GetComponent<SphereCollider>().radius;
            _buildingAroundPlanetGenerator = new BuildingAroundPlanetGenerator(stateController, data, planetView.transform, 
                planetRadius, rootEnvironment, preparedMaterials[0], preparedMaterials[2]);
            _buildingOnPlanetGenerator = new BuildingOnPlanetGenerator(data, planetRadius, rootEnvironment, preparedMaterials[0]);
            _treesOnPlanetGenerator = new TreesOnPlanetGenerator(data, planetRadius, rootEnvironment, preparedMaterials[2]);
            _cheliksOnPlanetGenerator = new CheliksOnPlanetGenerator(stateController, data, planetRadius, rootEnvironment, preparedMaterials[3][0][0]);
            _paintPlanet = new PaintPlanet(preparedMaterials[1][0]);
        }

        private void GenerateCells(float maxAngleUp, float maxAngleDown)
        {
            var halfEnvironmentObject = _environmentObjects / 40;
            var upCellSize = new Vector3((90f - maxAngleUp) / halfEnvironmentObject, 360f / halfEnvironmentObject,(90f - maxAngleUp) / halfEnvironmentObject);
            var downCellSize = new Vector3((90f - maxAngleDown) / halfEnvironmentObject, 360f / halfEnvironmentObject,(90f - maxAngleDown) / halfEnvironmentObject);
            var availableAngleX = 90f - maxAngleUp;
            var availableAngleY = 360f;
            var availableAngleZ = 90f - maxAngleUp;

            for (var x = upCellSize.x; x < availableAngleX; x+= upCellSize.x)
            {
                for (var y = upCellSize.y; y < availableAngleY; y += upCellSize.y)
                {
                    for (var z = upCellSize.z; z < availableAngleZ; z += upCellSize.z)
                    {
                        var upCell = new PlanetCell
                        (
                            new Vector2(x - upCellSize.x, x),
                            new Vector2(y - upCellSize.y, y),
                            new Vector2(z - upCellSize.z, z)
                        );
                        _planetCellsTop.Add(upCell);
                    }
                }
            }

            availableAngleX = 180f;
            availableAngleZ = 180f;
            var startDownX = 90f + maxAngleDown;
            var startDownZ = 90f + maxAngleDown;
            
            for (var x = startDownX + downCellSize.x; x < availableAngleX; x+= downCellSize.x)
            {
                for (var y = downCellSize.y; y < availableAngleY; y += downCellSize.y)
                {
                    for (var z = startDownZ + downCellSize.z; z < availableAngleZ; z += downCellSize.z)
                    {
                        var downCell = new PlanetCell
                        (
                            new Vector2(x - downCellSize.x, x),
                            new Vector2(y - downCellSize.y, y),
                            new Vector2(z - downCellSize.z, z)
                        );
                        _planetCellsDown.Add(downCell);
                    }
                }
            }
        }

        public List<Transform> GenerateEnvironment()
        {
            var buildingsAroundPlanet = _buildingAroundPlanetGenerator.GenerateBuildingsAroundPlanet();
            var treesAroundPlanet = _buildingAroundPlanetGenerator.GenerateTreesAroundPlanet();
            var topBuildingsOnPlanet = _buildingOnPlanetGenerator.CreateTopBuildingAndPosition(_planetCellsTop);
            var downBuildingsOnPlanet = _buildingOnPlanetGenerator.CreateDownBuildingAndPosition(_planetCellsDown);
            var topTreesOnPlanet = _treesOnPlanetGenerator.CreateTopTreesAndPosition(_planetCellsTop);
            var downTreesOnPlanet = _treesOnPlanetGenerator.CreateDownTreesAndPosition(_planetCellsDown);
            var topCheliksOnPlanet = _cheliksOnPlanetGenerator.CreateTopCheliksAndPosition(_planetCellsTop);
            var downCheliksOnPlanet = _cheliksOnPlanetGenerator.CreateDownCheliksAndPosition(_planetCellsDown);

            _allEnvironment.Add(0, buildingsAroundPlanet);
            _allEnvironment.Add(1, treesAroundPlanet);
            _allEnvironment.Add(2, topBuildingsOnPlanet);
            _allEnvironment.Add(3, downBuildingsOnPlanet);
            _allEnvironment.Add(4, topTreesOnPlanet);
            _allEnvironment.Add(5, downTreesOnPlanet);
            _allEnvironment.Add(6, topCheliksOnPlanet);
            _allEnvironment.Add(7, downCheliksOnPlanet);

            var transformsList = new List<Transform>();
            foreach (var transforms in _allEnvironment)
            {
                transformsList.AddRange(transforms.Value);
            }
            return transformsList;
        }
        
        public List<Transform> SetEnvironment(Dictionary<int, List<Transform>> allEnvironment)
        {
            _allEnvironment = allEnvironment;
            //_buildingAroundPlanetGenerator.SetBuildingsAroundPlanet(_allEnvironment[0]);
            // _buildingAroundPlanetGenerator.SetTreesAroundPlanet(_allEnvironment[1]);
            // _buildingOnPlanetGenerator.SetBuildingAndPosition(_allEnvironment[2]);
            // _buildingOnPlanetGenerator.SetBuildingAndPosition(_allEnvironment[3]);
            // _treesOnPlanetGenerator.SetTreesAndPosition(_allEnvironment[4]);
            // _treesOnPlanetGenerator.SetTreesAndPosition(_allEnvironment[5]);
            // _cheliksOnPlanetGenerator.SetCheliksAndPosition(_allEnvironment[6]);
            //_cheliksOnPlanetGenerator.SetCheliksAndPosition(_allEnvironment[7]);

            var transformsList = new List<Transform>();
            foreach (var transforms in _allEnvironment)
            {
                transformsList.AddRange(transforms.Value);
            }
            return transformsList;
        }

        public List<Transform> TakePlanetPieces()
        {
            var pieces = _planetView.GetComponentsInChildren<Transform>().ToList();
            _paintPlanet.Paint(pieces);
            return pieces;
        }
    }
}