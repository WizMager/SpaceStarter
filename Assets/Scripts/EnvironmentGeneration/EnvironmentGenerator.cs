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
        private readonly List<Transform> _allEnvironment;
        private readonly List<PlanetCell> _planetCellsTop;
        private readonly List<PlanetCell> _planetCellsDown;
        private readonly int _environmentObjects;
        private readonly PlanetView _planetView;

        private readonly BuildingAroundPlanetGenerator _buildingAroundPlanetGenerator;
        private readonly BuildingOnPlanetGenerator _buildingOnPlanetGenerator;
        private readonly TreesOnPlanetGenerator _treesOnPlanetGenerator;
        private readonly CheliksOnPlanetGenerator _cheliksOnPlanetGenerator;
        private readonly PaintPlanet _paintPlanet;

        public EnvironmentGenerator(StateController stateController, AllData data, PlanetView planetView, 
            Dictionary<int, Dictionary<int, List<Material>>> preparedMaterials)
        {
            _planetView = planetView;
            _planetCellsTop = new List<PlanetCell>();
            _planetCellsDown = new List<PlanetCell>();
            _environmentObjects = data.ObjectsOnPlanetData.buildingsOnPlanet + data.ObjectsOnPlanetData.buildingsOnPlanet + data.ObjectsOnPlanetData.cheliksOnPlanet;
            GenerateCells(data.ObjectsOnPlanetData.maximumBuildingAngleUp, data.ObjectsOnPlanetData.maximumBuildingAngleDown);
            _allEnvironment = new List<Transform>();
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
            ClearCells();
            var buildingsAroundPlanet = _buildingAroundPlanetGenerator.GenerateBuildingsAroundPlanet();
            var treesAroundPlanet = _buildingAroundPlanetGenerator.GenerateTreesAroundPlanet();
            var topBuildingsOnPlanet = _buildingOnPlanetGenerator.CreateTopBuildingAndPosition(_planetCellsTop);
            var downBuildingsOnPlanet = _buildingOnPlanetGenerator.CreateDownBuildingAndPosition(_planetCellsDown);
            var topTreesOnPlanet = _treesOnPlanetGenerator.CreateTopTreesAndPosition(_planetCellsTop);
            var downTreesOnPlanet = _treesOnPlanetGenerator.CreateDownTreesAndPosition(_planetCellsDown);
            var topCheliksOnPlanet = _cheliksOnPlanetGenerator.CreateTopCheliksAndPosition(_planetCellsTop);
            var downCheliksOnPlanet = _cheliksOnPlanetGenerator.CreateDownCheliksAndPosition(_planetCellsDown);

            _allEnvironment.AddRange(buildingsAroundPlanet);
            _allEnvironment.AddRange(treesAroundPlanet);
            _allEnvironment.AddRange(topBuildingsOnPlanet);
            _allEnvironment.AddRange(downBuildingsOnPlanet);
            _allEnvironment.AddRange(topTreesOnPlanet);
            _allEnvironment.AddRange(downTreesOnPlanet);
            _allEnvironment.AddRange(topCheliksOnPlanet);
            _allEnvironment.AddRange(downCheliksOnPlanet);

            return _allEnvironment;
        }

        private void ClearCells()
        {
            for (int i = 0; i < _planetCellsTop.Count; i++)
            {
                if (!_planetCellsTop[i].IsOccupied) continue;
                var tempCell = new PlanetCell(_planetCellsTop[i].rangeX, _planetCellsTop[i].rangeY, _planetCellsTop[i].rangeZ);
                _planetCellsTop[i] = tempCell;
            }

            for (int i = 0; i < _planetCellsDown.Count; i++)
            {
                if (!_planetCellsDown[i].IsOccupied) continue;
                var tempCell = new PlanetCell(_planetCellsDown[i].rangeX, _planetCellsDown[i].rangeY, _planetCellsDown[i].rangeZ);
                _planetCellsDown[i] = tempCell;
            }
        }
        
        public List<Transform> TakePlanetPieces()
        {
            var pieces = _planetView.GetComponentsInChildren<Transform>().ToList();
            _paintPlanet.Paint(pieces);
            return pieces;
        }
    }
}