using System;
using System.Collections;
using InputClasses;
using Interface;
using UnityEngine;
using View;
using Object = UnityEngine.Object;

public class CameraMove
{
    private readonly Transform _cameraTransform;
    private readonly float _cameraUpSpeed;
    private readonly float _cameraUpOffset;
    private readonly IUserInput<SwipeData> _swipeInput;
    private readonly Vector3 _lastPlanetCenter;
    private readonly float _firstPersonRotationSpeed;
    private readonly PlayerView _playerView;
    private readonly float _cameraDownPosition;
    private readonly float _cameraDownSpeed;
    private readonly CameraColliderView _cameraColliderView;
    private readonly float _cameraDownPositionLastPlanet;
    private readonly float _cameraDownSpeedLastPlanet;
    private readonly float _distanceLastPlanet;
    private readonly float _moveSpeedLastPlanet;
    private readonly Transform _lastPlanetTransform;
    private Transform _currentPlanet;
    private readonly FlyPlanetAngle _flyPlanetAngle;
    private  readonly float _speedToCenterBetween;
    private readonly float _angleLeadRotateAround;
    private readonly FlyToCenterGravity _flyToCenterGravity;
    private readonly int _minimalPercentMoveSpeed;

    private readonly Transform _playerTransform;
    private bool _cameraStopped;
    private bool _cameraColliderEntered;
    private float _distanceFlyFirstPerson;
    private float _pathToCenter;
    private int _planetIndex;

    private CameraRotateLastPlanet _cameraRotateLastPlanet;
    ScriptableData.ScriptableData data;

    public CameraMove(Camera camera, float cameraUpSpeed, float cameraUpOffset, 
        IUserInput<SwipeData> swipeInput, Vector3 lastPlanetCenter, float firstPersonRotationSpeed, PlayerView playerView,
        float cameraDownPosition, float cameraDownSpeed, CameraColliderView cameraCameraColliderView, 
        float cameraDownPositionLastPlanet, float cameraDownSpeedLastPlanet, float distanceLastPlanet, float moveSpeedLastPlanet,
        Transform lastPlanetTransform, Transform currentPlanet, FlyPlanetAngle flyPlanetAngle, float speedToCenterBetween, 
        float angleLeadRotateAround, FlyToCenterGravity flyToCenterGravity, int minimalPercentMoveSpeedFirstPerson, float speedDrift)
    {
        _cameraTransform = camera.transform;
        _cameraUpSpeed = cameraUpSpeed;
        _cameraUpOffset = cameraUpOffset;
        _swipeInput = swipeInput;
        _lastPlanetCenter = lastPlanetCenter;
        _firstPersonRotationSpeed = firstPersonRotationSpeed;
        _playerView = playerView;
        _cameraDownPosition = cameraDownPosition;
        _cameraDownSpeed = cameraDownSpeed;
        _cameraColliderView = cameraCameraColliderView;
        _cameraDownPositionLastPlanet = cameraDownPositionLastPlanet;
        _cameraDownSpeedLastPlanet = cameraDownSpeedLastPlanet;
        _distanceLastPlanet = distanceLastPlanet;
        _moveSpeedLastPlanet = moveSpeedLastPlanet;
        _lastPlanetTransform = lastPlanetTransform;
        _currentPlanet = currentPlanet;
        _flyPlanetAngle = flyPlanetAngle;
        _speedToCenterBetween = speedToCenterBetween;
        _angleLeadRotateAround = angleLeadRotateAround;
        _flyToCenterGravity = flyToCenterGravity;
        _minimalPercentMoveSpeed = minimalPercentMoveSpeedFirstPerson;

        _playerTransform = playerView.transform;
        _swipeInput.OnChange += CameraSwipeRotate;
        _cameraColliderView.OnPlayerEnter += PlayerCameraCameraColliderEntered;
        _flyPlanetAngle.OnRotateCalculated += RotateAroundPlanet;
        _flyPlanetAngle.OnPathBetweenPlanets += SetCenterBetweenPlanets;
        _flyToCenterGravity.OnDirectionCalculated += RotatedToPlanet;

        _cameraRotateLastPlanet = new CameraRotateLastPlanet(speedDrift, _cameraTransform, _lastPlanetCenter);
    }

    public bool CameraDownPlanet(float deltaTime)
    {
        return CameraDown(deltaTime, _cameraDownSpeed, _cameraDownPosition);
    }
    
    private bool CameraDown(float deltaTime, float downSpeed, float cameraDownPosition)
    {
        var cameraPositionY = _cameraTransform.position.y;
        var playerTransformPosition = _playerTransform.position;
        var playerPositionX = playerTransformPosition.x;
        var playerPositionZ = playerTransformPosition.z;

        if (cameraDownPosition <= cameraPositionY)
        {
            cameraPositionY -= deltaTime * downSpeed;
            var offset = new Vector3(playerPositionX, cameraPositionY, playerPositionZ);
            _cameraTransform.position = offset;
            return false;
        }
        else
        {
            var offset = new Vector3(playerPositionX, cameraPositionY, playerPositionZ);
            _cameraTransform.position = offset;
            
            return true;
        }
    }

