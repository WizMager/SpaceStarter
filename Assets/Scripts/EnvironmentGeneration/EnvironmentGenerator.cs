using System.Collections.Generic;
using System.Linq;
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

        public EnvironmentGenerator(AllData data, PlanetView planetView)
        {
            _planetView = planetView;
            _planetCellsTop = new List<PlanetCell>();
            _planetCellsDown = new List<PlanetCell>();
            _environmentObjects = data.ObjectsOnPlanetData.buildingsOnPlanet + data.ObjectsOnPlanetData.buildingsOnPlanet + data.ObjectsOnPlanetData.cheliksOnPlanet;
            GenerateCells(data.ObjectsOnPlanetData.maximumBuildingAngleUp, data.ObjectsOnPlanetData.maximumBuildingAngleDown);
            Debug.Log($"top {_planetCellsTop.Count}, down {_planetCellsDown.Count}");
            _allEnvironment = new List<Transform>();
            var rootEnvironment = new GameObject("PlanetEnvironment");
            var planetRadius = planetView.GetComponent<SphereCollider>().radius;
            _buildingAroundPlanetGenerator = new BuildingAroundPlanetGenerator(data, planetView.transform, planetRadius, rootEnvironment);
            _buildingOnPlanetGenerator = new BuildingOnPlanetGenerator(data, planetRadius, rootEnvironment);
            _treesOnPlanetGenerator = new TreesOnPlanetGenerator(data, planetRadius, rootEnvironment);
            _cheliksOnPlanetGenerator = new CheliksOnPlanetGenerator(data, planetRadius, rootEnvironment);
        }

        private void GenerateCells(float maxAngleUp, float maxAngleDown)
        {
            var halfEnvironmentObject = _environmentObjects / 40;
            Debug.Log(halfEnvironmentObject);
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
            
            //     do
            //     {
            //         var planetCell = new PlanetCell
            //         {
            //             isOccupied = false
            //         };
            //         planetCell.rangeX.x = startX;
            //         planetCell.rangeY.x = startY;
            //         planetCell.rangeZ.x = startZ;
            //         startX -= downCellSize.x;
            //         startY -= downCellSize.y;
            //         startZ -= downCellSize.z;
            //         planetCell.rangeX.y = startX;
            //         planetCell.rangeY.y = startY;
            //         planetCell.rangeZ.y = startZ;
            //         _planetCellsDown.Add(planetCell);
            //     } 
            //     while (startX > upCellSize.x && startZ > upCellSize.z && startY > upCellSize.y);
        }
        
        public List<Transform> GenerateEnvironment()
        {
            var buildingsAroundPlanet = _buildingAroundPlanetGenerator.GenerateBuildingsAroundPlanet();
            var topBuildingsOnPlanet = _buildingOnPlanetGenerator.CreateTopBuildingAndPosition(_planetCellsTop);
            var downBuildingsOnPlanet = _buildingOnPlanetGenerator.CreateDownBuildingAndPosition(_planetCellsDown);
            var topTreesOnPlanet = _treesOnPlanetGenerator.CreateTopTreesAndPosition(_planetCellsTop);
            var downTreesOnPlanet = _treesOnPlanetGenerator.CreateDownTreesAndPosition(_planetCellsDown);
            var topCheliksOnPlanet = _cheliksOnPlanetGenerator.CreateTopTreesAndPosition(_planetCellsTop);
            var downCheliksOnPlanet = _cheliksOnPlanetGenerator.CreateDownTreesAndPosition(_planetCellsDown);
            var planetPieces = _planetView.GetComponentsInChildren<Transform>().ToList();

            _allEnvironment.AddRange(buildingsAroundPlanet);
            _allEnvironment.AddRange(topBuildingsOnPlanet);
            _allEnvironment.AddRange(downBuildingsOnPlanet);
            _allEnvironment.AddRange(topTreesOnPlanet);
            _allEnvironment.AddRange(downTreesOnPlanet);
            _allEnvironment.AddRange(topCheliksOnPlanet);
            _allEnvironment.AddRange(downCheliksOnPlanet);
            _allEnvironment.AddRange(planetPieces);

            return _allEnvironment;
        }
    }
}