using DefaultNamespace;
using UnityEngine;

public class MovementController
{
    private readonly MoveAroundPlanet _moveAroundPlanet;
    
    public MovementController(float engineForce, float gravityForce, float speedRotation, Transform playerTransform)
    {
        _moveAroundPlanet = new MoveAroundPlanet(engineForce, gravityForce, speedRotation, playerTransform);
    }

    public void MoveAroundPlanet(float deltaTime, Transform currentPlanet)
    {
        _moveAroundPlanet.MovementAroundPlanet(deltaTime, currentPlanet);
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