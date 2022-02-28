using UnityEditor;
using UnityEngine;

public class PlayerTopDownController : IExecute, IClean
{
    private IUserInput<Vector3> _inputTouchDown;
    private IUserInput<Vector3> _inputTouchUp;
    private bool _isTouched;
    private float _gravityForce;
    private float _engineForce;
    private Transform _currentPlanet;
    private float _speedRotation;
    private Transform _playerTransform;
    private Rigidbody _playerRigidbody;

    public PlayerTopDownController((IUserInput<Vector3> inputTouchDownDown, IUserInput<Vector3> inputTouchUp) touchInput, 
        GameObject player, float gravityForce, float engineForce, Transform planet, float speedRotation)
    {
        _inputTouchDown = touchInput.inputTouchDownDown;
        _inputTouchUp = touchInput.inputTouchUp;
        _inputTouchDown.OnChange += OnTouchedDown;
        _inputTouchUp.OnChange += OnTouchedUp;
        _playerRigidbody = player.GetComponent<Rigidbody>();
        _playerTransform = player.GetComponent<Transform>();
        _gravityForce = gravityForce;
        _engineForce = engineForce;
        _currentPlanet = planet;
        _speedRotation = speedRotation;
    }

    private void OnTouchedDown(Vector3 touchPosition)
    {
        _isTouched = true;
    }
    
    private void OnTouchedUp(Vector3 touchPosition)
    {
        _isTouched = false;
    }

    private void PlayerMove(bool isTouched, float deltaTime)
    {
        var shipPositionAxisX = new Vector3(0, 0);
        if (isTouched)
        {
            shipPositionAxisX.x = -_engineForce;
            _playerTransform.transform.Translate(shipPositionAxisX * deltaTime);
            //_playerRigidbody.AddForce(_playerTransform.up * _engineForce, ForceMode.Force);
            //_isTouched = false;
        }
        else
        {
            shipPositionAxisX.x = _gravityForce;
            _playerTransform.transform.Translate(shipPositionAxisX * deltaTime);
            //_playerRigidbody.AddForce(_playerTransform.right * _gravityForce * deltaTime, ForceMode.Impulse);
            //var directionToPlanet = (_currentPlanet.position - _playerTransform.position).normalized;
            //var distanceToPlanet = (_currentPlanet.position - _playerTransform.position).magnitude;
            //_playerRigidbody.AddForce(directionToPlanet * _gravityForce / distanceToPlanet);
        }
    }
    
    private void PlayerRotation(float deltaTime)
    {
        //_playerRigidbody.AddForce(_playerTransform.up * _speedRotation * deltaTime / 5, ForceMode.Impulse);
        //_playerTransform.LookAt(_playerTransform, Vector3.left);
        _playerTransform.RotateAround(_currentPlanet.position, Vector3.up, _speedRotation * deltaTime);
    }
    
    public void Execute(float deltaTime)
    {
        PlayerMove(_isTouched, deltaTime);
        PlayerRotation(deltaTime);
    }

    public void Clean()
    {
        _inputTouchDown.OnChange -= OnTouchedDown;
    }
}