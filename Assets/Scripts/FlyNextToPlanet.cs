using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using View;

public class FlyNextToPlanet
{
    private GravityView _gravityView;

    private readonly TrajectoryCalculate _trajectoryCalculate;
    
    private bool _isInGravity;
    private bool _isActive;

    public FlyNextToPlanet(float moveSpeed, GravityView gravityView, Transform playerTransform)
    {
        _gravityView = gravityView;

        _trajectoryCalculate = new TrajectoryCalculate(playerTransform, moveSpeed);
        _gravityView.OnPlayerGravityEnter += GravityEntered;
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

    public void SetActive(bool isActive)
    {
        _isActive = isActive;
    }

    public void ChangePlanet(GravityView currentGravityView, List<AsteroidView> asteroidViewsList)
    {
        OnDestroy();
        _gravityView = currentGravityView;
        _gravityView.OnPlayerGravityEnter += GravityEntered;
        _trajectoryCalculate.ChangePlanet(asteroidViewsList);
    }

    public void OnDestroy()
    {
        _gravityView.OnPlayerGravityEnter -= GravityEntered;
    }
}