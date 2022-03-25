using System;
using UnityEngine;
using View;

public class FlyToCenterGravity
{
    public event Action<float> OnPathToCenterCalculated; 

    private readonly float _rotationSpeedGravity;
    private readonly float _moveSpeedGravity;

    private readonly Transform _playerTransform;
    private Transform _currentPlanet;
    private Vector3 _direction;
    private float _pathCenter;
    private bool _isFinish;
    private bool _isMoved;
    private float _edgeRotationAngle;
    private SphereCollider _planetCollider;
        
    public FlyToCenterGravity(PlayerView playerView, float rotationSpeedGravity, float moveSpeedGravity, Transform currentPlanet)
    {
        _rotationSpeedGravity = rotationSpeedGravity;
        _moveSpeedGravity = moveSpeedGravity;

        _playerTransform = playerView.transform;
        _currentPlanet = currentPlanet;
        _planetCollider = _currentPlanet.GetComponent<SphereCollider>();
    }

    public void Active()
    {
        _isFinish = false;
        _isMoved = false;
        var playerPosition = _playerTransform.position;
        var planetPosition = _currentPlanet.position;
        _direction = (planetPosition - playerPosition).normalized;
        //TODO : need to add plus offset planet collider 
        _pathCenter = (Vector3.Distance(playerPosition, planetPosition) - _planetCollider.radius) / 2;
        OnPathToCenterCalculated?.Invoke(_pathCenter);
        _edgeRotationAngle = Vector3.Angle(_playerTransform.right, _direction);
    }

    public bool IsFinished(float deltaTime)
    {
        if (_isMoved)
        {
            Rotate(deltaTime);
        }
        else
        {
            Move(deltaTime);
        }
            
        return _isFinish;
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
            _isFinish = true;  
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

    public void ChangePlanet(Transform currentPlanet)
    {
        _currentPlanet = currentPlanet;
        _planetCollider = _currentPlanet.GetComponent<SphereCollider>();
    }
}