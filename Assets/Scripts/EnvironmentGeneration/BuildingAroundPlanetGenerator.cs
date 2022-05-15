using System;
using System.Collections.Generic;
using System.Linq;
using Builders;
using ScriptableData;
using UnityEngine;
using Random = UnityEngine.Random;

namespace EnvironmentGeneration
{
    public class BuildingAroundPlanetGenerator
    {
        private readonly float _minimumAngleBetweenBuildings;
        private readonly float _maximumAngleBetweenBuildings;
        private readonly float _maximumBuildingAngleUp;
        private readonly float _maximumBuildingAngleDown;
        private readonly float _maximumAngleRotateBuildingAroundItself;
        private readonly int _maximumFloorsInHouse;
        private readonly int _buildingWithGlass;
        private readonly Transform _positionGenerator;
        private readonly Transform _planet;
        private readonly float _planetRadius;
        
        private int _buildingsCounter;
        private readonly GameObject _rootBuildingAroundPlanet;
        private readonly FirstTypeHouseBuilder _firstTypeHouseBuilder;
        private readonly SecondTypeHouseBuilder _secondTypeHouseBuilder;
        private readonly ThirdTypeHouseBuilder _thirdTypeHouseBuilder;
        private readonly FourthTypeHouseBuilder _fourthTypeHouseBuilder;
        private readonly FifthTypeHouseBuilder _fifthTypeHouseBuilder;
        private readonly SixthTypeHouseBuilder _sixthTypeHouseBuilder;
        private readonly HouseDirector _houseDirector;
        private readonly List<Vector3> _buildingPositions;
        private readonly List<Quaternion> _buildingRotations;
        private readonly List<Transform> _spawnedBuildings;

        public BuildingAroundPlanetGenerator(AllData data, Transform planet, float planetRadius, GameObject rootEnvironment)
        {
            _minimumAngleBetweenBuildings = data.ObjectsOnPlanetData.minimalAngleBetweenBuildings;
            _maximumAngleBetweenBuildings = data.ObjectsOnPlanetData.maximumAngleBetweenBuildings;
            _maximumBuildingAngleUp = data.ObjectsOnPlanetData.maximumBuildingAngleUp;
            _maximumBuildingAngleDown = data.ObjectsOnPlanetData.maximumBuildingAngleDown;
            _maximumAngleRotateBuildingAroundItself = data.ObjectsOnPlanetData.maximumAngleRotateBuildingAroundItselfAroundPlanet;
            _maximumFloorsInHouse = data.ObjectsOnPlanetData.maximumFloorInHouseAroundPlanet;
            _buildingWithGlass = data.ObjectsOnPlanetData.buildingsWithBonus;
            _positionGenerator = new GameObject("BuildingAroundPlanetPositionGenerator").GetComponent<Transform>();
            _planet = planet;
            _planetRadius = planetRadius;

            _buildingPositions = new List<Vector3>();
            _buildingRotations = new List<Quaternion>();
            
            _rootBuildingAroundPlanet = new GameObject("BuildingAroundPlanet");
            _rootBuildingAroundPlanet.transform.SetParent(rootEnvironment.transform);
            _firstTypeHouseBuilder = new FirstTypeHouseBuilder();
            _secondTypeHouseBuilder = new SecondTypeHouseBuilder();
            _thirdTypeHouseBuilder = new ThirdTypeHouseBuilder();
            _fourthTypeHouseBuilder = new FourthTypeHouseBuilder();
            _fifthTypeHouseBuilder = new FifthTypeHouseBuilder();
            _sixthTypeHouseBuilder = new SixthTypeHouseBuilder();
            _houseDirector = new HouseDirector
            {
                Builder = _firstTypeHouseBuilder
            };
            _spawnedBuildings = new List<Transform>();
        }

        public List<Transform> GenerateBuildingsAroundPlanet()
        {
            GeneratePositions();
            return CreateBuildingAndPosition();
        }
        
        private void GeneratePositions()
        {
            var planetPosition = _planet.position;
            var ray = new Ray(planetPosition, _planet.forward);
            _positionGenerator.position = ray.GetPoint(_planetRadius);
            for (float i = 0; i < 360f; )
            {
                var iterationAngle = Random.Range(_minimumAngleBetweenBuildings, _maximumAngleBetweenBuildings);
                if (360f - i < _minimumAngleBetweenBuildings)
                {
                    return;
                }
                i += iterationAngle;
                _positionGenerator.RotateAround(planetPosition, _planet.up, iterationAngle);
                var upOrDownAngle = Random.Range(-_maximumBuildingAngleDown, _maximumBuildingAngleUp);
                _positionGenerator.RotateAround(planetPosition, _planet.forward, upOrDownAngle);
                _buildingPositions.Add(_positionGenerator.position);
                _buildingRotations.Add(_positionGenerator.rotation);
                _positionGenerator.RotateAround(planetPosition, _planet.forward, -upOrDownAngle);
                _buildingsCounter++;
            }
        }

        private List<int> TakeRandomFloorNumber(int numberOfHouses)
        {
            var randomBuildings = new List<int> {Random.Range(0, numberOfHouses)};
            while (randomBuildings.Count < _buildingWithGlass)
            {
                var randomBuildingNumber = Random.Range(0, numberOfHouses);
                var isExistInList = randomBuildings.Any(numberInList => numberInList == randomBuildingNumber);
                if (!isExistInList)
                {
                    randomBuildings.Add(randomBuildingNumber);
                }
            }
            return randomBuildings;
        }
        
        private List<Transform> CreateBuildingAndPosition()
        {
            var numbersBuildingsWithGlass = TakeRandomFloorNumber(_buildingsCounter);
            for (int i = 0; i < _buildingsCounter; i++)
            {
                var randomAngleRotationBuilding = Random.Range(0f, _maximumAngleRotateBuildingAroundItself);
                var randomFloors = Random.Range(1, _maximumFloorsInHouse);
                var randomBuildingType = Random.Range(0, 5);
                switch (randomBuildingType)
                {
                    case 0: 
                        _houseDirector.Builder = _firstTypeHouseBuilder;
                        break;
                    case 1:
                        _houseDirector.Builder = _secondTypeHouseBuilder;
                        break;
                    case 2: 
                        _houseDirector.Builder = _thirdTypeHouseBuilder;
                        break;
                    case 3:
                        _houseDirector.Builder = _fourthTypeHouseBuilder;
                        break;
                    case 4:
                        _houseDirector.Builder = _fifthTypeHouseBuilder;
                        break;
                    case 5:
                        _houseDirector.Builder = _sixthTypeHouseBuilder;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("Out of range type in generate building");
                }

                var isGlassHouse = numbersBuildingsWithGlass.Any(buildingWithGlass => i == buildingWithGlass);
                var building = isGlassHouse ? _houseDirector.BuildGlassHouse(randomFloors) : _houseDirector.BuildSimpleHouse(randomFloors);
                building.transform.SetPositionAndRotation(_buildingPositions[i], _buildingRotations[i]);
                building.transform.RotateAround(building.transform.position, building.transform.forward, randomAngleRotationBuilding);
                _spawnedBuildings.Add(building.transform);
                building.transform.SetParent(_rootBuildingAroundPlanet.transform);
            }

            return _spawnedBuildings;
        }
    }
}