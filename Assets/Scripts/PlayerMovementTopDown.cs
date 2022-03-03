using UnityEngine;

public class PlayerMovementTopDown
{
    private readonly float _engineForce;
    private readonly float _gravityForce;
    private readonly float _speedRotation;
    private readonly Transform _playerTransform;

    private bool _isTouched;
    private bool _insidePlanet;
    private bool _outsideGravity;

    public PlayerMovementTopDown(float engineForce, float gravityForce, float speedRotation, Transform playerTransform)
    {
        _engineForce = engineForce;
        _gravityForce = gravityForce;
        _speedRotation = speedRotation;
        _playerTransform = playerTransform;
    }

    public void MoveAroundPlanet(float deltaTime, Transform currentPlanet)
    {
        Move(deltaTime);
        RotationAroundPlanet(deltaTime, currentPlanet);
    }
    
    private void Move(float deltaTime)
    {
        var shipPositionAxisX = new Vector3(0, 0);
        if (_isTouched && !_outsideGravity)
        {
            shipPositionAxisX.x = -_engineForce;
            _playerTransform.Translate(shipPositionAxisX * deltaTime);
        }
        else if (_insidePlanet && !_outsideGravity)
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
    
    private void RotationAroundPlanet(float deltaTime, Transform currentPlanet)
    {
        _playerTransform.RotateAround(currentPlanet.position, Vector3.up, _speedRotation * deltaTime);
    }

    public void InsidePlanet(bool isInside)
    {
        _insidePlanet = isInside;
    }

    public void RotationPlayer(Vector3 touchPoint)
    {
        if (!_isTouched) return;
        var rotationVector = (touchPoint - _playerTransform.position).normalized;
        rotationVector.y = 0;
        _playerTransform.up = rotationVector;
    }
    
    public void PlayerTouched(bool isTouched)
    {
        _isTouched = isTouched;
    }
    
    public void EdgeGravityState(bool isOut)
    {
        _outsideGravity = isOut;
    }
}