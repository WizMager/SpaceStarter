using System.Collections.Generic;
using System.Linq;
using Builders.HouseBuilder;
using Controllers;
using ScriptableData;
using UnityEngine;
using View;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace EnvironmentGeneration
{
    public class BuildingAroundPlanetGenerator
    {
        private readonly Transform _positionGenerator;
        private readonly Transform _planet;
        private readonly float _planetRadius;
        #region Buildings
        private readonly float _minimumAngleBetweenBuildings;
        private readonly float _maximumAngleBetweenBuildings;
        private readonly float _maximumBuildingAngleUp;
        private readonly float _maximumBuildingAngleDown;
        private readonly float _maximumAngleRotateBuildingAroundItself;
        private readonly int _maximumFloorsInHouse;
        private readonly int _buildingWithGlass;
        private readonly StateController _stateController;
        
        private int _buildingsCounter;
        private readonly GameObject _rootBuildingAroundPlanet;
        private readonly HouseBuilder[] _houseBuilders;
        
        private readonly HouseDirector _houseDirector;
        private readonly List<Vector3> _buildingPositions;
        private readonly List<Quaternion> _buildingRotations;
        private readonly List<Transform> _spawnedBuildings;
        #endregion
        #region Trees
        private readonly float _minimumAngleBetweenTrees;
        private readonly float _maximumAngleBetweenTrees;

        private int _treesCounter;

        private readonly GameObject _rootTreesAroundPlanet;

        private readonly List<GameObject> _treesPrefabs;

        private readonly List<Transform> _spawnedTrees;
        private readonly List<Vector3> _treesPositions;
        private readonly List<Quaternion> _treesRotations;       
        #endregion
        public BuildingAroundPlanetGenerator(StateController stateController, AllData data, Transform planet, float planetRadius, 
            GameObject rootEnvironment, Dictionary<int, List<Material>> floorMaterials, Dictionary<int, List<Material>> treesMaterials)
        {
            _stateController = stateController;
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
            _houseBuilders = new [] { 
                new HouseBuilder(1, floorMaterials[0], floorMaterials[1]),
                new HouseBuilder(2, floorMaterials[0], floorMaterials[2]),
                new HouseBuilder(3, floorMaterials[0], floorMaterials[3]),
                new HouseBuilder(4, floorMaterials[0], floorMaterials[4]),
                new HouseBuilder(5, floorMaterials[0], floorMaterials[5]),
                new HouseBuilder(6, floorMaterials[0], floorMaterials[6])
            };
            _houseDirector = new HouseDirector
            {
                Builder = _houseBuilders[0]
            };
            _spawnedBuildings = new List<Transform>();

            _maximumAngleBetweenTrees = data.ObjectsOnPlanetData.maximumAngleBetweenTrees;
            _minimumAngleBetweenTrees = data.ObjectsOnPlanetData.minimalAngleBetweenTrees;

            _treesPrefabs = new List<GameObject>(data.Prefab.trees.Length);
            for (int i = 0; i < data.Prefab.trees.Length; i++)
            {
                _treesPrefabs.Add(PaintTree(data.Prefab.trees[i], treesMaterials[i]));
            }

            _rootTreesAroundPlanet = new GameObject("TreesAroundPlanet");
            
            _spawnedTrees = new List<Transform>();
            _treesPositions = new List<Vector3>();
            _treesRotations = new List<Quaternion>();
        }

        private GameObject PaintTree(GameObject tree, List<Material> materials)
        {
            var meshRenderers = tree.GetComponentsInChildren<MeshRenderer>();
            foreach (var meshRenderer in meshRenderers)
            {
                if (meshRenderer.gameObject.CompareTag("Crown"))
                {
                    meshRenderer.material = materials[0];
                }

                if (meshRenderer.gameObject.CompareTag("Trunk"))
                {
                    meshRenderer.material = materials[1];
                }
            }
            return tree;
        }
        
        public List<Transform> GenerateBuildingsAroundPlanet()
        {
            GeneratePositionsForBuildings();
            return CreateBuildingAndPosition();
        }
        public List<Transform> GenerateTreesAroundPlanet()
        {
            GeneratePositionsForTrees();
            return CreateTreeAndPosition();
        }
        
        private void GeneratePositionsForBuildings()
        {
            var planetPosition = _planet.position;
            var ray = new Ray(planetPosition, _planet.forward);
            _positionGenerator.position = ray.GetPoint(_planetRadius);
            _positionGenerator.RotateAround(_positionGenerator.transform.position, _planet.right, 90f);
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
                _positionGenerator.RotateAround(planetPosition, _planet.right, upOrDownAngle);
                _buildingPositions.Add(_positionGenerator.position);
                _buildingRotations.Add(_positionGenerator.rotation);
                _positionGenerator.RotateAround(planetPosition, _planet.right, -upOrDownAngle);
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
                _houseDirector.Builder = _houseBuilders[randomBuildingType];

                var isGlassHouse = numbersBuildingsWithGlass.Any(buildingWithGlass => i == buildingWithGlass);
                var building = isGlassHouse ? _houseDirector.BuildGlassHouse(randomFloors) : _houseDirector.BuildSimpleHouse(randomFloors);
                building.transform.SetPositionAndRotation(_buildingPositions[i], _buildingRotations[i]);
                building.transform.RotateAround(building.transform.position, building.transform.up, randomAngleRotationBuilding);
                _spawnedBuildings.Add(building.transform);
                building.transform.SetParent(_rootBuildingAroundPlanet.transform);
            }

            return _spawnedBuildings;
        }

        private void GeneratePositionsForTrees()
        {
            var planetPosition = _planet.position;
            var ray = new Ray(planetPosition, _planet.forward);
            _positionGenerator.position = ray.GetPoint(_planetRadius);
            _positionGenerator.RotateAround(_positionGenerator.transform.position, _planet.right, 90f);
            for (float i = 0; i < 360f;)
            {
                var iterationAngle = Random.Range(_minimumAngleBetweenTrees, _maximumAngleBetweenTrees);
                if (360f - i < _minimumAngleBetweenTrees)
                {
                    return;
                }
                i += iterationAngle;
                _positionGenerator.RotateAround(planetPosition, _planet.up, iterationAngle);
                var upOrDownAngle = Random.Range(-_maximumBuildingAngleDown, _maximumBuildingAngleUp);
                _positionGenerator.RotateAround(planetPosition, _planet.right, upOrDownAngle);
                _treesPositions.Add(_positionGenerator.position);
                _treesRotations.Add(_positionGenerator.rotation);
                _positionGenerator.RotateAround(planetPosition, _planet.right, -upOrDownAngle);
                _treesCounter++;
            }
        }
        private List<Transform> CreateTreeAndPosition()
        {
            for(int i = 0; i < _treesCounter; i++)
            {
                var randomTreeType = Random.Range(0, _treesPrefabs.Count);
                var tree = Object.Instantiate(_treesPrefabs[randomTreeType]);
                tree.GetComponent<ObjectOnPlanet>().GetStateController(_stateController);
                tree.transform.SetPositionAndRotation(_treesPositions[i], _treesRotations[i]);
                tree.transform.RotateAround(tree.transform.position, tree.transform.right, 270);
                _spawnedTrees.Add(tree.transform);
                tree.transform.SetParent(_rootTreesAroundPlanet.transform);
            }
            return _spawnedTrees;
        }
    }
}