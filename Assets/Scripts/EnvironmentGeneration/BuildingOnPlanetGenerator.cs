using System;
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
        private readonly List<GameObject> _invisibleBuildings;

        private readonly HouseBuilder[] _houseBuilders;
        // TODO Убрать если HauseBuilder работает
        //private readonly FirstTypeHouseBuilder _firstTypeHouseBuilder;
        //private readonly SecondTypeHouseBuilder _secondTypeHouseBuilder;
        //private readonly ThirdTypeHouseBuilder _thirdTypeHouseBuilder;
        //private readonly FourthTypeHouseBuilder _fourthTypeHouseBuilder;
        //private readonly FifthTypeHouseBuilder _fifthTypeHouseBuilder;
        //private readonly SixthTypeHouseBuilder _sixthTypeHouseBuilder;
        private readonly HouseDirector _houseDirector;

        public BuildingOnPlanetGenerator(AllData data, float planetRadius, GameObject rootEnvironment)
        {
            _invisibleBuildingAngle = data.ObjectsOnPlanetData.flyAroundInvisibleObjectAngle;
            _planetRadius = planetRadius;
            _maximumFloorInHouse = data.ObjectsOnPlanetData.maximumFloorInHouseOnPlanet;
            _maximumAngleRotateBuildingAroundItself =
                data.ObjectsOnPlanetData.maximumAngleRotateBuildingAroundItselfOnPlanet;
            _buildingsOnPlanet = data.ObjectsOnPlanetData.buildingsOnPlanet;
            _invisibleBuildings = new List<GameObject>();
            _spawnedTopBuildings = new List<Transform>();
            _spawnedDownBuildings = new List<Transform>();
            
            _rootBuildingOnPlanet = new GameObject("BuildingsOnPlanet");
            _rootBuildingOnPlanet.transform.SetParent(rootEnvironment.transform);

            _houseBuilders = new HouseBuilder[6] {
                new HouseBuilder(1),
                new HouseBuilder(2),
                new HouseBuilder(3),
                new HouseBuilder(4),
                new HouseBuilder(5),
                new HouseBuilder(6)
            };
            // TODO Убрать если HauseBuilder работает
            //_firstTypeHouseBuilder = new FirstTypeHouseBuilder();
            //_secondTypeHouseBuilder = new SecondTypeHouseBuilder();
            //_thirdTypeHouseBuilder = new ThirdTypeHouseBuilder();
            //_fourthTypeHouseBuilder = new FourthTypeHouseBuilder();
            //_fifthTypeHouseBuilder = new FifthTypeHouseBuilder();
            //_sixthTypeHouseBuilder = new SixthTypeHouseBuilder();
            //_houseDirector = new HouseDirector
            //{
            //    Builder = _firstTypeHouseBuilder
            //};
            _houseDirector = new HouseDirector
            {
                Builder = _houseBuilders[0]
            };
        }

        public List<Transform> CreateTopBuildingAndPosition(List<PlanetCell> planetCellsTop)
        {
            var createdBuildings = 0;
            var halfBuildingsOnPlanet = _buildingsOnPlanet / 2;
            do
            {
                var randomCell = Random.Range(0, planetCellsTop.Count);
                if (planetCellsTop[randomCell].IsOccupied) continue;
                var tempCell = planetCellsTop[randomCell];
                tempCell.Occupied();
                planetCellsTop[randomCell] = tempCell;
                createdBuildings++;
                var randomAngleRotationBuilding = Random.Range(0f, _maximumAngleRotateBuildingAroundItself);
                var randomFloors = Random.Range(1, _maximumFloorInHouse);
                var randomBuildingType = Random.Range(0, 5);
                _houseDirector.Builder = _houseBuilders[randomBuildingType];
                // TODO Убрать если HauseBuilder работает
                //switch (randomBuildingType)
                //{
                //    case 0:
                //        _houseDirector.Builder = _firstTypeHouseBuilder;
                //        break;
                //    case 1:
                //        _houseDirector.Builder = _secondTypeHouseBuilder;
                //        break;
                //    case 2:
                //        _houseDirector.Builder = _thirdTypeHouseBuilder;
                //        break;
                //    case 3:
                //        _houseDirector.Builder = _fourthTypeHouseBuilder;
                //        break;
                //    case 4:
                //        _houseDirector.Builder = _fifthTypeHouseBuilder;
                //        break;
                //    case 5:
                //        _houseDirector.Builder = _sixthTypeHouseBuilder;
                //        break;
                //    default:
                //        throw new ArgumentOutOfRangeException("Out of range type in generate building");
                //}
                var building = _houseDirector.BuildSimpleHouse(randomFloors);
                var positionAndRotation = GeneratePositionAndRotation(planetCellsTop[randomCell]);
                building.transform.SetPositionAndRotation(positionAndRotation.Item1, positionAndRotation.Item2);
                building.transform.RotateAround(building.transform.position, building.transform.up,
                    randomAngleRotationBuilding);
                //building.transform.rotation.SetLookRotation(Vector3.zero, building.transform.forward);
                //building.transform.Rotate(building.transform.right, 180f);
                _spawnedTopBuildings.Add(building.transform);
                building.transform.SetParent(_rootBuildingOnPlanet.transform);
                // if (!positionAndRotation.Item2) continue;
                // _invisibleBuildings.Add(building);
                // building.SetActive(false);
            } while (halfBuildingsOnPlanet > createdBuildings);

            return _spawnedTopBuildings;
        }
        
        public List<Transform> CreateDownBuildingAndPosition(List<PlanetCell> planetCellsDown)
        {
            var createdBuildings = 0;
            var halfBuildingsOnPlanet = _buildingsOnPlanet / 2;
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
                // TODO Убрать если HauseBuilder работает
                //switch (randomBuildingType)
                //{
                //    case 0:
                //        _houseDirector.Builder = _firstTypeHouseBuilder;
                //        break;
                //    case 1:
                //        _houseDirector.Builder = _secondTypeHouseBuilder;
                //        break;
                //    case 2:
                //        _houseDirector.Builder = _thirdTypeHouseBuilder;
                //        break;
                //    case 3:
                //        _houseDirector.Builder = _fourthTypeHouseBuilder;
                //        break;
                //    case 4:
                //        _houseDirector.Builder = _fifthTypeHouseBuilder;
                //        break;
                //    case 5:
                //        _houseDirector.Builder = _sixthTypeHouseBuilder;
                //        break;
                //    default:
                //        throw new ArgumentOutOfRangeException("Out of range type in generate building");
                //}
                var building = _houseDirector.BuildSimpleHouse(randomFloors);
                var positionAndRotation = GeneratePositionAndRotation(planetCellsDown[randomCell]);
                building.transform.SetPositionAndRotation(positionAndRotation.Item1, positionAndRotation.Item2);
                building.transform.RotateAround(building.transform.position, building.transform.forward, 180f);
                _spawnedDownBuildings.Add(building.transform);
                building.transform.RotateAround(building.transform.position, building.transform.up,
                    randomAngleRotationBuilding);
                building.transform.SetParent(_rootBuildingOnPlanet.transform);
                // if (!positionAndRotation.Item2) continue;
                // _invisibleBuildings.Add(building);
                // building.SetActive(false);
            } while (halfBuildingsOnPlanet > createdBuildings);

            return _spawnedDownBuildings;
        }

        private (Vector3, Quaternion, bool) GeneratePositionAndRotation(PlanetCell planetCell)
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
            return (position, rotation, isInvisible);
        }
    }
}