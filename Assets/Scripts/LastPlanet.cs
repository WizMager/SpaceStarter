using System;
using DefaultNamespace;
using View;

public class LastPlanet : IDisposable
{
    private readonly GravityView _gravityView;

    private bool _inGravity;
    private readonly TrajectoryCalculate _trajectory;

    public LastPlanet(GravityView gravityView, TrajectoryCalculate trajectoryCalculate)
    {
        _gravityView = gravityView;

        _gravityView.OnPlayerGravityEnter += GravityEntered;
        _trajectory = trajectoryCalculate;
    }

    public bool FlyLastPlanet(float deltaTime)
    {
        if (_inGravity) return true;
            
        PlayerTranslate(deltaTime);
        return false;
    }

    private void PlayerTranslate(float deltaTime)
    {
        _trajectory.Move(deltaTime);
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