using System;
using System.Collections.Generic;
using Builders;
using ScriptableData;
using UnityEngine;
using Random = UnityEngine.Random;

namespace EnvironmentGeneration
{
    public class BuildingOnPlanetGenerator
    {
        private readonly float _maximumBuildingAroundAngleUp;
        private readonly float _maximumBuildingAroundAngleDown;
        private readonly float _maximumBuildingAngleUp;
        private readonly float _maximumBuildingAngleDown;
        private readonly float _invisibleBuildingAngle;
        private readonly Transform _planet;
        private readonly float _planetRadius;
        private readonly Transform _positionGenerator;
        private readonly int _maximumFloorInHouse;
        private readonly float _maximumAngleRotateBuildingAroundItself;
        private readonly int _buildingsOnPlanet;
        
        private readonly List<Vector3> _buildingPositions;
        private readonly List<Vector3> _invisibleBuildingPositions;
        private readonly List<Quaternion> _buildingRotations;
        private readonly List<Quaternion> _invisibleBuildingRotations;
        private readonly List<Transform> _spawnedBuildings;
        private readonly GameObject _rootBuildingOnPlanet;
        private int _invisibleBuildingsCounter;
        private int _buildingsCounter;
        private readonly List<GameObject> _invisibleBuildings;

        private readonly FirstTypeHouseBuilder _firstTypeHouseBuilder;
        private readonly SecondTypeHouseBuilder _secondTypeHouseBuilder;
        private readonly ThirdTypeHouseBuilder _thirdTypeHouseBuilder;
        private readonly FourthTypeHouseBuilder _fourthTypeHouseBuilder;
        private readonly FifthTypeHouseBuilder _fifthTypeHouseBuilder;
        private readonly SixthTypeHouseBuilder _sixthTypeHouseBuilder;
        private readonly HouseDirector _houseDirector;

        public BuildingOnPlanetGenerator(AllData data, Transform planet, float planetRadius, GameObject rootEnvironment)
        {
            _maximumBuildingAroundAngleUp = data.ObjectsOnPlanetData.maximumBuildingAngleUp;
            _maximumBuildingAroundAngleDown = data.ObjectsOnPlanetData.maximumBuildingAngleDown;
            _maximumBuildingAngleUp = 89f;
            _maximumBuildingAngleDown = 89f;
            _invisibleBuildingAngle = data.ObjectsOnPlanetData.flyAroundInvisibleObjectAngle;
            _planet = planet;
            _planetRadius = planetRadius;
            _maximumFloorInHouse = data.ObjectsOnPlanetData.maximumFloorInHouseOnPlanet;
            _maximumAngleRotateBuildingAroundItself =
                data.ObjectsOnPlanetData.maximumAngleRotateBuildingAroundItselfOnPlanet;
            _positionGenerator = new GameObject("BuildingOnPlanetPositionGenerator").GetComponent<Transform>();
            _buildingsOnPlanet = data.ObjectsOnPlanetData.buildingsOnPlanet;
            _invisibleBuildings = new List<GameObject>();
            
            _buildingPositions = new List<Vector3>();
            _buildingRotations = new List<Quaternion>();
            _invisibleBuildingPositions = new List<Vector3>();
            _invisibleBuildingRotations = new List<Quaternion>();
            _spawnedBuildings = new List<Transform>();
            _rootBuildingOnPlanet = new GameObject("BuildingAroundPlanet");
            _rootBuildingOnPlanet.transform.SetParent(rootEnvironment.transform);
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
        }

