using System;
using Controllers;
using Interface;
using UnityEngine;
using Utils;

public class FlyToCenterGravity
{
    public event Action OnFinish;

    private readonly float _rotationSpeedGravity;
    private readonly float _moveSpeedGravity;
    private readonly Transform _playerTransform;
    private Transform _planet;
    private readonly StateController _stateController;
    
    private Vector3 _direction;
    private float _pathCenter;
    private bool _isMoved;
    private float _edgeRotationAngle;
    private SphereCollider _planetCollider;
    private bool _isActive;
    
        
    public FlyToCenterGravity(Transform playerTransform, float rotationSpeedGravity, float moveSpeedGravity, 
        Transform planet, StateController stateController)
    {
        _rotationSpeedGravity = rotationSpeedGravity;
        _moveSpeedGravity = moveSpeedGravity;
        _playerTransform = playerTransform;
        _planet = planet;
        _stateController = stateController;
        _planetCollider = _planet.GetComponent<SphereCollider>();

        _stateController.OnStateChange += StateChange;
    }

    private void StateChange(States state)
    {
        if (state == States.ToCenterGravity)
        {
            _isActive = true;
            _isMoved = false;
            var playerPosition = _playerTransform.position;
            var planetPosition = _planet.position;
            _direction = (planetPosition - playerPosition).normalized;
            _pathCenter = (Vector3.Distance(playerPosition, planetPosition) - _planetCollider.radius) / 2;
            _edgeRotationAngle = Vector3.Angle(_playerTransform.right, _direction);
        }
        else
        {
            _isActive = false;
        }
    }

    private void MoveAndRotate(float deltaTime)
    {
        if (_isMoved)
        {
            Rotate(deltaTime);
        }
        else
        {
            Move(deltaTime);
        }
    }
        
    private void Rotate(float deltaTime)
    {
        if (_edgeRotationAngle > 0)
        {
            var offsetAngle = deltaTime * _rotationSpeedGravity;
            _playerTransform.Rotate(_playerTransform.up, -offsetAngle);
            _edgeRotationAngle -= offsetAngle;
        }
        else
        {
            OnFinish?.Invoke();
        }
    }

    private void Move(float deltaTime)
    {
        if (_pathCenter >= 0)
        {
            var speed = deltaTime * _moveSpeedGravity;
            _playerTransform.Translate(_direction * speed, Space.World);
            _pathCenter -= speed;
        }
        else
        {
            _isMoved = true;
        }
    }

    public void FlyToCenter(float deltaTime)
    {
        if (!_isActive) return;
        MoveAndRotate(deltaTime);
    }
}