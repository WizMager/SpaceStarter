using System;
using Interface;
using UnityEngine;
using Utils;
using View;

public class UpAndDownAroundPlanet : IDisposable
{
    public event Action<bool> OnTakeDamage;
    
    private readonly float _startEngineForce;
    private readonly float _startGravityForce;
    private readonly Transform _playerTransform;
    private readonly GravityLittleView _gravityView;
    private readonly PlanetView _planetView;
    private readonly IUserInput<Vector3>[] _touch;
    private readonly float _maxGravityForce;
    private readonly float _maxEngineForce;
    private readonly float _gravityAcceleration;
    private readonly float _engineAcceleration;
    private readonly float _cooldownTakeDamage;
    private readonly float _thresholdPlanetGravity;
    
    private float _currentEngineForce;
    private float _currentGravityForce;
    private bool _engineOn;
    private UpAndDownState _state;
    private bool _isActive;
    private float _timeInPlanetOrGravity;
    private bool _timerStarted;
    private float _currentThresholdPlanetGravity;
    private float _playerTouchEdgeOrPlanetTime;

    public float GetPlayerTouchTime
    {
        get
        {
            var time = _playerTouchEdgeOrPlanetTime;
            _playerTouchEdgeOrPlanetTime = 0;
            return time;
        }
    }
    
    public UpAndDownAroundPlanet(float startEngineForce, float startGravityForce, Transform playerTransform, 
        PlanetView planetView, GravityLittleView gravityView, IUserInput<Vector3>[] touch, float maxGravityForce,
        float maxEngineForce, float gravityAcceleration, float engineAcceleration, float cooldownTakeDamage, float thresholdPlanetGravity)
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
        _cooldownTakeDamage = cooldownTakeDamage;
        _thresholdPlanetGravity = thresholdPlanetGravity;

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
        if (!value) return;
        _state = UpAndDownState.GravityAccelerate;
        _engineOn = false;
        _timerStarted = false;
    }
    
    public void Move(float deltaTime)
    {
        if (_timerStarted)
        {
            _playerTouchEdgeOrPlanetTime += deltaTime;
            if (_timeInPlanetOrGravity < _cooldownTakeDamage)
            {
                _timeInPlanetOrGravity += deltaTime;
            }
            else
            {
                OnTakeDamage?.Invoke(true);
            }
        }
        else
        {
            if (_currentThresholdPlanetGravity < _thresholdPlanetGravity)
            {
                _currentThresholdPlanetGravity += deltaTime;
                _timeInPlanetOrGravity += deltaTime;
            }
            else
            {
                OnTakeDamage?.Invoke(false);
                _currentThresholdPlanetGravity = 0;
                _timeInPlanetOrGravity = 0; 
            }
        }
        
        var shipPositionAxisY = Vector3.zero;
        switch (_state)
        {
            case UpAndDownState.Engine:
                shipPositionAxisY.y = _startEngineForce;
                _playerTransform.Translate(shipPositionAxisY * deltaTime);
                break;
            case UpAndDownState.EngineAccelerate:
                if (_currentEngineForce < _maxEngineForce)
                {
                    _currentEngineForce += _engineAcceleration * deltaTime;
                }
                shipPositionAxisY.y = _currentEngineForce;
                _playerTransform.Translate(shipPositionAxisY * deltaTime);
                break;
            case UpAndDownState.Gravity:
                shipPositionAxisY.y = -_startGravityForce;
                _playerTransform.Translate(shipPositionAxisY * deltaTime);
                break;
            case UpAndDownState.GravityAccelerate:
                if (_currentGravityForce < _maxGravityForce)
                {
                    _currentGravityForce += _gravityAcceleration * deltaTime;
                }
                shipPositionAxisY.y = -_currentGravityForce;
                _playerTransform.Translate(shipPositionAxisY * deltaTime);
                break;
            default:
                throw new ArgumentOutOfRangeException("This state not exist in UpAndDown");
        }
    }
        
    private void PlanetEntered()
    {
        if (!_isActive) return;
        _state = _engineOn ? UpAndDownState.EngineAccelerate : UpAndDownState.Engine;
        _timerStarted = true;
    }
        
    private void PlanetExited()
    {
        if (!_isActive) return;
        _state = _engineOn ? UpAndDownState.EngineAccelerate : UpAndDownState.GravityAccelerate;
        _timerStarted = false;
    }
        
    private void GravityEntered()
    {
        if (!_isActive) return;
        _state = _engineOn ? UpAndDownState.EngineAccelerate : UpAndDownState.GravityAccelerate;
        _timerStarted = false;
    }
        
    private void GravityExited()
    {
        if (!_isActive) return;
        _state = _engineOn ? UpAndDownState.Gravity : UpAndDownState.GravityAccelerate;
        _timerStarted = true;
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