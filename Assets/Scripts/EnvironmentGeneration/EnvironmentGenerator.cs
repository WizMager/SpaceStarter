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
        private Dictionary<int, List<Vector3>> _allPositions;
        private Dictionary<int, List<Quaternion>> _allRotations;
        private readonly List<PlanetCell> _planetCellsTop;
        private readonly List<PlanetCell> _planetCellsDown;
        private readonly int _environmentObjects;
        private readonly PlanetView _planetView;

        private readonly BuildingAroundPlanetGenerator _buildingAroundPlanetGenerator;
        private readonly BuildingOnPlanetGenerator _buildingOnPlanetGenerator;
        private readonly TreesOnPlanetGenerator _treesOnPlanetGenerator;
        private readonly CheliksOnPlanetGenerator _cheliksOnPlanetGenerator;
        private readonly PaintPlanet _paintPlanet;

        public Dictionary<int, List<Vector3>> GetAllPositions => _allPositions;
        public Dictionary<int, List<Quaternion>> GetAllRotations => _allRotations;
        
        public EnvironmentGenerator(StateController stateController, AllData data, PlanetView planetView, 
            Dictionary<int, Dictionary<int, List<Material>>> preparedMaterials)
        {
            _planetView = planetView;
            _planetCellsTop = new List<PlanetCell>();
            _planetCellsDown = new List<PlanetCell>();
            _environmentObjects = data.ObjectsOnPlanetData.buildingsOnPlanet + data.ObjectsOnPlanetData.buildingsOnPlanet + data.ObjectsOnPlanetData.cheliksOnPlanet;
            GenerateCells(data.ObjectsOnPlanetData.maximumBuildingAngleUp, data.ObjectsOnPlanetData.maximumBuildingAngleDown);
            _allPositions = new Dictionary<int, List<Vector3>>();
            _allRotations = new Dictionary<int, List<Quaternion>>();
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
            var transformsList = new List<Transform>();
            var buildingsAroundPlanet = _buildingAroundPlanetGenerator.GenerateBuildingsAroundPlanet();
            var treesAroundPlanet = _buildingAroundPlanetGenerator.GenerateTreesAroundPlanet();
            var topBuildingsOnPlanet = _buildingOnPlanetGenerator.CreateTopBuildingAndPosition(_planetCellsTop);
            var downBuildingsOnPlanet = _buildingOnPlanetGenerator.CreateDownBuildingAndPosition(_planetCellsDown);
            var topTreesOnPlanet = _treesOnPlanetGenerator.CreateTopTreesAndPosition(_planetCellsTop);
            var downTreesOnPlanet = _treesOnPlanetGenerator.CreateDownTreesAndPosition(_planetCellsDown);
            var topCheliksOnPlanet = _cheliksOnPlanetGenerator.CreateTopCheliksAndPosition(_planetCellsTop);
            var downCheliksOnPlanet = _cheliksOnPlanetGenerator.CreateDownCheliksAndPosition(_planetCellsDown);
            transformsList.AddRange(buildingsAroundPlanet);
            transformsList.AddRange(treesAroundPlanet);
            transformsList.AddRange(topBuildingsOnPlanet);
            transformsList.AddRange(downBuildingsOnPlanet);
            transformsList.AddRange(topTreesOnPlanet);
            transformsList.AddRange(downTreesOnPlanet);
            transformsList.AddRange(topCheliksOnPlanet);
            transformsList.AddRange(downCheliksOnPlanet);

            _allPositions.Add(0, buildingsAroundPlanet.Select(t => t.position).ToList());
            _allRotations.Add(0, buildingsAroundPlanet.Select(t => t.rotation).ToList());
            _allPositions.Add(1, treesAroundPlanet.Select(t => t.position).ToList());
            _allRotations.Add(1, treesAroundPlanet.Select(t => t.rotation).ToList());
            _allPositions.Add(2, topBuildingsOnPlanet.Select(t => t.position).ToList());
            _allRotations.Add(2, topBuildingsOnPlanet.Select(t => t.rotation).ToList());
            _allPositions.Add(3, downBuildingsOnPlanet.Select(t => t.position).ToList());
            _allRotations.Add(3, downBuildingsOnPlanet.Select(t => t.rotation).ToList());
            _allPositions.Add(4, topTreesOnPlanet.Select(t => t.position).ToList());
            _allRotations.Add(4, topTreesOnPlanet.Select(t => t.rotation).ToList());
            _allPositions.Add(5, downTreesOnPlanet.Select(t => t.position).ToList());
            _allRotations.Add(5, downTreesOnPlanet.Select(t => t.rotation).ToList());
            _allPositions.Add(6, topCheliksOnPlanet.Select(t => t.position).ToList());
            _allRotations.Add(6, topCheliksOnPlanet.Select(t => t.rotation).ToList());
            _allPositions.Add(7, downCheliksOnPlanet.Select(t => t.position).ToList());
            _allRotations.Add(7, downCheliksOnPlanet.Select(t => t.rotation).ToList());
            
            return transformsList;
        }

        public List<Transform> SetEnvironment(Dictionary<int, List<Vector3>> allPositions, Dictionary<int, List<Quaternion>> allRotations)
        {
            //TODO something wrong in BuildingsAroundPlanetGeneration, if activate for tree - return empty lis, if activate for houses - freeze game
            var transformsList = new List<Transform>();
            //var buildingsAroundPlanet = _buildingAroundPlanetGenerator.SetBuildingsAroundPlanet(allPositions[0], allRotations[0]);
            //var treesAroundPlanet = _buildingAroundPlanetGenerator.SetTreesAroundPlanet(allPositions[1], allRotations[1]);
            var topBuildingsOnPlanet = _buildingOnPlanetGenerator.SetBuildingAndPosition(allPositions[2], allRotations[2]);
            var downBuildingsOnPlanet = _buildingOnPlanetGenerator.SetBuildingAndPosition(allPositions[3], allRotations[3]);
            var topTreesOnPlanet = _treesOnPlanetGenerator.SetTreesAndPosition(allPositions[4], allRotations[4]);
            var downTreesOnPlanet = _treesOnPlanetGenerator.SetTreesAndPosition(allPositions[5], allRotations[5]);
            var topCheliksOnPlanet = _cheliksOnPlanetGenerator.SetCheliksAndPosition(allPositions[6], allRotations[6]);
            var downCheliksOnPlanet = _cheliksOnPlanetGenerator.SetCheliksAndPosition(allPositions[7], allRotations[7]);
            //transformsList.AddRange(buildingsAroundPlanet);
            //transformsList.AddRange(treesAroundPlanet);
            transformsList.AddRange(topBuildingsOnPlanet);
            transformsList.AddRange(downBuildingsOnPlanet);
            transformsList.AddRange(topTreesOnPlanet);
            transformsList.AddRange(downTreesOnPlanet);
            transformsList.AddRange(topCheliksOnPlanet);
            transformsList.AddRange(downCheliksOnPlanet);
            
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