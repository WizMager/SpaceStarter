using System.Collections.Generic;
using Builders.HouseBuilder;
using ScriptableData;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace EnvironmentGeneration
{
    public class BuildingOnPlanetGenerator
    {
        private readonly float _invisibleBuildingAngle;
        private readonly float _planetRadius;
        private readonly int _maximumFloorInHouse;
        private readonly float _maximumAngleRotateBuildingAroundItself;
        private readonly int _buildingsOnPlanet;

        private readonly List<Transform> _spawnedTopBuildings;
        private readonly List<Transform> _spawnedDownBuildings;
        private readonly GameObject _rootBuildingOnPlanet;

        private readonly HouseBuilder[] _houseBuilders;
        private readonly HouseDirector _houseDirector;

        public BuildingOnPlanetGenerator(AllData data, float planetRadius, GameObject rootEnvironment)
        {
            _invisibleBuildingAngle = data.ObjectsOnPlanetData.flyAroundInvisibleObjectAngle;
            _planetRadius = planetRadius;
            _maximumFloorInHouse = data.ObjectsOnPlanetData.maximumFloorInHouseOnPlanet;
            _maximumAngleRotateBuildingAroundItself =
                data.ObjectsOnPlanetData.maximumAngleRotateBuildingAroundItselfOnPlanet;
            _buildingsOnPlanet = data.ObjectsOnPlanetData.buildingsOnPlanet;
            _spawnedTopBuildings = new List<Transform>();
            _spawnedDownBuildings = new List<Transform>();
            
            _rootBuildingOnPlanet = new GameObject("BuildingsOnPlanet");
            _rootBuildingOnPlanet.transform.SetParent(rootEnvironment.transform);

            _houseBuilders = new HouseBuilder[] {
                new HouseBuilder(data,1),
                new HouseBuilder(data,2),
                new HouseBuilder(data,3),
                new HouseBuilder(data,4),
                new HouseBuilder(data,5),
                new HouseBuilder(data,6)
            };
            _houseDirector = new HouseDirector
            {
                Builder = _houseBuilders[0]
            };
        }

        public List<Transform> CreateTopBuildingAndPosition(List<PlanetCell> planetCellsTop)
        {
            var spawnedTopBuildings = new List<Transform>();
            var createdBuildings = 0;
            var halfBuildingsOnPlanet = Mathf.RoundToInt(_buildingsOnPlanet / 2);
            do
            {
                var randomCell = Random.Range(0, planetCellsTop.Count);
                Debug.Log(planetCellsTop[randomCell].IsOccupied);
                if (planetCellsTop[randomCell].IsOccupied) continue;
                var tempCell = planetCellsTop[randomCell];
                tempCell.Occupied();
                planetCellsTop[randomCell] = tempCell;
                createdBuildings++;
                var randomAngleRotationBuilding = Random.Range(0f, _maximumAngleRotateBuildingAroundItself);
                var randomFloors = Random.Range(1, _maximumFloorInHouse);
                var randomBuildingType = Random.Range(0, 5);
                _houseDirector.Builder = _houseBuilders[randomBuildingType];
                var building = _houseDirector.BuildSimpleHouse(randomFloors);
                var positionAndRotation = GeneratePositionAndRotation(planetCellsTop[randomCell]);
                 building.transform.SetPositionAndRotation(positionAndRotation.Item1, positionAndRotation.Item2);
                 building.transform.RotateAround(building.transform.position, building.transform.up,
                     randomAngleRotationBuilding);
                spawnedTopBuildings.Add(building.transform);
                building.transform.SetParent(_rootBuildingOnPlanet.transform);
            } while (halfBuildingsOnPlanet > createdBuildings);

            return spawnedTopBuildings;
        }
        
        public List<Transform> CreateDownBuildingAndPosition(List<PlanetCell> planetCellsDown)
        {
            var spawnedDownBuildings = new List<Transform>();
            var createdBuildings = 0;
            var halfBuildingsOnPlanet = Mathf.RoundToInt(_buildingsOnPlanet / 2);
            do
            {
                var randomCell = Random.Range(0, planetCellsDown.Count);
                if (planetCellsDown[randomCell].IsOccupied) continue;
                var tempCell = planetCellsDown[randomCell];
                tempCell.Occupied();
                planetCellsDown[randomCell] = tempCell;
                createdBuildings++;
                var randomAngleRotationBuilding = Random.Range(0f, _maximumAngleRotateBuildingAroundItself);
                var randomFloors = Random.Range(1, _maximumFloorInHouse);
                var randomBuildingType = Random.Range(0, 5);
                _houseDirector.Builder = _houseBuilders[randomBuildingType];
                var building = _houseDirector.BuildSimpleHouse(randomFloors);
                var positionAndRotation = GeneratePositionAndRotation(planetCellsDown[randomCell]);
                building.transform.SetPositionAndRotation(positionAndRotation.Item1, positionAndRotation.Item2);
                building.transform.RotateAround(building.transform.position, building.transform.forward, 180f);
                spawnedDownBuildings.Add(building.transform);
                building.transform.RotateAround(building.transform.position, building.transform.up,
                    randomAngleRotationBuilding);
                building.transform.SetParent(_rootBuildingOnPlanet.transform);
            } while (halfBuildingsOnPlanet > createdBuildings);

            return spawnedDownBuildings;
        }

        private (Vector3, Quaternion) GeneratePositionAndRotation(PlanetCell planetCell)
        {
            var randomX = Random.Range(planetCell.rangeX.x, planetCell.rangeX.y);
            var vectorUp = Vector3.up;
            if (randomX > 90f)
            {
                vectorUp = -Vector3.up; 
            }
            var randomY = Random.Range(planetCell.rangeY.x, planetCell.rangeY.y);
            var randomZ = Random.Range(planetCell.rangeZ.x, planetCell.rangeZ.y);
            var rotation = Quaternion.Euler(randomX, randomY, randomZ);
            var position = rotation * vectorUp * _planetRadius;
            return (position, rotation);
        }
    }
}