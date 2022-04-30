using System;
using System.Collections.Generic;
using System.Linq;
using Builders;
using ScriptableData;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Controllers

{
    public class BuildingsController
    {
        private readonly AllData _data;
        private readonly Transform _planet;
        private readonly float _planetRadius;
        private readonly Transform _positionGenerator;
        private readonly float _minimumAngleBetweenBuildings;
        private readonly float _maximumAngleBetweenBuildings;
        private readonly float _maximumBuildingAngleUp;
        private readonly float _maximumBuildingAngleDown;
        private readonly float _maximumAngleRotateBuildingAroundItself;
        private readonly int _maximumFloorsInHouse;
        private readonly int _buildingWithGlass;
        
        private int _buildingsCounter;
        private readonly GameObject _rootBuildingAroundPlanet;
        private readonly FirstTypeHouseBuilder _firstTypeHouseBuilder;
        private readonly SecondTypeHouseBuilder _secondTypeHouseBuilder;
        private readonly ThirdTypeHouseBuilder _thirdTypeHouseBuilder;
        private readonly FourthTypeHouseBuilder _fourthTypeHouseBuilder;
        private readonly HouseDirector _houseDirector;
        private readonly List<Vector3> _buildingPositions;
        private readonly List<Quaternion> _buildingRotations;

        public BuildingsController(AllData data, Transform planet, Transform positionGenerator)
        {
            _data = data;
            _planet = planet;
            _planetRadius = planet.GetComponent<SphereCollider>().radius;
            _positionGenerator = positionGenerator;
            _minimumAngleBetweenBuildings = data.ObjectsOnPlanetData.minimalAngleBetweenBuildings;
            _maximumAngleBetweenBuildings = data.ObjectsOnPlanetData.maximumAngleBetweenBuildings;
            _maximumBuildingAngleUp = data.ObjectsOnPlanetData.maximumBuildingAngleUp;
            _maximumBuildingAngleDown = data.ObjectsOnPlanetData.maximumBuildingAngleDown;
            _maximumAngleRotateBuildingAroundItself = data.ObjectsOnPlanetData.maximumAngleRotateBuildingAroundItself;
            _maximumFloorsInHouse = data.ObjectsOnPlanetData.maximumFloorInHouse;
            _buildingWithGlass = data.ObjectsOnPlanetData.buildingsWithBonus;

            _buildingPositions = new List<Vector3>();
            _buildingRotations = new List<Quaternion>();
            var rootEnvironment = new GameObject("PlanetEnvironment");
            _rootBuildingAroundPlanet = new GameObject("BuildingAroundPlanet");
            _rootBuildingAroundPlanet.transform.SetParent(rootEnvironment.transform);
            _firstTypeHouseBuilder = new FirstTypeHouseBuilder();
            _secondTypeHouseBuilder = new SecondTypeHouseBuilder();
            _thirdTypeHouseBuilder = new ThirdTypeHouseBuilder();
            _fourthTypeHouseBuilder = new FourthTypeHouseBuilder();
            _houseDirector = new HouseDirector
            {
                Builder = _firstTypeHouseBuilder
            };
        }

        #region Generate_Buildings_Around_Planet

        public void GenerateBuildingsAroundPlanet()
        {
            GeneratePositions();
            CreateBuildingAndPosition();
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
        
        private void CreateBuildingAndPosition()
        {
            var numbersBuildingsWithGlass = TakeRandomFloorNumber(_buildingsCounter);
            for (int i = 0; i < _buildingsCounter; i++)
            {
                var randomAngleRotationBuilding = Random.Range(0f, _maximumAngleRotateBuildingAroundItself);
                var randomFloors = Random.Range(1, _maximumFloorsInHouse);
                var randomBuildingType = Random.Range(0, 3);
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
                    default:
                        throw new ArgumentOutOfRangeException("Out of range type in generate building");
                }

                var isGlassHouse = numbersBuildingsWithGlass.Any(buildingWithGlass => i == buildingWithGlass);
                var building = isGlassHouse ? _houseDirector.BuildGlassHouse(randomFloors) : _houseDirector.BuildSimpleHouse(randomFloors);
                building.transform.SetPositionAndRotation(_buildingPositions[i], _buildingRotations[i]);
                building.transform.RotateAround(building.transform.position, building.transform.forward, randomAngleRotationBuilding);
                building.transform.SetParent(_rootBuildingAroundPlanet.transform);
            }
        }
        
        #endregion
        
        private void CreateBuildingsByPrefab(Transform planet, GameObject buildingPrefab, int buildingsCount)
        {
            
            var buildingHeight = buildingPrefab.GetComponent<BoxCollider>().size.y;
            planet.GetComponent<SphereCollider>();
            for (int i = 1; i <= buildingsCount; i++)
            {
                var x = Random.Range(0f, 360f);
                var y = Random.Range(0f, 360f);
                var z = Random.Range(0f, 360f);
                Quaternion rotation = Quaternion.Euler(x, y, z);
                Vector3 position = planet.position + rotation * Vector3.up * (_planetRadius + buildingHeight / 2);
                Object.Instantiate(buildingPrefab, position, rotation);
            }
        }
        
        public void CreateBuildings(Transform planet)
        {
            var objectTypes = _data.ObjectsOnPlanetData.objectsTypeOnPlanet;
            foreach (var objectType in objectTypes)
            {
                CreateBuildingsByPrefab(planet, objectType.prefab, objectType.count);
            }
        }
    }
}
