using UnityEngine;

public class PlayerMovementTopDown
{
    private readonly float _engineForce;
    private readonly float _gravityForce;
    private readonly float _speedRotation;
    private readonly Transform _playerTransform;

    public PlayerMovementTopDown(float engineForce, float gravityForce, float speedRotation, Transform playerTransform)
    {
        _engineForce = engineForce;
        _gravityForce = gravityForce;
        _speedRotation = speedRotation;
        _playerTransform = playerTransform;
    }

    public void Move(bool isTouched, bool insidePlanet, float deltaTime)
    {
        var shipPositionAxisX = new Vector3(0, 0);
        if (isTouched)
        {
            shipPositionAxisX.x = -_engineForce;
            _playerTransform.Translate(shipPositionAxisX * deltaTime);
        }
        else if (insidePlanet)
        {
            shipPositionAxisX.x = -_engineForce;
            _playerTransform.Translate(shipPositionAxisX * deltaTime);
        }
        else
        {
            shipPositionAxisX.x = _gravityForce;
            _playerTransform.Translate(shipPositionAxisX * deltaTime);
        }
    }
    
    public void Rotation(float deltaTime, Transform currentPlanet)
    {
        _playerTransform.RotateAround(currentPlanet.position, Vector3.up, _speedRotation * deltaTime);
    }
}