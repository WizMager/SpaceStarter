using UnityEngine;

public class PlayerTopDownController : IExecute, IClean
{
    private IUserInput<Vector3> _inputTouchDown;
    private IUserInput<Vector3> _inputTouchUp;
    private bool _isTouched;
    private float _gravityForce;
    private float _engineForce;
    private GameObject[] _planets;
    private float _speedRotation;
    private Transform _playerTransform;
    private bool _insidePlanet;
    private GameObject[] _gravityFields;
    private float _playerEndFlyingAngle;
    private float _playerCurrentFlyingAngle;
    private Vector3 _playerStartFlying;
    private Vector3 _playerEndFlying;
    

    public PlayerTopDownController((IUserInput<Vector3> inputTouchDownDown, IUserInput<Vector3> inputTouchUp) touchInput, 
        GameObject player, float gravityForce, float engineForce, GameObject[] planets, float speedRotation, 
        GameObject[] gravityFields, float playerFlyingAngle)
    {
        _inputTouchDown = touchInput.inputTouchDownDown;
        _inputTouchUp = touchInput.inputTouchUp;
        _inputTouchDown.OnChange += OnTouchedDown;
        _inputTouchUp.OnChange += OnTouchedUp;
        _playerTransform = player.GetComponent<Transform>();
        _gravityForce = gravityForce;
        _engineForce = engineForce;
        _planets = planets;
        _planets[0].GetComponent<PlanetCollider>().OnPlayerPlanetEnter += PlayerEnteredPlanet;
        _planets[0].GetComponent<PlanetCollider>().OnPlayerPlanetExit += PlayerExitedPlanet;
        _speedRotation = speedRotation;
        _gravityFields = gravityFields;
        _gravityFields[0].GetComponent<GravityCollider>().OnPlayerGravityEnter += PlayerEnteredGravity;
        _gravityFields[0].GetComponent<GravityCollider>().OnPlayerGravityExit += PlayerExitedGravity;
        _playerEndFlyingAngle = playerFlyingAngle;
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
        _playerTransform.RotateAround(_planets[0].transform.position, Vector3.up, _speedRotation * deltaTime);
    }

    private void OnTouchedDown(Vector3 touchPosition)
    {
        _isTouched = true;
    }
    
    private void OnTouchedUp(Vector3 touchPosition)
    {
        _isTouched = false;
    }
    
    private void PlayerEnteredPlanet()
    {
        _insidePlanet = true;
    }

    private void PlayerExitedPlanet()
    {
        _insidePlanet = false;
    }

    private void PlayerEnteredGravity(Vector3 contact)
    {
        _playerEndFlying = _planets[0].transform.position - contact;
    }

    private void PlayerExitedGravity()
    {
        
    }

    private void FlyingAngle()
    {
        _playerStartFlying = _planets[0].transform.position - _playerTransform.position;
        Debug.Log(_playerCurrentFlyingAngle);
        if (_playerCurrentFlyingAngle >= _playerEndFlyingAngle)
        {
            _engineForce = 0;
            _gravityForce = 0;
            _speedRotation = 0;
            Debug.Log($"Your way ended here! {_playerCurrentFlyingAngle}");
        }
        else
        {
            _playerCurrentFlyingAngle += Vector3.Angle(_playerStartFlying, _playerEndFlying);
            _playerEndFlying = _playerStartFlying;
        }
    }
    
    public void Execute(float deltaTime)
    {
        PlayerMove(_isTouched, deltaTime);
        PlayerRotation(deltaTime);
        FlyingAngle();
    }

    public void Clean()
    {
        _inputTouchDown.OnChange -= OnTouchedDown;
        _inputTouchUp.OnChange -= OnTouchedUp;
    }
}