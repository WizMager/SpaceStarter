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
        private List<bool> _isUpCellOccupied;
        private List<bool> _isDownCellOccupied;
        private readonly int _environmentObjects;

        private readonly BuildingAroundPlanetGenerator _buildingAroundPlanetGenerator;
        private readonly BuildingOnPlanetGenerator _buildingOnPlanetGenerator;

        public EnvironmentGenerator(AllData data, Transform planet)
        {
            _planetCellsUp = new List<PlanetCell>();
            _isUpCellOccupied = new List<bool>();
            _planetCellsDown = new List<PlanetCell>();
            _isDownCellOccupied = new List<bool>();
            _environmentObjects = data.ObjectsOnPlanetData.buildingsOnPlanet;
            GenerateCells(data.ObjectsOnPlanetData.maximumBuildingAngleUp, data.ObjectsOnPlanetData.maximumBuildingAngleDown);
            _allEnvironment = new List<Transform>();
            var rootEnvironment = new GameObject("PlanetEnvironment");
            var planetRadius = planet.GetComponent<SphereCollider>().radius;
            _buildingAroundPlanetGenerator = new BuildingAroundPlanetGenerator(data, planet, planetRadius, rootEnvironment);
            _buildingOnPlanetGenerator = new BuildingOnPlanetGenerator(data, planet, planetRadius, rootEnvironment);
        }

        private void GenerateCells(float maxAngleUp, float maxAngleDown)
        {
            var halfEnvironmentObject = _environmentObjects / 2;
            var availableAngleX = 90f - maxAngleUp;
            var availableAngleZ = 90f - maxAngleUp;
            var upCellSize = new Vector3(availableAngleX / halfEnvironmentObject, 360f / halfEnvironmentObject,availableAngleZ / halfEnvironmentObject);
            availableAngleX = 90f - maxAngleDown;
            availableAngleZ = 90f - maxAngleDown;
            var downCellSize = new Vector3(availableAngleX / halfEnvironmentObject, 360f / halfEnvironmentObject,availableAngleZ / halfEnvironmentObject);
            var startX = 90f - maxAngleUp;
            var startZ = 90f - maxAngleUp;
            var startY = 360f;
            do
            {
                var planetCell = new PlanetCell();
                planetCell.rangeX.x = startX;
                planetCell.rangeY.x = startY;
                planetCell.rangeZ.x = startZ;
                startX -= upCellSize.x;
                startY -= upCellSize.y;
                startZ -= upCellSize.z;
                planetCell.rangeX.y = startX;
                planetCell.rangeY.y = startY;
                planetCell.rangeZ.y = startZ;
                _planetCellsUp.Add(planetCell);
            } 
            while (startX > upCellSize.x && startZ > upCellSize.z && startY > upCellSize.y);

            startX = 90f - maxAngleUp;
            startZ = 90f - maxAngleUp;
            startY = 360f;
            
            do
            {
                var planetCell = new PlanetCell();
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
            _buildingOnPlanetGenerator.CreateBuildingAndPosition(numberBuildingsAroundPlanet);
            return _allEnvironment;
        }

    }
}