        private void GeneratePositions(int buildingsAroundPlanet)
        {
            var planetPosition = _planet.position;
            var ray = new Ray(planetPosition, _planet.forward);
            _positionGenerator.position = ray.GetPoint(_planetRadius);
            var iterationAngle = 360f / buildingsAroundPlanet;
            var generateBuildingsOnIteration = Mathf.RoundToInt(_buildingsOnPlanet / buildingsAroundPlanet);
            for (float i = 0; i < buildingsAroundPlanet; i++)
            {
                _positionGenerator.RotateAround(planetPosition, _planet.up, iterationAngle * i);
                for (int j = 0; j < generateBuildingsOnIteration; j++)
                {
                    var upAngle = Random.Range(_maximumBuildingAroundAngleUp, _maximumBuildingAngleUp);
                    var downAngle = Random.Range(_maximumBuildingAroundAngleDown, _maximumBuildingAngleDown);
                    TakeUpPosition(_positionGenerator, upAngle, planetPosition);
                    TakeDownPosition(_positionGenerator, downAngle, planetPosition);
                }
            }
        }

        private void TakeUpPosition(Transform generatorTransform, float upAngle, Vector3 planetPosition)
        {
            generatorTransform.RotateAround(planetPosition, _planet.forward, upAngle);
            if (upAngle < _invisibleBuildingAngle)
            {
                _invisibleBuildingPositions.Add(generatorTransform.position);
                _invisibleBuildingRotations.Add(generatorTransform.rotation);
                _invisibleBuildingsCounter++;
            }
            else
            {
                _buildingPositions.Add(generatorTransform.position);
                _buildingRotations.Add(generatorTransform.rotation);
                _buildingsCounter++;
            }
            generatorTransform.RotateAround(planetPosition, _planet.forward, -upAngle);
        }

        private void TakeDownPosition(Transform generatorTransform, float downAngle, Vector3 planetPosition)
        {
            generatorTransform.RotateAround(planetPosition, _planet.up, -downAngle);
            if (downAngle < _invisibleBuildingAngle)
            {
                _invisibleBuildingPositions.Add(generatorTransform.position);
                _invisibleBuildingRotations.Add(generatorTransform.rotation);
                _invisibleBuildingsCounter++;
            }
            else
            {
                _buildingPositions.Add(generatorTransform.position);
                _buildingRotations.Add(generatorTransform.rotation);
                _buildingsCounter++;
            }
            generatorTransform.RotateAround(planetPosition, _planet.up, downAngle);
        }
        
        public List<Transform> CreateBuildingAndPosition(int buildingsAroundPlanet)
        {
            CreateBuildings(buildingsAroundPlanet);
            CreateInvisibleBuildings();
            return _spawnedBuildings;
        }

        private void CreateBuildings(int buildingsAroundPlanet)
        {
            GeneratePositions(buildingsAroundPlanet);
            for (int i = 0; i < _buildingsCounter; i++)
            {
                var randomAngleRotationBuilding = Random.Range(0f, _maximumAngleRotateBuildingAroundItself);
                var randomFloors = Random.Range(1, _maximumFloorInHouse);
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

                var building = _houseDirector.BuildSimpleHouse(randomFloors);
                building.transform.SetPositionAndRotation(_buildingPositions[i], _buildingRotations[i]);
                building.transform.RotateAround(building.transform.position, building.transform.forward, randomAngleRotationBuilding);
                _spawnedBuildings.Add(building.transform);
                building.transform.SetParent(_rootBuildingOnPlanet.transform);
            }
        }

        private void CreateInvisibleBuildings()
        {
            for (int i = 0; i < _invisibleBuildingsCounter; i++)
            {
                var randomAngleRotationBuilding = Random.Range(0f, _maximumAngleRotateBuildingAroundItself);
                var randomFloors = Random.Range(1, _maximumFloorInHouse);
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

                var building = _houseDirector.BuildSimpleHouse(randomFloors);
                building.transform.SetPositionAndRotation(_invisibleBuildingPositions[i], _invisibleBuildingRotations[i]);
                building.transform.RotateAround(building.transform.position, building.transform.forward, randomAngleRotationBuilding);
                _spawnedBuildings.Add(building.transform);
                building.transform.SetParent(_rootBuildingOnPlanet.transform);
                _invisibleBuildings.Add(building);
                building.SetActive(false);
            }
        }
    }
}