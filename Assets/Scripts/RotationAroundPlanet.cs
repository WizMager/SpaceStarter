﻿using UnityEngine;

public class RotationAroundPlanet
{
    private readonly float _speedRotation;
    private readonly Transform _playerTransform;
    private Transform _currentPlanet;

    public RotationAroundPlanet(float speedRotation, Transform playerTransform, Transform currentPlanet)
    {
        _speedRotation = speedRotation;
        _playerTransform = playerTransform;
        _currentPlanet = currentPlanet;
    }
        
    public void Move(float deltaTime)
    {
        _playerTransform.RotateAround(_currentPlanet.position, _currentPlanet.forward, _speedRotation * deltaTime);
    }

    public void ChangePlanet(Transform currentPlanet)
    {
        _currentPlanet = currentPlanet;
    }
}