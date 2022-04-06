using System;
using Interface;
using UnityEngine;
using Utils;
using View;

public class UpAndDownAroundPlanet : IDisposable
{
    private readonly float _startEngineForce;
    private readonly float _startGravityForce;
    private readonly Transform _playerTransform;
    private GravityLittleView _gravityView;
    private PlanetView _planetView;
    private readonly IUserInput<Vector3>[] _touch;
    private readonly float _maxGravityForce;
    private readonly float _maxEngineForce;
    private readonly float _gravityAcceleration;
    private readonly float _engineAcceleration;
    
    private float _currentEngineForce;
    private float _currentGravityForce;
    private bool _engineOn;
    private UpAndDownState _state;
    private bool _isActive;

    public UpAndDownAroundPlanet(float startEngineForce, float startGravityForce, Transform playerTransform, 
        PlanetView planetView, GravityLittleView gravityView, IUserInput<Vector3>[] touch, float maxGravityForce,
        float maxEngineForce, float gravityAcceleration, float engineAcceleration)
    {
        _startEngineForce = startEngineForce;
        _startGravityForce = startGravityForce;
        _playerTransform = playerTransform;
        _planetView = planetView;
        _gravityView = gravityView;
        _touch = touch;
        _maxGravityForce = maxGravityForce;
        _maxEngineForce = maxEngineForce;
        _gravityAcceleration = gravityAcceleration;
        _engineAcceleration = engineAcceleration;

        _currentGravityForce = _startEngineForce;
        _currentEngineForce = _startEngineForce;
        _state = UpAndDownState.GravityAccelerate;

        _planetView.OnPlayerPlanetEnter += PlanetEntered;
        _planetView.OnPlayerPlanetExit += PlanetExited;
        _gravityView.OnPlayerGravityEnter += GravityEntered;
        _gravityView.OnPlayerGravityExit += GravityExited;
        _touch[(int) TouchInputState.InputTouchDown].OnChange += TouchedDown;
        _touch[(int) TouchInputState.InputTouchUp].OnChange += TouchedUp;
    }

    public void Active(bool value)
    {
        _isActive = value;
    }
    
    public void Move(float deltaTime)
    {
        var shipPositionAxisX = Vector3.zero;
        switch (_state)
        {
            case UpAndDownState.Engine:
                shipPositionAxisX.x = -_startEngineForce;
                _playerTransform.Translate(shipPositionAxisX * deltaTime);
                break;
            case UpAndDownState.EngineAccelerate:
                if (_currentEngineForce < _maxEngineForce)
                {
                    _currentEngineForce += _engineAcceleration * deltaTime;
                }
                shipPositionAxisX.x = -_currentEngineForce;
                _playerTransform.Translate(shipPositionAxisX * deltaTime);
                break;
            case UpAndDownState.Gravity:
                shipPositionAxisX.x = _startGravityForce;
                _playerTransform.Translate(shipPositionAxisX * deltaTime);
                break;
            case UpAndDownState.GravityAccelerate:
                if (_currentGravityForce < _maxGravityForce)
                {
                    _currentGravityForce += _gravityAcceleration * deltaTime;
                }
                shipPositionAxisX.x = _currentGravityForce;
                _playerTransform.Translate(shipPositionAxisX * deltaTime);
                break;
            default:
                throw new ArgumentOutOfRangeException("This state not exist in UpAndDown");
        }
    }
        
    private void PlanetEntered()
    {
        if (!_isActive) return;
        _state = _engineOn ? UpAndDownState.EngineAccelerate : UpAndDownState.Engine;
    }
        
    private void PlanetExited()
    {
        if (!_isActive) return;
        _state = _engineOn ? UpAndDownState.EngineAccelerate : UpAndDownState.GravityAccelerate;
    }
        
    private void GravityEntered()
    {
        if (!_isActive) return;
        _state = _engineOn ? UpAndDownState.EngineAccelerate : UpAndDownState.GravityAccelerate;
    }
        
    private void GravityExited()
    {
        if (!_isActive) return;
        _state = _engineOn ? UpAndDownState.Gravity : UpAndDownState.GravityAccelerate;
    }

    private void TouchedDown(Vector3 value)
    {
        if (!_isActive) return;
        _engineOn = true;
        _currentGravityForce = _startGravityForce;
        _state = UpAndDownState.EngineAccelerate;
    }

    private void TouchedUp(Vector3 value)
    {
        if (!_isActive) return;
        _engineOn = false;
        _currentEngineForce = _startEngineForce;
        _state = UpAndDownState.GravityAccelerate;
    }
    
    public void Dispose()
    {
        _planetView.OnPlayerPlanetEnter -= PlanetEntered;
        _planetView.OnPlayerPlanetExit -= PlanetExited;
        _gravityView.OnPlayerGravityEnter -= GravityEntered;
        _gravityView.OnPlayerGravityExit -= GravityExited;
        _touch[(int) TouchInputState.InputTouchDown].OnChange -= TouchedDown;
        _touch[(int) TouchInputState.InputTouchUp].OnChange -= TouchedUp;
    }
}