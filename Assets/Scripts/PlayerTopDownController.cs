using UnityEditor;
using UnityEngine;

public class PlayerTopDownController : IExecute, IClean
{
    private IUserInput<Vector3> _inputTouchDown;
    private IUserInput<Vector3> _inputTouchUp;
    private bool _isTouched;
    private float _gravityForce;
    private float _engineForce;
    private GameObject _currentPlanet;
    private Transform _currentPlanetTransform;
    private float _speedRotation;
    private Transform _playerTransform;
    private Rigidbody _playerRigidbody;
    private bool _insidePlanet;

    public PlayerTopDownController((IUserInput<Vector3> inputTouchDownDown, IUserInput<Vector3> inputTouchUp) touchInput, 
        GameObject player, float gravityForce, float engineForce, GameObject planet, float speedRotation)
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
        _currentPlanetTransform = _currentPlanet.transform;
        _currentPlanet.GetComponent<PlanetCollider>().OnPlayerEnter += PlayerInsidePlanet;
        _currentPlanet.GetComponent<PlanetCollider>().OnPlayerExit += PlayerOutsidePlanet;
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
        }
        else 
        if (_insidePlanet)
        {
            shipPositionAxisX.x = -_engineForce;
            _playerTransform.transform.Translate(shipPositionAxisX * deltaTime);
        }
        else
        {
            shipPositionAxisX.x = _gravityForce;
            _playerTransform.transform.Translate(shipPositionAxisX * deltaTime);
        }
    }
    
    private void PlayerRotation(float deltaTime)
    {
        _playerTransform.RotateAround(_currentPlanetTransform.position, Vector3.up, _speedRotation * deltaTime);
    }

    private void PlayerInsidePlanet()
    {
        _insidePlanet = true;
    }

    private void PlayerOutsidePlanet()
    {
        _insidePlanet = false;
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