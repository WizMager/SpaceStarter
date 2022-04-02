using System.Collections;
using UnityEngine;
using View;

public class FlyToEdgeGravity
{
    private readonly float _rotationSpeed;
    private readonly float _moveSpeed;
    private GravityOutColliderView _gravityColliderView;
    private readonly Transform _playerTransform;
        
    private bool _isInGravity;
    private bool _isRotated;
    private Quaternion _lookRotation;

    public FlyToEdgeGravity(float rotationSpeed, float moveSpeed, GravityOutColliderView gravityColliderView, Transform playerTransform)
    {
        _rotationSpeed = rotationSpeed;
        _moveSpeed = moveSpeed;
        _gravityColliderView = gravityColliderView;
        _playerTransform = playerTransform;

        _gravityColliderView.OnPlayerGravityExit += GravityColliderExited;
    }

    public void SetDirection(Vector3 lookDirection)
    {
        _isInGravity = true;
        _isRotated = false;
        _lookRotation = Quaternion.LookRotation(new Vector3(lookDirection.x, 0, lookDirection.z));
        _gravityColliderView.StartCoroutine(Rotate());
    }

    private IEnumerator Rotate()
    {
        var startRotation = _playerTransform.rotation;
        for (float i = 0; i < _rotationSpeed; i += Time.deltaTime)
        {
            _playerTransform.rotation = Quaternion.Lerp(startRotation, _lookRotation, i / _rotationSpeed);
            yield return null;
        }

        _isRotated = true;
        _gravityColliderView.StopCoroutine(Rotate());
    }

    private void GravityColliderExited()
    {
        _isInGravity = false;
    }

    public bool IsFinished()
    {
        if (!_isInGravity)
        {
            return true;
        }
                
        if (_isRotated)
        {
            _playerTransform.Translate(_playerTransform.forward * Time.deltaTime * _moveSpeed, Space.World);
        }
                
        return false;
    }
    
            
    public void OnDestroy()
    {
        _gravityColliderView.OnPlayerGravityExit -= GravityColliderExited;
    }
}