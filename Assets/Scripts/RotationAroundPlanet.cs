using UnityEngine;

public class RotationAroundPlanet
{
    private readonly float _speedRotation;
    private readonly Transform _playerTransform;
    private readonly Transform _currentPlanet;

    public RotationAroundPlanet(float speedRotation, Transform playerTransform, Transform currentPlanet)
    {
        _speedRotation = speedRotation;
        _playerTransform = playerTransform;
        _currentPlanet = currentPlanet;
    }
        
    public void Move(float deltaTime)
    {
        _playerTransform.RotateAround(_currentPlanet.position, _currentPlanet.up, _speedRotation * deltaTime);
    }
}