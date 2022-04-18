using System.Collections;
using System.Collections.Generic;
using Controllers;
using UnityEngine;
using Utils;

public class Restart : MonoBehaviour
{
    private List<Transform> _allObjectTransforms = new List<Transform>();
    private List<Vector3> _allObjectStartPositions = new List<Vector3>();
    private List<Quaternion> _allObjectStartRotations = new List<Quaternion>();
    private StateController _stateController;

    private void Start()
    {
        SaveAllObjects();
    }

    private void SaveAllObjects()
    {
        var transforms = gameObject.GetComponentsInChildren<Transform>();
        foreach (var currentTransform in transforms)
        {
            _allObjectTransforms.Add(currentTransform);
            _allObjectStartPositions.Add(currentTransform.position);
            _allObjectStartRotations.Add(currentTransform.rotation);
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
            }

            _allObjectTransforms[j]
                .SetPositionAndRotation(_allObjectStartPositions[j], _allObjectStartRotations[j]);
        }
    }
    
}