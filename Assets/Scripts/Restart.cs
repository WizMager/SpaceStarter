using System.Collections;
using System.Collections.Generic;
using Controllers;
using UnityEngine;
using Utils;

public class Restart : MonoBehaviour
{
    [SerializeField] private float _timeForRestart;
    [SerializeField] private float _restartSpeed;
    private List<Transform> _allObjectTransforms = new List<Transform>();
    private List<Vector3> _allObjectStartPositions = new List<Vector3>();
    private List<Quaternion> _allObjectStartRotations = new List<Quaternion>();
    private StateController _stateController;
    private bool _firstIteration = true;

    private void Awake()
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
            StartCoroutine(RestartActivate());
        }
    }

    private IEnumerator RestartActivate()
    {
        for (float i = 0; i < _timeForRestart;)
        {
            var deltaTime = Time.deltaTime;
            i += deltaTime;

            for (int j = 0; j < _allObjectTransforms.Count; j++)
            {
                if (_firstIteration)
                {
                    var rigidBody = _allObjectTransforms[j].GetComponent<Rigidbody>();
                    if (rigidBody != null)
                    {
                        rigidBody.isKinematic = true; 
                    }
                }
                var stepTransform = Vector3.Lerp(_allObjectTransforms[j].position, _allObjectStartPositions[j],
                    deltaTime * _timeForRestart / _timeForRestart);
                var stepRotation = Quaternion.Lerp(_allObjectTransforms[j].rotation, _allObjectStartRotations[j],
                    deltaTime * _timeForRestart / _timeForRestart);
                _allObjectTransforms[j].SetPositionAndRotation(stepTransform, stepRotation);
            }

            _firstIteration = false;
            yield return null;
        }

        StopCoroutine(RestartActivate());
    }
    
}