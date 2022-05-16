﻿using System.Collections.Generic;
using ScriptableData;
using UnityEngine;
using Utils;

namespace EnvironmentGeneration
{
    public class CheliksOnPlanetGenerator
    {
        private readonly List<GameObject> _cheliksPrefabs;
        private readonly float _planetRadius;
        private readonly GameObject _rootCheliksOnPlanet;
        private readonly List<Transform> _spawnedTopCheliks;
        private readonly List<Transform> _spawnedDownCheliks;
        private readonly int _cheliksOnPlanet;
        
        public CheliksOnPlanetGenerator(AllData data, float planetRadius, GameObject rootEnvironment)
        {
            _cheliksPrefabs = new List<GameObject>(data.Prefab.cheliks.Length);
            _spawnedTopCheliks = new List<Transform>();
            _spawnedDownCheliks = new List<Transform>();
            foreach (var chelik in data.Prefab.cheliks)
            {
                _cheliksPrefabs.Add(chelik);
            }
            _planetRadius = planetRadius;
            _rootCheliksOnPlanet = new GameObject("CheliksOnPlanet");
            _rootCheliksOnPlanet.transform.SetParent(rootEnvironment.transform);
            _cheliksOnPlanet = data.ObjectsOnPlanetData.cheliksOnPlanet;
        }
        
        public List<Transform> CreateTopTreesAndPosition(List<PlanetCell> planetCellsDown)
        {
            var createdCheliks = 0;
            var halfCheliksOnPlanet = Mathf.RoundToInt(_cheliksOnPlanet / 2);
            do
            {
                var randomCell = Random.Range(0, planetCellsDown.Count);
                if (planetCellsDown[randomCell].IsOccupied) continue;
                var tempCell = planetCellsDown[randomCell];
                tempCell.Occupied();
                planetCellsDown[randomCell] = tempCell;
                createdCheliks++;
                var randomTreeType = Random.Range(0, _cheliksPrefabs.Count);
                var positionAndRotation = GeneratePositionAndRotation(planetCellsDown[randomCell]);
                var tree = Object.Instantiate(_cheliksPrefabs[randomTreeType], positionAndRotation.Item1, positionAndRotation.Item2);
                _spawnedTopCheliks.Add(tree.transform);
                tree.transform.SetParent(_rootCheliksOnPlanet.transform);
                // if (!positionAndRotation.Item2) continue;
                // _invisibleBuildings.Add(building);
                // building.SetActive(false);
            } while (halfCheliksOnPlanet > createdCheliks);

            return _spawnedTopCheliks;
        }
        
        public List<Transform> CreateDownTreesAndPosition(List<PlanetCell> planetCellsDown)
        {
            var createdTrees = 0;
            var halfCheliksOnPlanet = Mathf.RoundToInt(_cheliksOnPlanet / 2);
            do
            {
                var randomCell = Random.Range(0, planetCellsDown.Count);
                if (planetCellsDown[randomCell].IsOccupied) continue;
                var tempCell = planetCellsDown[randomCell];
                tempCell.Occupied();
                planetCellsDown[randomCell] = tempCell;
                createdTrees++;
                var randomTreeType = Random.Range(0, _cheliksPrefabs.Count);
                var positionAndRotation = GeneratePositionAndRotation(planetCellsDown[randomCell]);
                var tree = Object.Instantiate(_cheliksPrefabs[randomTreeType], positionAndRotation.Item1, positionAndRotation.Item2);
                _spawnedDownCheliks.Add(tree.transform);
                tree.transform.SetParent(_rootCheliksOnPlanet.transform);
                // if (!positionAndRotation.Item2) continue;
                // _invisibleBuildings.Add(building);
                // building.SetActive(false);
            } while (halfCheliksOnPlanet > createdTrees);

            return _spawnedDownCheliks;
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