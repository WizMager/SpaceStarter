using DefaultNamespace;
using UnityEngine;
using View;

public class FlyToNextPlanet
{
    private GravityView _gravityView;

    private readonly TrajectoryCalculate _trajectoryCalculate;
    
    private bool _isInGravity;
    private bool _isActive;

    public FlyToNextPlanet(GravityView gravityView, TrajectoryCalculate trajectoryCalculate)
    {
        _gravityView = gravityView;

        _trajectoryCalculate = trajectoryCalculate;
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

    public void ChangePlanet(GravityView currentGravityView)
    {
        OnDestroy();
        _gravityView = currentGravityView;
        _gravityView.OnPlayerGravityEnter += GravityEntered;
    }

    public void OnDestroy()
    {
        _gravityView.OnPlayerGravityEnter -= GravityEntered;
    }
}