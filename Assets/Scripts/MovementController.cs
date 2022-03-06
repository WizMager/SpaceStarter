using Controller;
using DefaultNamespace;
using UnityEngine;
using View;

public class MovementController
{
    private readonly MoveAroundPlanet _moveAroundPlanet;
    private readonly MoveToDirection _moveToDirection;
    
    public MovementController(float engineForce, float gravityForce, float rotationAroundPlanet, 
        PlayerView playerView, float rotationSpeed, float moveSpeed)
    {
        _moveAroundPlanet = new MoveAroundPlanet(engineForce, gravityForce, rotationAroundPlanet, playerView.transform);
        _moveToDirection = new MoveToDirection(rotationSpeed, moveSpeed, playerView);
    }

    public void MoveAroundPlanet(float deltaTime, Transform currentPlanet)
    {
        _moveAroundPlanet.MovementAroundPlanet(deltaTime, currentPlanet);
    }

    public void MoveToPoint(float deltaTime)
    {
        _moveToDirection.MovingToPoint(deltaTime);
    }

    public void SetDirection(Vector3 lookDirection)
    {
        _moveToDirection.SetDirection(lookDirection);
    }
    
    public void InsidePlanet(bool isInside)
    {
        _moveAroundPlanet.InsidePlanet = isInside;
    }

    public void PlayerTouched(bool isTouched)
    {
        _moveAroundPlanet.IsTouched = isTouched;
    }
    
    public void EdgeGravityState(bool isOut)
    {
        _moveAroundPlanet.OutsideGravity = isOut;
    }

    public void GravityDirectionMove(bool isOut)
    {
        _moveToDirection.Activation(!isOut);
    }
}