using UnityEngine;

public class CameraTopDownMoveController : IExecute, IClean
{
    private Transform _playerTransform;
    private IUserInput<Vector3> _inputTouch;
    private bool _isTouched;
    private float _gravityForce;
    private float _engineForce;
    private float _speedRotation;
    private Transform _currentPlanet;

    public CameraTopDownMoveController(IUserInput<Vector3> inputTouch, Transform playerTransform, float gravityForce, 
        float engineForce, float speedRotation, Transform planetTransform)
    {
        _playerTransform = playerTransform;
        _inputTouch = inputTouch;
        _inputTouch.OnChange += OnTouched;
        _gravityForce = gravityForce;
        _engineForce = engineForce;
        _speedRotation = speedRotation;
        _currentPlanet = planetTransform;
    }

    private void OnTouched(Vector3 touchPosition)
    {
        _isTouched = true;
    }
    
    private void CameraRotation(float deltaTime)
    {
        _playerTransform.RotateAround(_currentPlanet.position, Vector3.up, _speedRotation * deltaTime);
    }

    private void CameraMove(bool isTouched, float deltaTime)
    {
        var shipPositionAxisX = new Vector3(0, 0);
        if (isTouched)
        {
            shipPositionAxisX.x = -_engineForce;
            _playerTransform.Translate(shipPositionAxisX * deltaTime);
            _isTouched = false;
        }
        else
        {
            shipPositionAxisX.x = _gravityForce;
            _playerTransform.Translate(shipPositionAxisX * deltaTime);
        }
    }
    
    public void Execute(float deltaTime)
    {
        CameraMove(_isTouched, deltaTime);
        CameraRotation(deltaTime);
    }

    public void Clean()
    {
        _inputTouch.OnChange -= OnTouched;
    }
}