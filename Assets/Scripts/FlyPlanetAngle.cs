using System;
using UnityEngine;

public class FlyPlanetAngle
{
    public event Action<float> OnRotateCalculated; 

    private Transform _currentPlanet;
    private Transform _nextPlanet;
    private readonly Transform _player;

    private Vector3 _start;
    private Vector3 _end;
    private float _fullAngle;
    private float _currentAngle;
        
    public FlyPlanetAngle(Transform currentPlanet, Transform player, Transform nextPlanet)
    {
        _currentPlanet = currentPlanet;
        _nextPlanet = nextPlanet;
        _player = player;
        _start = _player.position - _currentPlanet.position;
        _end = _start;
        _fullAngle = Vector3.Angle(_start, nextPlanet.position - currentPlanet.position) + 360f;
    }
        
    public Vector3 FlewAngle()
    {
        _start = _player.position - _currentPlanet.position;
        if (_currentAngle >= _fullAngle)
        {
            _currentAngle = 0;
            var lookDirection = (_player.position - _currentPlanet.position).normalized;
            return lookDirection;
        }

        var angle = Vector3.Angle(_start, _end);
        OnRotateCalculated?.Invoke(angle);
        _currentAngle += angle;
        _end = _start;
        return Vector3.zero;
    }

    public void CalculateAngle()
    {
        _start = _player.position - _currentPlanet.position;
        _end = _start;
        _fullAngle = Vector3.Angle(_start, _nextPlanet.position - _currentPlanet.position) + 360f;
    }
        
    public void ChangePlanet(Transform currentPlanet, Transform nextPlanet)
    {
        _currentPlanet = currentPlanet;
        _nextPlanet = nextPlanet;
    }
}