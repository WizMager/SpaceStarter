using System.Collections.Generic;
using EnvironmentGeneration;
using Interface;
using UnityEngine;
using Utils;

namespace Controllers
{
    public class RestartController : IController, IClean
    {
        private readonly List<Transform> _allObjectTransforms = new List<Transform>();
        private readonly List<Vector3> _allObjectStartPositions = new List<Vector3>();
        private readonly List<Quaternion> _allObjectStartRotations = new List<Quaternion>();
        private readonly List<Vector3> _allObjectStartScale = new List<Vector3>();
        private readonly StateController _stateController;
        private readonly EnvironmentGenerator _environmentGenerator;

        public RestartController(StateController stateController, EnvironmentGenerator environmentGenerator)
        {
            _stateController = stateController;
            _environmentGenerator = environmentGenerator;
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

        private void ChangeState(GameState gameState)
        {
            if (gameState == GameState.Restart)
            {
                RestartActivate();
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

        public void Clean()
        {
            _stateController.OnStateChange -= ChangeState;
        }
    }
}