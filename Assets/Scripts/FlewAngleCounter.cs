using System;
using Controllers;
using UnityEngine;
using Utils;

public class FlewAngleCounter
{
    public event Action OnFinish;
    
    private readonly Transform _planet;
    private readonly Transform _player;
    private float _flyAngle;
    private readonly StateController _stateController;

    private Vector3 _start;
    private Vector3 _end;
    private float _currentAngle;
    private bool _isActive;
        
    public FlewAngleCounter(Transform planet, Transform player, float flyAngle, StateController stateController)
    {
        _planet = planet;
        _flyAngle = flyAngle;
        _player = player;
        _stateController = stateController;
        _start = _player.position - _planet.position;
        _end = _start;

        _stateController.OnStateChange += StateChange;
    }

    private void StateChange(GameState gameState)
    {
        _isActive = gameState == GameState.FlyAroundPlanet;
    }

    public void FlewAngle()
    {
        if (!_isActive) return;
        _start = _player.position - _planet.position;
        if (_currentAngle >= _flyAngle)
        {
            _currentAngle = 0;
            OnFinish?.Invoke();
        }
        var angle = Vector3.Angle(_start, _end);
        _currentAngle += angle;
        _end = _start;
    }
}