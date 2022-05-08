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
        private readonly float _maximumBuildingAngleUp;
        private readonly float _maximumBuildingAngleDown;
        private readonly float _invisibleBuildingAngle;
        private readonly Transform _planet;
        private readonly float _planetRadius;
        private readonly Transform _positionGenerator;
        private readonly int _maximumFloorInHouse;
        private readonly int _buildingsAroundPlanet;
        private readonly float _maximumAngleRotateBuildingAroundItself;
        
        private readonly List<Vector3> _buildingPositions;
        private readonly List<Vector3> _invisibleBuildingPositions;
        private readonly List<Quaternion> _buildingRotations;
        private readonly List<Vector3> _invisibleBuildingRotations;
        private readonly List<Transform> _spawnedBuildings;
        private readonly GameObject _rootBuildingOnPlanet;
        private readonly FirstTypeHouseBuilder _firstTypeHouseBuilder;
        private readonly SecondTypeHouseBuilder _secondTypeHouseBuilder;
        private readonly ThirdTypeHouseBuilder _thirdTypeHouseBuilder;
        private readonly FourthTypeHouseBuilder _fourthTypeHouseBuilder;
        private readonly FifthTypeHouseBuilder _fifthTypeHouseBuilder;
        private readonly SixthTypeHouseBuilder _sixthTypeHouseBuilder;
        private readonly HouseDirector _houseDirector;

        public BuildingOnPlanetGenerator(AllData data, Transform planet, float planetRadius, GameObject rootEnvironment, int buildingsAroundPlanet)
        {
            _maximumBuildingAngleUp = 89f - data.ObjectsOnPlanetData.maximumBuildingAngleUp;
            _maximumBuildingAngleDown = 89f - data.ObjectsOnPlanetData.maximumBuildingAngleDown;
            _invisibleBuildingAngle = data.ObjectsOnPlanetData.flyAroundInvisibleObjectAngle;
            _planet = planet;
            _planetRadius = planetRadius;
            _maximumFloorInHouse = data.ObjectsOnPlanetData.maximumFloorInHouseOnPlanet;
            _buildingsAroundPlanet = buildingsAroundPlanet;
            _maximumAngleRotateBuildingAroundItself =
                data.ObjectsOnPlanetData.maximumAngleRotateBuildingAroundItselfOnPlanet;
            _positionGenerator = new GameObject("BuildingOnPlanetPositionGenerator").GetComponent<Transform>();
            
            _buildingPositions = new List<Vector3>();
            _buildingRotations = new List<Quaternion>();
            _invisibleBuildingPositions = new List<Vector3>();
            _invisibleBuildingRotations = new List<Vector3>();
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
        
        private void GeneratePositions()
        {
            var planetPosition = _planet.position;
            var ray = new Ray(planetPosition, _planet.forward);
            _positionGenerator.position = ray.GetPoint(_planetRadius);
            for (float i = 0; i < _buildingsAroundPlanet; i++)
            {
                var iterationAngle = 360f / _buildingsAroundPlanet;
                _positionGenerator.RotateAround(planetPosition, _planet.up, iterationAngle);
                
                var upOrDownAngle = Random.Range(-_maximumBuildingAngleDown, _maximumBuildingAngleUp);
                _positionGenerator.RotateAround(planetPosition, _planet.forward, upOrDownAngle);
                _buildingPositions.Add(_positionGenerator.position);
                _buildingRotations.Add(_positionGenerator.rotation);
                _positionGenerator.RotateAround(planetPosition, _planet.forward, -upOrDownAngle);
            }
        }
        
        private List<Transform> CreateBuildingAndPosition()
        {
            for (int i = 0; i < _buildingsAroundPlanet; i++)
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

            return _spawnedBuildings;
        }
    }
}