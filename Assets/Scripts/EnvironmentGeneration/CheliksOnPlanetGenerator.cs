using System.Collections.Generic;
using Controllers;
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
        private readonly StateController _stateController;
        private readonly Material _material;
        
        public CheliksOnPlanetGenerator(StateController stateController, AllData data, float planetRadius, GameObject rootEnvironment, Material material)
        {
            _stateController = stateController;
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
            _material = material;
        }
        
        public void SetCheliksAndPosition(List<Transform> transforms)
        {
            for (int i = 0; i < transforms.Count; i++)
            {
                var randomChelikType = Random.Range(0, _cheliksPrefabs.Count);
                var chelik = Object.Instantiate(_cheliksPrefabs[randomChelikType], transforms[i].position, transforms[i].rotation);
                chelik.GetComponent<ChelikMove>().GetStateController(_stateController);
                var meshRenderers = chelik.GetComponentsInChildren<MeshRenderer>();
                foreach (var mesh in meshRenderers)
                {
                    mesh.material = _material;
                }
                chelik.transform.Translate(new Vector3(0, 0.2f, 0));
                _spawnedTopCheliks.Add(chelik.transform);
                chelik.transform.SetParent(_rootCheliksOnPlanet.transform);
            }
        }
        
        public List<Transform> CreateTopCheliksAndPosition(List<PlanetCell> planetCellsTop)
        {
            var createdCheliks = 0;
            var halfCheliksOnPlanet = Mathf.RoundToInt(_cheliksOnPlanet / 2);
            do
            {
                var randomCell = Random.Range(0, planetCellsTop.Count);
                if (planetCellsTop[randomCell].IsOccupied) continue;
                var tempCell = planetCellsTop[randomCell];
                tempCell.Occupied();
                planetCellsTop[randomCell] = tempCell;
                createdCheliks++;
                var randomChelikType = Random.Range(0, _cheliksPrefabs.Count);
                var positionAndRotation = GeneratePositionAndRotation(planetCellsTop[randomCell]);
                var chelik = Object.Instantiate(_cheliksPrefabs[randomChelikType], positionAndRotation.Item1, positionAndRotation.Item2);
                chelik.GetComponent<ChelikMove>().GetStateController(_stateController);
                var meshRenderers = chelik.GetComponentsInChildren<MeshRenderer>();
                foreach (var mesh in meshRenderers)
                {
                    mesh.material = _material;
                }
                chelik.transform.Translate(new Vector3(0, 0.2f, 0));
                _spawnedTopCheliks.Add(chelik.transform);
                chelik.transform.SetParent(_rootCheliksOnPlanet.transform);
            } while (halfCheliksOnPlanet > createdCheliks);

            return _spawnedTopCheliks;
        }
        
        public List<Transform> CreateDownCheliksAndPosition(List<PlanetCell> planetCellsDown)
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
                var randomChelikType = Random.Range(0, _cheliksPrefabs.Count);
                var positionAndRotation = GeneratePositionAndRotation(planetCellsDown[randomCell]);
                var chelik = Object.Instantiate(_cheliksPrefabs[randomChelikType], positionAndRotation.Item1, positionAndRotation.Item2);
                chelik.GetComponent<ChelikMove>().GetStateController(_stateController);
                _spawnedDownCheliks.Add(chelik.transform);
                chelik.transform.RotateAround(chelik.transform.position, chelik.transform.forward, 180);
                chelik.transform.SetParent(_rootCheliksOnPlanet.transform);
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