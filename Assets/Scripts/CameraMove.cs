using System;
using System.Collections;
using InputClasses;
using Interface;
using UnityEngine;
using View;
using Object = UnityEngine.Object;

public class CameraMove
{
    private readonly Camera _camera;
    private readonly float _cameraUpSpeed;
    private readonly float _cameraUpOffset;
    private readonly IUserInput<SwipeData> _swipeInput;
    private readonly Vector3 _lastPlanetCenter;
    private readonly float _firstPersonRotationSpeed;
    private readonly PlayerView _playerView;
    private readonly float _cameraDownPosition;
    private readonly float _cameraDownSpeed;
    private readonly CameraColliderView _colliderView;
    private readonly float _cameraDownPositionLastPlanet;
    private readonly float _cameraDownSpeedLastPlanet;
    private readonly float _distanceLastPlanet;
    private readonly float _moveSpeedLastPlanet;
    private readonly Transform _lastPlanetTransform;
    private Transform _currentPlanet;
    private readonly FlyPlanetAngle _flyPlanetAngle;
    private readonly FlyToCenterGravity _flyToCenterGravity;

    private readonly Transform _playerTransform;
    private bool _cameraStopped;
    private bool _cameraColliderEntered;
    private float _distanceFlyFirstPerson;
    private float _pathToCenter;

    public CameraMove(Camera camera, float cameraUpSpeed, float cameraUpOffset, 
        IUserInput<SwipeData> swipeInput, Vector3 lastPlanetCenter, float firstPersonRotationSpeed, PlayerView playerView,
        float cameraDownPosition, float cameraDownSpeed, CameraColliderView cameraColliderView, 
        float cameraDownPositionLastPlanet, float cameraDownSpeedLastPlanet, float distanceLastPlanet, float moveSpeedLastPlanet,
        Transform lastPlanetTransform, Transform currentPlanet, FlyPlanetAngle flyPlanetAngle, FlyToCenterGravity flyToCenterGravity)
    {
        _camera = camera;
        _cameraUpSpeed = cameraUpSpeed;
        _cameraUpOffset = cameraUpOffset;
        _swipeInput = swipeInput;
        _lastPlanetCenter = lastPlanetCenter;
        _firstPersonRotationSpeed = firstPersonRotationSpeed;
        _playerView = playerView;
        _cameraDownPosition = cameraDownPosition;
        _cameraDownSpeed = cameraDownSpeed;
        _colliderView = cameraColliderView;
        _cameraDownPositionLastPlanet = cameraDownPositionLastPlanet;
        _cameraDownSpeedLastPlanet = cameraDownSpeedLastPlanet;
        _distanceLastPlanet = distanceLastPlanet;
        _moveSpeedLastPlanet = moveSpeedLastPlanet;
        _lastPlanetTransform = lastPlanetTransform;
        _currentPlanet = currentPlanet;
        _flyPlanetAngle = flyPlanetAngle;
        _flyToCenterGravity = flyToCenterGravity;

        _playerTransform = playerView.transform;
        _swipeInput.OnChange += CameraSwipeRotate;
        _colliderView.OnPlayerEnter += PlayerCameraColliderEntered;
        _flyPlanetAngle.OnRotateCalculated += RotateAroundPlanet;
        _flyToCenterGravity.OnPathToCenterCalculated += SetPathToCenter;
    }
    
    public void RotateAroundPlanet(float angle)
    {
        _camera.transform.RotateAround(_currentPlanet.position, _currentPlanet.forward, angle);
    }

    private void SetPathToCenter(float value)
    {
        _pathToCenter = value;
    }
    
    public void FollowPlayer()
    {
        var offsetPosition = _playerTransform.position;
        var cameraTransform = _camera.transform;
        offsetPosition.y = cameraTransform.position.y;
        cameraTransform.position = offsetPosition;
    }

