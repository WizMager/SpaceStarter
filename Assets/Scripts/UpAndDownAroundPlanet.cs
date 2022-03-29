using System;
using Interface;
using UnityEngine;
using Utils;
using View;

public class UpAndDownAroundPlanet
{
    private readonly float _startEngineForce;
    private readonly float _startGravityForce;
    private readonly Transform _playerTransform;
    private GravityView _gravityView;
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

    public UpAndDownAroundPlanet(float startEngineForce, float startGravityForce, Transform playerTransform, 
        PlanetView currentPlanet, GravityView currentGravity, IUserInput<Vector3>[] touch, float maxGravityForce,
        float maxEngineForce, float gravityAcceleration, float engineAcceleration)
    {
        _startEngineForce = startEngineForce;
        _startGravityForce = startGravityForce;
        _playerTransform = playerTransform;
        _planetView = currentPlanet;
        _gravityView = currentGravity;
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
        _touch[(int) TouchInput.InputTouchDown].OnChange += TouchedDown;
        _touch[(int) TouchInput.InputTouchUp].OnChange += TouchedUp;
    }
        
    public void Move(float deltaTime)
    {
        var shipPositionAxisX = Vector3.zero;
        switch (_state)
        {
            case UpAndDownState.Engine:
                shipPositionAxisX.x = -_currentEngineForce;
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
                shipPositionAxisX.x = _currentGravityForce;
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
        _state = _engineOn ? UpAndDownState.EngineAccelerate : UpAndDownState.Engine;
    }
        
    private void PlanetExited()
    {
        _state = _engineOn ? UpAndDownState.EngineAccelerate : UpAndDownState.GravityAccelerate;
    }
        
    private void GravityEntered()
    {
        _state = _engineOn ? UpAndDownState.EngineAccelerate : UpAndDownState.GravityAccelerate;
    }
        
    private void GravityExited()
    {
        _state = _engineOn ? UpAndDownState.Gravity : UpAndDownState.GravityAccelerate;
    }

    private void TouchedDown(Vector3 value)
    {
        _engineOn = true;
        _currentGravityForce = _startGravityForce;
        _state = UpAndDownState.EngineAccelerate;
    }

    private void TouchedUp(Vector3 value)
    {
        _engineOn = false;
        _currentEngineForce = _startEngineForce;
        _state = UpAndDownState.GravityAccelerate;
    }

    public void ChangePlanet(PlanetView currentPlanet, GravityView currentGravity)
    {
        _state = UpAndDownState.GravityAccelerate;
        
        _planetView.OnPlayerPlanetEnter -= PlanetEntered;
        _planetView.OnPlayerPlanetExit -= PlanetExited;
        _gravityView.OnPlayerGravityEnter -= GravityEntered;
        _gravityView.OnPlayerGravityExit -= GravityExited;

        _planetView = currentPlanet;
        _gravityView = currentGravity;
            
        _planetView.OnPlayerPlanetEnter += PlanetEntered;
        _planetView.OnPlayerPlanetExit += PlanetExited;
        _gravityView.OnPlayerGravityEnter += GravityEntered;
        _gravityView.OnPlayerGravityExit += GravityExited;
    }
        
    public void OnDestroy()
    {
        _planetView.OnPlayerPlanetEnter -= PlanetEntered;
        _planetView.OnPlayerPlanetExit -= PlanetExited;
        _gravityView.OnPlayerGravityEnter -= GravityEntered;
        _gravityView.OnPlayerGravityExit -= GravityExited;
        _touch[(int) TouchInput.InputTouchDown].OnChange -= TouchedDown;
        _touch[(int) TouchInput.InputTouchUp].OnChange -= TouchedUp;
    }
}