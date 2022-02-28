using UnityEditor;
using UnityEngine;

public class PlayerTopDownController : IExecute, IClean
{
    //private GameObject _player;
    private IUserInput<Vector3> _inputTouch;
    private bool _isTouched;
    private float _gravityForce;
    private float _engineForce;
    private Transform _currentPlanet;
    private float _speedRotation;
    private Rigidbody _playerRigidbody;
    private Transform _playerTransform;

    public PlayerTopDownController(IUserInput<Vector3> inputTouch, GameObject player, float gravityForce, 
        float engineForce,Transform planet, float speedRotation)
    {
        //_player = player;
        _inputTouch = inputTouch;
        _inputTouch.OnChange += OnTouched;
        _playerRigidbody = player.GetComponent<Rigidbody>();
        _playerTransform = player.GetComponent<Transform>();
        _gravityForce = gravityForce;
        _engineForce = engineForce;
        _currentPlanet = planet;
        _speedRotation = speedRotation;
    }

    private void OnTouched(Vector3 touchPosition)
    {
        _isTouched = true;
    }

    private void CameraMove(bool isTouched, float deltaTime)
    {
        var shipPositionAxisX = new Vector3(0, 0);
        if (isTouched)
        {
            shipPositionAxisX.x = -_engineForce;
            _playerTransform.transform.Translate(shipPositionAxisX * deltaTime);
            //_playerRigidbody.AddForce(_playerTransform.up * _engineForce, ForceMode.Force);
            _isTouched = false;
        }
        else
        {
            shipPositionAxisX.x = _gravityForce;
            _playerTransform.transform.Translate(shipPositionAxisX * deltaTime);
            //_playerRigidbody.AddForce(_playerTransform.right * _gravityForce * deltaTime, ForceMode.Impulse);
            var directionToPlanet = (_currentPlanet.position - _playerTransform.position).normalized;
            var distanceToPlanet = (_currentPlanet.position - _playerTransform.position).magnitude;
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
        CameraMove(_isTouched, deltaTime);
        PlayerRotation(deltaTime);
    }

    public void Clean()
    {
        _inputTouch.OnChange -= OnTouched;
    }
}