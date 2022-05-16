using System.Collections.Generic;
using ScriptableData;
using UnityEngine;
using Utils;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace EnvironmentGeneration
{
    public class TreesOnPlanetGenerator
    {
        private readonly List<GameObject> _treesPrefabs;
        private readonly float _planetRadius;
        private readonly GameObject _rootTreesOnPlanet;
        private readonly List<Transform> _spawnedTopTrees;
        private readonly List<Transform> _spawnedDownTrees;
        private readonly int _treesOnPlanet;

        public TreesOnPlanetGenerator(AllData data, float planetRadius, GameObject rootEnvironment)
        {
            _treesPrefabs = new List<GameObject>(data.Prefab.trees.Length);
            _spawnedTopTrees = new List<Transform>();
            _spawnedDownTrees = new List<Transform>();
            foreach (var tree in data.Prefab.trees)
            {
                _treesPrefabs.Add(tree);
            }

            _planetRadius = planetRadius;
            _rootTreesOnPlanet = new GameObject("TreesOnPlanet");
            _rootTreesOnPlanet.transform.SetParent(rootEnvironment.transform);
            _treesOnPlanet = data.ObjectsOnPlanetData.treesOnPlanet;
        }
        
        public List<Transform> CreateTopTreesAndPosition(List<PlanetCell> planetCellsDown)
        {
            var createdTrees = 0;
            var halfTreesOnPlanet = Mathf.RoundToInt(_treesOnPlanet / 2);
            do
            {
                var randomCell = Random.Range(0, planetCellsDown.Count);
                if (planetCellsDown[randomCell].IsOccupied) continue;
                var tempCell = planetCellsDown[randomCell];
                tempCell.Occupied();
                planetCellsDown[randomCell] = tempCell;
                createdTrees++;
                var randomTreeType = Random.Range(0, _treesPrefabs.Count);
                var positionAndRotation = GeneratePositionAndRotation(planetCellsDown[randomCell]);
                var tree = Object.Instantiate(_treesPrefabs[randomTreeType], positionAndRotation.Item1, positionAndRotation.Item2);
                _spawnedTopTrees.Add(tree.transform);
                //TODO: here can realize rotate around itself
                //tree.transform.RotateAround(tree.transform.position, tree.transform.up, randomAngleRotationBuilding);
                tree.transform.SetParent(_rootTreesOnPlanet.transform);
                // if (!positionAndRotation.Item2) continue;
                // _invisibleBuildings.Add(building);
                // building.SetActive(false);
            } while (halfTreesOnPlanet > createdTrees);

            return _spawnedTopTrees;
        }
        
        public List<Transform> CreateDownTreesAndPosition(List<PlanetCell> planetCellsDown)
        {
            var createdTrees = 0;
            var halfTreesOnPlanet = Mathf.RoundToInt(_treesOnPlanet / 2);
            do
            {
                var randomCell = Random.Range(0, planetCellsDown.Count);
                if (planetCellsDown[randomCell].IsOccupied) continue;
                var tempCell = planetCellsDown[randomCell];
                tempCell.Occupied();
                planetCellsDown[randomCell] = tempCell;
                createdTrees++;
                var randomTreeType = Random.Range(0, _treesPrefabs.Count);
                var positionAndRotation = GeneratePositionAndRotation(planetCellsDown[randomCell]);
                var tree = Object.Instantiate(_treesPrefabs[randomTreeType], positionAndRotation.Item1, positionAndRotation.Item2);
                _spawnedDownTrees.Add(tree.transform);
                //tree.transform.RotateAround(tree.transform.position, tree.transform.up, randomAngleRotationBuilding);
                tree.transform.SetParent(_rootTreesOnPlanet.transform);
                // if (!positionAndRotation.Item2) continue;
                // _invisibleBuildings.Add(building);
                // building.SetActive(false);
            } while (halfTreesOnPlanet > createdTrees);

            return _spawnedDownTrees;
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