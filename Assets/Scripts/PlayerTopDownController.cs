using System.Collections;
using UnityEngine;

public class PlayerTopDownController : IExecute, IClean
{
    private IUserInput<Vector3> _inputTouchDown;
    private IUserInput<Vector3> _inputTouchUp;
    private GameObject[] _planets;
    private Transform _playerTransform;
    private GameObject[] _gravityFields;
    private float _playerEndFlyingAngle;
    
    private float _playerCurrentFlyingAngle;
    private Vector3 _playerStartFlying;
    private Vector3 _playerEndFlying;
    private bool _isTouched;
    private bool _isPathFinished;

    private PlayerMovementTopDown _playerMovementTopDown;
    private FlyToEdge _flyToEdge;

    public PlayerTopDownController((IUserInput<Vector3> inputTouchDownDown, IUserInput<Vector3> inputTouchUp) touchInput, 
        GameObject player, float gravityForce, float engineForce, GameObject[] planets, float speedRotation, 
        GameObject[] gravityFields, float playerFlyingAngle)
    {
        _playerTransform = player.GetComponent<Transform>();
        _inputTouchDown = touchInput.inputTouchDownDown;
        _inputTouchUp = touchInput.inputTouchUp;
        _inputTouchDown.OnChange += OnTouchedDown;
        _inputTouchUp.OnChange += OnTouchedUp;
        _planets = planets;
        _planets[0].GetComponent<PlanetCollider>().OnPlayerPlanetEnter += PlayerEnteredPlanet;
        _planets[0].GetComponent<PlanetCollider>().OnPlayerPlanetExit += PlayerExitedPlanet;
        _gravityFields = gravityFields;
        _gravityFields[0].GetComponent<GravityCollider>().OnPlayerFirstGravityEnter += PlayerFirstEnteredGravity;
        _gravityFields[0].GetComponent<GravityCollider>().OnPlayerGravityEnter += PlayerEnteredGravity;
        _gravityFields[0].GetComponent<GravityCollider>().OnPlayerGravityExit += PlayerExitedGravity;
        _playerEndFlyingAngle = playerFlyingAngle;
        
        _playerMovementTopDown = new PlayerMovementTopDown(engineForce, gravityForce, speedRotation, _playerTransform);
        _flyToEdge = new FlyToEdge(speedRotation);
    }

    private void OnTouchedDown(Vector3 touchPosition)
    {
        _playerMovementTopDown.PlayerTouched(true);
    }
    
    private void OnTouchedUp(Vector3 touchPosition)
    {
        _playerMovementTopDown.PlayerTouched(false);
    }
    
    private void PlayerEnteredPlanet()
    {
        _playerMovementTopDown.InsidePlanet(true);
    }

    private void PlayerExitedPlanet()
    {
        _playerMovementTopDown.InsidePlanet(false);
    }

    private void PlayerFirstEnteredGravity(Vector3 contact)
    {
        _playerEndFlying = _planets[0].transform.position - contact;
    }

    private void PlayerEnteredGravity()
    {
        _playerMovementTopDown.EdgeGravityPlayerState(false);
    }
    
    private void PlayerExitedGravity()
    {
        if (!_isPathFinished)
        {
            _playerMovementTopDown.EdgeGravityPlayerState(true);
        }
        else
        {
           _flyToEdge.Activator(false); 
        }
    }

    private void FlyingAngle()
    {
        _playerStartFlying = _planets[0].transform.position - _playerTransform.position;
        if (_playerCurrentFlyingAngle >= _playerEndFlyingAngle)
        {
            _isPathFinished = true;
            _flyToEdge.Activator(true);
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
        if (!_isPathFinished)
        {
            _playerMovementTopDown.Move(deltaTime);
            _playerMovementTopDown.Rotation(deltaTime, _planets[0].transform);
            FlyingAngle();  
        }
        else
        {
            _flyToEdge.FlyingToEdge(_playerTransform, deltaTime);
        }
    }

    public void Clean()
    {
        _inputTouchDown.OnChange -= OnTouchedDown;
        _inputTouchUp.OnChange -= OnTouchedUp;
    }
}