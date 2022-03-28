using System;
using DefaultNamespace;
using View;

public class LastPlanet : IDisposable
{
    private readonly GravityView _gravityView;
    private readonly float _moveSpeed;

    private bool _inGravity;
    private readonly TrajectoryCalculate _trajectory;

    public LastPlanet(GravityView gravityView, TrajectoryCalculate trajectoryCalculate, float moveSpeed)
    {
        _gravityView = gravityView;
        _moveSpeed = moveSpeed;

        _gravityView.OnPlayerGravityEnter += GravityEntered;
        _trajectory = trajectoryCalculate;
    }

    public bool FlyToLastPlanet(float deltaTime)
    {
        if (_inGravity) return true;
            
        PlayerTranslate(deltaTime);
        return false;
    }

    private void PlayerTranslate(float deltaTime)
    {
        _trajectory.Move(deltaTime, _moveSpeed);
    }
        
    private void GravityEntered()
    {
        _inGravity = true;
    }
    
    public void Dispose()
    {
        _gravityView.OnPlayerGravityEnter -= GravityEntered;
    }
}