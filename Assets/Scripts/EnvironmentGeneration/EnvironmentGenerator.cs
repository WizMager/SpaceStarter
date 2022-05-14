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
            var startX = 90f - maxAngleUp;
            var startZ = 90f - maxAngleUp;
            var startY = 360f;
            
            // do
            // {
            //     var planetCell = new PlanetCell
            //     {
            //        isOccupied = false
            //     };
            //     planetCell.rangeX.x = startX;
            //     planetCell.rangeY.x = startY;
            //     planetCell.rangeZ.x = startZ;
            //     startX -= upCellSize.x;
            //     startY -= upCellSize.y;
            //     startZ -= upCellSize.z;
            //     planetCell.rangeX.y = startX;
            //     planetCell.rangeY.y = startY;
            //     planetCell.rangeZ.y = startZ;
            //     _planetCellsUp.Add(planetCell);
            // } 
            // while (startX > upCellSize.x && startZ > upCellSize.z && startY > upCellSize.y);

            for (var x = upCellSize.x; x < startX; x+= upCellSize.x)
            {
                for (var y = upCellSize.y; y < startY; y += upCellSize.y)
                {
                    for (var z = upCellSize.z; z < startZ; z += upCellSize.z)
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
            
            startX = 90f - maxAngleUp;
            startZ = 90f - maxAngleUp;
            startY = 360f;
            
            do
            {
                var planetCell = new PlanetCell
                {
                    isOccupied = false
                };
                planetCell.rangeX.x = startX;
                planetCell.rangeY.x = startY;
                planetCell.rangeZ.x = startZ;
                startX -= downCellSize.x;
                startY -= downCellSize.y;
                startZ -= downCellSize.z;
                planetCell.rangeX.y = startX;
                planetCell.rangeY.y = startY;
                planetCell.rangeZ.y = startZ;
                _planetCellsDown.Add(planetCell);
            } 
            while (startX > upCellSize.x && startZ > upCellSize.z && startY > upCellSize.y);
        }
        
        public List<Transform> GenerateEnvironment()
        {
            var buildingsAroundPlanet = _buildingAroundPlanetGenerator.GenerateBuildingsAroundPlanet();
            _allEnvironment.Union(buildingsAroundPlanet);
            var numberBuildingsAroundPlanet = _buildingAroundPlanetGenerator.BuildingsSpawned;
            Debug.Log(_planetCellsUp.Count);
            _buildingOnPlanetGenerator.CreateBuildingAndPosition(_planetCellsUp);
            return _allEnvironment;
        }

    }
}