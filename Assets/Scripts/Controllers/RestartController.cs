using System;
using System.Collections.Generic;
using System.Linq;
using EnvironmentGeneration;
using Interface;
using ScriptableData;
using UnityEngine;
using Utils;
using View;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Controllers
{
    public class RestartController : IController, IClean
    {
        private readonly List<Transform> _allObjectTransforms = new List<Transform>();
        private readonly List<Vector3> _allObjectStartPositions = new List<Vector3>();
        private readonly List<Quaternion> _allObjectStartRotations = new List<Quaternion>();
        private readonly List<Vector3> _allObjectStartScale = new List<Vector3>();
        private readonly List<Transform> _planetPieces = new List<Transform>();
        private readonly List<Vector3> _planetPiecesPositions = new List<Vector3>();
        private readonly List<Quaternion> _planetPiecesRotations = new List<Quaternion>();
        private readonly List<Vector3> _planetPiecesScales = new List<Vector3>();
        private readonly StateController _stateController;
        private readonly EnvironmentGenerator _environmentGenerator;
        private readonly PaintPlanet _paintPlanet;

        public RestartController(StateController stateController, EnvironmentGenerator environmentGenerator, MaterialsData materialsData, 
            AfterRestart afterRestart)
        {
            _stateController = stateController;
            _environmentGenerator = environmentGenerator;
            var planetMaterials = afterRestart.PrepareMaterials;
            _paintPlanet = new PaintPlanet(planetMaterials[1][0]);
            _stateController.OnStateChange += ChangeState;
        }

        public void SaveObjects()
        {
            var objectsTransforms = _environmentGenerator.GenerateEnvironment();
            foreach (var spawnedBuilding in objectsTransforms)
            {
                var buildingTransforms = spawnedBuilding.GetComponent<Transform>();
                foreach (Transform currentTransform in buildingTransforms)
                {
                    _allObjectTransforms.Add(currentTransform);
                    _allObjectStartPositions.Add(currentTransform.position);
                    _allObjectStartRotations.Add(currentTransform.rotation);
                    _allObjectStartScale.Add(currentTransform.localScale);
                }
            }
        }

        public void SavePlanet()
        {
            var planetPieces = _environmentGenerator.TakePlanetPieces();
            foreach (var planetPiece in planetPieces)
            {
                _planetPieces.Add(planetPiece);
                _planetPiecesPositions.Add(planetPiece.position);
                _planetPiecesRotations.Add(planetPiece.rotation);
                _planetPiecesScales.Add(planetPiece.localScale);
            }
        }

        private void ChangeState(GameState gameState)
        {
            switch (gameState)
            {
                case GameState.Restart:
                    RestartActivate();
                    break;
                case GameState.NextLevel:
                    NextLevel();
                    break;
            }
        }

        private void RestartActivate()
        {
            for (int j = 0; j < _allObjectTransforms.Count; j++)
            {
                var rigidBody = _allObjectTransforms[j].GetComponent<Rigidbody>();
                if (rigidBody != null)
                {
                    rigidBody.isKinematic = true;
                    rigidBody.transform.localScale = Vector3.one;
                }

                _allObjectTransforms[j]
                    .SetPositionAndRotation(_allObjectStartPositions[j], _allObjectStartRotations[j]);
                _allObjectTransforms[j].localScale = _allObjectStartScale[j];
            }
        }

        private void NextLevel()
        {
            foreach (var objectTransform in _allObjectTransforms)
            {
                Object.Destroy(objectTransform.parent.gameObject);
            }
            _allObjectTransforms.Clear();
            _allObjectStartPositions.Clear();
            _allObjectStartRotations.Clear();
            _allObjectStartScale.Clear();
            for (int i = 0; i < _planetPieces.Count; i++)
            {
                _planetPieces[i].SetPositionAndRotation(_planetPiecesPositions[i], _planetPiecesRotations[i]);
                _planetPieces[i].localScale = _planetPiecesScales[i];
                var rigidBody = _planetPieces[i].GetComponent<Rigidbody>();
                if (rigidBody != null)
                {
                    rigidBody.isKinematic = true;
                    rigidBody.transform.localScale = Vector3.one;
                }
            }
            _paintPlanet.Paint(_planetPieces);
            SaveObjects();
        }

        public void Clean()
        {
            _stateController.OnStateChange -= ChangeState;
        }
    }
}