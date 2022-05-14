using System.Collections.Generic;
using System.Linq;
using ScriptableData;
using UnityEngine;
using Utils;


namespace EnvironmentGeneration
{
    public class EnvironmentGenerator
    {
        private readonly List<Transform> _allEnvironment;
        private List<PlanetCell> _planetCellsUp;
        private List<PlanetCell> _planetCellsDown;
        private readonly int _environmentObjects;

        private readonly BuildingAroundPlanetGenerator _buildingAroundPlanetGenerator;
        private readonly BuildingOnPlanetGenerator _buildingOnPlanetGenerator;

        public EnvironmentGenerator(AllData data, Transform planet)
        {
            _planetCellsUp = new List<PlanetCell>();
            _planetCellsDown = new List<PlanetCell>();
            _environmentObjects = data.ObjectsOnPlanetData.buildingsOnPlanet;
            GenerateCells(data.ObjectsOnPlanetData.maximumBuildingAngleUp, data.ObjectsOnPlanetData.maximumBuildingAngleDown);
            _allEnvironment = new List<Transform>();
            var rootEnvironment = new GameObject("PlanetEnvironment");
            var planetRadius = planet.GetComponent<SphereCollider>().radius;
            _buildingAroundPlanetGenerator = new BuildingAroundPlanetGenerator(data, planet, planetRadius, rootEnvironment);
            _buildingOnPlanetGenerator = new BuildingOnPlanetGenerator(data, planetRadius, rootEnvironment, planet);
        }

        private void GenerateCells(float maxAngleUp, float maxAngleDown)
        {
            var halfEnvironmentObject = _environmentObjects / 2;
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
                        {
                            isOccupied = false,
                            rangeX = new Vector2(x - upCellSize.x, x),
                            rangeY = new Vector2(y - upCellSize.y, y),
                            rangeZ = new Vector2(z - upCellSize.z, z)
                        };
                        _planetCellsUp.Add(upCell);
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
                        {
                            isOccupied = false,
                            rangeX = new Vector2(x - downCellSize.x, x),
                            rangeY = new Vector2(y - downCellSize.y, y),
                            rangeZ = new Vector2(z - downCellSize.z, z)
                        };
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
            _allEnvironment.Union(buildingsAroundPlanet);
            var numberBuildingsAroundPlanet = _buildingAroundPlanetGenerator.BuildingsSpawned;
            _buildingOnPlanetGenerator.CreateBuildingAndPosition(_planetCellsUp);
            _buildingOnPlanetGenerator.CreateBuildingAndPosition(_planetCellsDown);
            return _allEnvironment;
        }

    }
}