    private void RotatedToPlanet()
    {
        var playerPositionUp = _playerTransform.position;
        playerPositionUp.y = _cameraTransform.position.y;
        _cameraTransform.position = playerPositionUp;
        var planetPositionUp = _currentPlanet.position;
        planetPositionUp.y = _cameraTransform.position.y;
        Quaternion lookRotationPlanet;
        Quaternion cameraRotation;
        var lookRotationPlayer = Quaternion.LookRotation(_cameraTransform.position - _playerTransform.position);
        
        if (_planetIndex % 2 == 0)
        {
            lookRotationPlanet = Quaternion.LookRotation(_cameraTransform.position - planetPositionUp, _cameraTransform.forward);
            cameraRotation = lookRotationPlanet * lookRotationPlayer;
            _cameraTransform.rotation = cameraRotation;
        }
        else
        {
            lookRotationPlanet = Quaternion.LookRotation(planetPositionUp - _cameraTransform.position, _cameraTransform.forward);
            cameraRotation = lookRotationPlanet * lookRotationPlayer;
            _cameraTransform.rotation = cameraRotation;
        }
    }
    
    public void RotateAroundPlanet(float angle)
    {
        _cameraTransform.RotateAround(_currentPlanet.position, _currentPlanet.forward, angle);
    }

    public void FollowPlayer()
    {
        var offsetPosition = _playerTransform.position;
        offsetPosition.y = _cameraTransform.position.y;
        _cameraTransform.position = offsetPosition;
    }

    public bool CameraUp(float deltaTime)
    {
        if (_cameraTransform.position.y >= _cameraUpOffset && _pathToCenter <= 0)
        {
            return true;
        }

        if (_cameraTransform.position.y < _cameraUpOffset)
        {
            var move = deltaTime * _cameraUpSpeed;
            _cameraTransform.position += new Vector3(0, move, 0);
        }

        if (_pathToCenter > 0)
        {
            var move = deltaTime * _speedToCenterBetween;
            if (_planetIndex % 2 == 0)
            {
                _cameraTransform.Translate(_cameraTransform.forward * move);
            }
            else
            {
                _cameraTransform.Translate(_cameraTransform.forward * -move);
            }
            
            _pathToCenter -= move;
        }
        
        return false;
    }

	internal bool CameraDrift(float deltaTime)
	{
        return _cameraRotateLastPlanet.CameraRotateTransform(deltaTime); //
	}

	private void SetCenterBetweenPlanets(float halfPath)
    {
        RotateAroundPlanet(-_angleLeadRotateAround);
        var planetPositionUp = _currentPlanet.position;
        planetPositionUp.y = _cameraTransform.position.y;
        var currentPathFromPlanet = Vector3.Distance(planetPositionUp, _cameraTransform.position);
        _pathToCenter = halfPath - currentPathFromPlanet;
    }

    public void FlyToLastPlanet(float deltaTime)
    {
        if (_cameraColliderEntered)
        {
            CameraDown(deltaTime, _cameraDownSpeedLastPlanet, _cameraDownPositionLastPlanet);
        }
        else
        {
            //FollowPlayer();
        }
    }
    
    private void PlayerCameraCameraColliderEntered()
    {
        _cameraColliderEntered = true;
    }
    
    public void FirstPersonActivation()
    {
        var currentDistance = Vector3.Distance(_cameraTransform.position, _lastPlanetTransform.position);
        _distanceFlyFirstPerson = currentDistance - _distanceLastPlanet;
        Object.Destroy(_playerView.gameObject);
        _cameraTransform.LookAt(_lastPlanetTransform.position);
        _cameraColliderView.StartCoroutine(StopFly());
    }

    private IEnumerator StopFly()
    {
        float moveSpeed;
        var moveSpeedMinimal = _moveSpeedLastPlanet / 100 * _minimalPercentMoveSpeed;
        for (float i = 0; i <= _distanceFlyFirstPerson; i += Time.deltaTime * moveSpeed)
        {
            moveSpeed = _moveSpeedLastPlanet - i / _distanceFlyFirstPerson * _moveSpeedLastPlanet;
            if (moveSpeed < moveSpeedMinimal)
            {
                moveSpeed = moveSpeedMinimal;
            }
            var moveDistance = Time.deltaTime * moveSpeed;
            _cameraTransform.transform.Translate(_cameraTransform.forward * moveDistance, Space.World);
            yield return null;
        }
        _cameraStopped = true;
        _cameraColliderView.StopCoroutine(StopFly());
    }

    public bool CameraFlyStopped()
    {
        return _cameraStopped;
    }

    private void CameraSwipeRotate(SwipeData swipeData)
    {
		if (!_cameraStopped) return;

		switch (swipeData.Direction)
		{
			case SwipeDirection.Left:
                _cameraTransform.RotateAround(_lastPlanetCenter, _cameraTransform.up, 
                    swipeData.Value * _firstPersonRotationSpeed); 
            break;

            case SwipeDirection.Right:
                _cameraTransform.RotateAround(_lastPlanetCenter, -_cameraTransform.up, 
                    swipeData.Value * _firstPersonRotationSpeed);
                break;

			case SwipeDirection.Up:
                _cameraTransform.RotateAround(_lastPlanetCenter, _cameraTransform.right, 
                    swipeData.Value * _firstPersonRotationSpeed);
                break;

			case SwipeDirection.Down:
                _cameraTransform.RotateAround(_lastPlanetCenter, -_cameraTransform.right, 
                    swipeData.Value * _firstPersonRotationSpeed);
                break;
			default:
				throw new ArgumentOutOfRangeException();
		}
	}

    public void ChangePlanet(Transform currentPlanet)
    {
        _currentPlanet = currentPlanet;
        _planetIndex++;
    }
    
    public void OnDestroy()
    {
        _swipeInput.OnChange -= CameraSwipeRotate;
        _cameraColliderView.OnPlayerEnter -= PlayerCameraCameraColliderEntered; 
        _flyPlanetAngle.OnRotateCalculated -= RotateAroundPlanet;
        _flyPlanetAngle.OnPathBetweenPlanets -= SetCenterBetweenPlanets;
        _flyToCenterGravity.OnDirectionCalculated -= RotatedToPlanet;
    }
}