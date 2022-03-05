using Controller;
using DefaultNamespace;
using UnityEngine;

public class MovementController
{
    private readonly MoveAroundPlanet _moveAroundPlanet;
    private readonly MoveToPoint _moveToPoint;
    
    public MovementController(float engineForce, float gravityForce, float rotationAroundPlanet, 
        Transform playerTransform, float rotationSpeed, float moveSpeed)
    {
        _moveAroundPlanet = new MoveAroundPlanet(engineForce, gravityForce, rotationAroundPlanet, playerTransform);
        _moveToPoint = new MoveToPoint(rotationSpeed, moveSpeed, playerTransform);
    }

    public void MoveAroundPlanet(float deltaTime, Transform currentPlanet)
    {
        _moveAroundPlanet.MovementAroundPlanet(deltaTime, currentPlanet);
    }

    public void MoveToPoint(float deltaTime)
    {
        _moveToPoint.MovingToPoint(deltaTime);
    }

    public void SetDirection(Vector3 lookDirection)
    {
        _moveToPoint.SetDirection(lookDirection);
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
}