    public bool CameraUp(float deltaTime)
    {
        var cameraTransform = _camera.transform;
        var playerPosition = _playerTransform.position;
        if (cameraTransform.position.y >= _cameraUpOffset)
        {
            playerPosition.y = cameraTransform.position.y;
            cameraTransform.position = playerPosition;
            return true;
        }
        playerPosition.y = cameraTransform.position.y + _cameraUpSpeed * deltaTime;
        cameraTransform.position = playerPosition;
        return false;
    }

    public bool CameraDownPlanet(float deltaTime)
    {
        return CameraDown(deltaTime, _cameraDownSpeed, _cameraDownPosition);
    }

    private bool CameraDown(float deltaTime, float downSpeed, float cameraDownPosition)
    {
        var cameraPositionY = _camera.transform.position.y;
        var playerTransformPosition = _playerTransform.position;
        var playerPositionX = playerTransformPosition.x;
        var playerPositionZ = playerTransformPosition.z;
        if (cameraDownPosition <= cameraPositionY)
        {
            cameraPositionY -= deltaTime * downSpeed;
            var offset = new Vector3(playerPositionX, cameraPositionY, playerPositionZ);
            _camera.transform.position = offset;
            return false;
        }
        else
        {
            var offset = new Vector3(playerPositionX, cameraPositionY, playerPositionZ);
            _camera.transform.position = offset;
            return true;
        }
    }

    public void FlyToLastPlanet(float deltaTime)
    {
        if (_cameraColliderEntered)
        {
            CameraDown(deltaTime, _cameraDownSpeedLastPlanet, _cameraDownPositionLastPlanet);
        }
        else
        {
            FollowPlayer();
        }
    }
    
    private void PlayerCameraColliderEntered()
    {
        _cameraColliderEntered = true;
    }
    
    public void FirstPersonActivation()
    {
        var currentDistance = Vector3.Distance(_camera.transform.position, _lastPlanetTransform.position);
        _distanceFlyFirstPerson = currentDistance - _distanceLastPlanet;
        _distanceFlyFirstPerson -= 0.1f;
        Object.Destroy(_playerView.gameObject);
        _camera.transform.LookAt(_lastPlanetTransform.position);
        _colliderView.StartCoroutine(StopFly());
    }

    private IEnumerator StopFly()
    {
        for (float i = 0; i < _distanceFlyFirstPerson; i += Time.deltaTime)
        {
            var moveSpeed = _moveSpeedLastPlanet - i / _distanceFlyFirstPerson * _moveSpeedLastPlanet;
            _camera.transform.Translate(_camera.transform.forward * moveSpeed * Time.deltaTime, Space.World);
            yield return null;
        }
        _cameraStopped = true;
        _colliderView.StopCoroutine(StopFly());
    }

    public bool CameraFlyStopped()
    {
        return _cameraStopped;
    }

    private void CameraSwipeRotate(SwipeData swipeData)
    {
        if (!_cameraStopped) return;
        
        switch(swipeData.Direction)
        {
            case SwipeDirection.Left:
                _camera.transform.RotateAround(_lastPlanetCenter, Vector3.up, swipeData.Value * _firstPersonRotationSpeed);
                break;
            case SwipeDirection.Right:
                _camera.transform.RotateAround(_lastPlanetCenter, Vector3.up, -swipeData.Value * _firstPersonRotationSpeed);
                break;
            case SwipeDirection.Up:
                _camera.transform.RotateAround(_lastPlanetCenter, Vector3.forward, swipeData.Value * _firstPersonRotationSpeed);
                break;
            case SwipeDirection.Down:
                _camera.transform.RotateAround(_lastPlanetCenter, Vector3.forward, -swipeData.Value * _firstPersonRotationSpeed);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void ChangePlanet(Transform currentPlanet)
    {
        _currentPlanet = currentPlanet;
    }
    
    public void OnDestroy()
    {
        _swipeInput.OnChange += CameraSwipeRotate;
        _colliderView.OnPlayerEnter += PlayerCameraColliderEntered; 
    }
}