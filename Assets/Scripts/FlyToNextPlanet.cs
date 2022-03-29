using DefaultNamespace;
using UnityEngine;
using View;

public class FlyToNextPlanet
{
    private GravityEnterView _gravityView;
    private readonly TrajectoryCalculate _trajectoryCalculate;
    private DeadZoneView[] _deadZoneViews;
    private readonly Transform _playerTransform;
    
    private bool _isInGravity;
    private bool _isActive;
    private bool _isInDeadZone;
    private Vector3 _startPosition;
    private Quaternion _startRotation;

    public FlyToNextPlanet(GravityEnterView gravityView, TrajectoryCalculate trajectoryCalculate, Transform playerTransform,
        DeadZoneView[] deadZoneViews)
    {
        _gravityView = gravityView;
        _trajectoryCalculate = trajectoryCalculate;
        _playerTransform = playerTransform;
        _deadZoneViews = deadZoneViews;
        
        _gravityView.OnPlayerGravityEnter += GravityEntered;
        SubscribeDeadZone();
    }

    private void GravityEntered()
    {
        if (!_isActive) return;
        
        _isInGravity = true;
    }

    public bool IsFinished(float deltaTime)
    {
        if (_isInGravity)
        {
            _isInGravity = false;
            return true;
        }
        _trajectoryCalculate.Move(deltaTime);

        return false;
    }

    public bool IsInDeadZone()
    {
        if (!_isInDeadZone) return false;
        
        _playerTransform.position = _startPosition;
        _playerTransform.rotation = _startRotation;
        _isInDeadZone = false;
        return true;

    }
    
    public void SetActive(bool isActive)
    {
        _isActive = isActive;
        _startPosition = _playerTransform.position;
        _startRotation = _playerTransform.rotation;
    }

    private void DeadZoneEntered()
    {
        if (!_isActive) return;
        
        _isInDeadZone = true;
    }

    private void SubscribeDeadZone()
    {
        foreach (var deadZoneView in _deadZoneViews)
        {
            deadZoneView.OnDeadZoneEnter += DeadZoneEntered;
        }
    }

    private void UnSubscribeDeadZone()
    {
        foreach (var deadZoneView in _deadZoneViews)
        {
            deadZoneView.OnDeadZoneEnter -= DeadZoneEntered;
        }
    }
    
    public void ChangePlanet(GravityEnterView currentGravityView)
    {
        _gravityView.OnPlayerGravityEnter -= GravityEntered;
        _gravityView = currentGravityView;
        _gravityView.OnPlayerGravityEnter += GravityEntered;
    }

    public void OnDestroy()
    {
        _gravityView.OnPlayerGravityEnter -= GravityEntered;
        UnSubscribeDeadZone();
    }
}