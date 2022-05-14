using System;
using System.Collections.Generic;
using Builders;
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
        private readonly Transform _planet;
        
        private readonly List<Transform> _spawnedBuildings;
        private readonly GameObject _rootBuildingOnPlanet;
        private readonly List<GameObject> _invisibleBuildings;

        private readonly FirstTypeHouseBuilder _firstTypeHouseBuilder;
        private readonly SecondTypeHouseBuilder _secondTypeHouseBuilder;
        private readonly ThirdTypeHouseBuilder _thirdTypeHouseBuilder;
        private readonly FourthTypeHouseBuilder _fourthTypeHouseBuilder;
        private readonly FifthTypeHouseBuilder _fifthTypeHouseBuilder;
        private readonly SixthTypeHouseBuilder _sixthTypeHouseBuilder;
        private readonly HouseDirector _houseDirector;

        public BuildingOnPlanetGenerator(AllData data, float planetRadius, GameObject rootEnvironment, Transform planet)
        {
            _planet = planet;
            _invisibleBuildingAngle = data.ObjectsOnPlanetData.flyAroundInvisibleObjectAngle;
            _planetRadius = planetRadius;
            _maximumFloorInHouse = data.ObjectsOnPlanetData.maximumFloorInHouseOnPlanet;
            _maximumAngleRotateBuildingAroundItself =
                data.ObjectsOnPlanetData.maximumAngleRotateBuildingAroundItselfOnPlanet;
            _buildingsOnPlanet = data.ObjectsOnPlanetData.buildingsOnPlanet;
            _invisibleBuildings = new List<GameObject>();
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

        public List<Transform> CreateBuildingAndPosition(List<PlanetCell> planetCells)
        {
            var createdBuildings = 0;
            do
            {
                var randomCell = Random.Range(0, planetCells.Count);
                if (planetCells[randomCell].isOccupied) continue;
                planetCells[randomCell].Occupied();
                createdBuildings++;
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
                var positionAndRotation = GeneratePositionAndRotation(planetCells[randomCell]);
                building.transform.SetPositionAndRotation(positionAndRotation.Item1, Quaternion.identity);
                building.transform.RotateAround(building.transform.position, building.transform.forward,
                    randomAngleRotationBuilding);
                _spawnedBuildings.Add(building.transform);
                building.transform.SetParent(_rootBuildingOnPlanet.transform);
                if (!positionAndRotation.Item2) continue;
                _invisibleBuildings.Add(building);
                building.SetActive(false);
            } while (_buildingsOnPlanet > createdBuildings);

            return _spawnedBuildings;
        }

        private (Vector3, bool) GeneratePositionAndRotation(PlanetCell planetCell)
        {
            var isInvisible = false;
            var randomX = Random.Range(planetCell.rangeX.x, planetCell.rangeX.y);
            var vectorUp = Vector3.up;
            if (randomX < 90f)
            {
                if (randomX > 90f - _invisibleBuildingAngle)
                {
                    isInvisible = true;
                }
            }
            else
            {
                vectorUp = -Vector3.up; 
                if (randomX < 90f + _invisibleBuildingAngle)
                {
                    isInvisible = true;
                }
            }
            var randomY = Random.Range(planetCell.rangeY.x, planetCell.rangeY.y);
            var randomZ = Random.Range(planetCell.rangeZ.x, planetCell.rangeZ.y);
            if (randomZ < 90f)
            {
                if (randomX > 90f - _invisibleBuildingAngle)
                {
                    isInvisible = true;
                }
            }
            else
            {
                if (randomZ < 90f + _invisibleBuildingAngle)
                {
                    isInvisible = true;
                }
            }
            var rotation = Quaternion.Euler(randomX, randomY, randomZ);
            var position = rotation * vectorUp * _planetRadius;
            return (position, isInvisible);
        }
    }
}