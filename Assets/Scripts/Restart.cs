using System.Collections.Generic;
using Controllers;
using UnityEngine;
using Utils;

public class Restart : MonoBehaviour
{
    private readonly List<Transform> _allObjectTransforms = new List<Transform>();
    private readonly List<Vector3> _allObjectStartPositions = new List<Vector3>();
    private readonly List<Quaternion> _allObjectStartRotations = new List<Quaternion>();
    private readonly List<Vector3> _allObjectStartScale = new List<Vector3>();
    private StateController _stateController;

    public void SaveAllObjects(List<Transform> spawnedBuildingTransforms)
    {
        foreach (var spawnedBuilding in spawnedBuildingTransforms)
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

    public void TakeStateController(StateController stateController)
    {
        _stateController = stateController;
        _stateController.OnStateChange += ChangeState;
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
    
}