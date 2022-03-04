using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTopDownController : IExecute, IClean
{
    private readonly IUserInput<Vector3> _inputTouchDown;
    private readonly IUserInput<Vector3> _inputTouchUp;
    private readonly IUserInput<Vector3> _inputTouchHold;
    private readonly GameObject[] _planets;
    private readonly Transform _playerTransform;
    private readonly GameObject[] _gravityFields;
    private readonly float _playerEndFlyingAngle;

    private float _playerCurrentFlyingAngle;
    private Vector3 _playerStartFlying;
    private Vector3 _playerEndFlying;
    private bool _isPathFinished;
    private int _currentPlanetIndex = 0;
    private bool _isEdgeAchived;

    private readonly PlayerMovementTopDown _playerMovementTopDown;
    private readonly CameraTopDown _cameraTopDown;
    private readonly FlyToEdge _flyToEdge;
    private readonly FlyToNextPlanet _flyToNextPlanet;

    public PlayerTopDownController(Data data, 
        (IUserInput<Vector3> inputTouchDownDown, IUserInput<Vector3> inputTouchUp, IUserInput<Vector3> inputTouchHold) touchInput, 
        GameObject player, GameObject[] planets, GameObject[] gravityFields, IReadOnlyList<Camera> cameras)
    {
        _inputTouchDown = touchInput.inputTouchDownDown;
        _inputTouchUp = touchInput.inputTouchUp;
        _inputTouchHold = touchInput.inputTouchHold;
        _inputTouchDown.OnChange += OnTouchedDown;
        _inputTouchUp.OnChange += OnTouchedUp;
        _inputTouchHold.OnChange += OnTouchedHold;
        _playerTransform = player.GetComponent<Transform>();
        _planets = planets;
        _gravityFields = gravityFields;
        SignetToPlanet(_currentPlanetIndex);
        _playerEndFlyingAngle = data.Player.flyingAroundPlanetAngle;

        _playerMovementTopDown = new PlayerMovementTopDown(data.Player.engineForce, data.Player.gravity, 
            data.Player.speedRotationAroundPlanet, _playerTransform);
        _cameraTopDown = new CameraTopDown(cameras[0], data.Player.cameraStartUpDivision, data.Player.cameraUpMultiply);
        _flyToEdge = new FlyToEdge(data.Player.speedRotationToEdgeGravity, data.Player.speedMoveToEdgeGravity);
        _flyToNextPlanet = new FlyToNextPlanet(data.Player.speedMoveToEdgeGravity, data.Player.speedRotationToEdgeGravity);
    }

    private void OnTouchedDown(Vector3 touchPosition)
    {
        _playerMovementTopDown.PlayerTouched(true);
    }
    
    private void OnTouchedUp(Vector3 touchPosition)
    {
        if (!_isPathFinished)
        {
            _playerMovementTopDown.PlayerTouched(false);
        }
        else
        {
            if (_isEdgeAchived)
            {
                _flyToNextPlanet.SetActive(true); 
            }
        }
        
    }
    
    private void OnTouchedHold(Vector3 touchPosition)
    {
        // if (_isPathFinished)
        // {
        //     _playerMovementTopDown.RotationPlayer(touchPosition);
        // }
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
        _playerEndFlying = _planets[_currentPlanetIndex].transform.position - contact;
    }

    private void PlayerEnteredGravity()
    {
        if (!_isPathFinished)
        {
            _playerMovementTopDown.EdgeGravityState(false); 
        }
        else
        {
            
        }
    }
    
    private void PlayerExitedGravity()
    {
        if (!_isPathFinished)
        {
            _playerMovementTopDown.EdgeGravityState(true);
        }
        else
        {
            _isEdgeAchived = true;
           _flyToEdge.Activator(false);
           _flyToNextPlanet.SetDirection(_planets[_currentPlanetIndex + 1].transform.position);
           UnsignetFromPlanet(_currentPlanetIndex);
           SignetToPlanet(+_currentPlanetIndex);
        }
    }
    
    private void FlyingAngle()
    {
        _playerStartFlying = _planets[_currentPlanetIndex].transform.position - _playerTransform.position;
        if (_playerCurrentFlyingAngle >= _playerEndFlyingAngle)
        {
            _isPathFinished = true;
            _flyToEdge.Activator(true);
            //Debug.Log($"Your way ended here! {_playerCurrentFlyingAngle}");
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
            _playerMovementTopDown.MoveAroundPlanet(deltaTime, _planets[_currentPlanetIndex].transform);
            _cameraTopDown.RotateAroundPlanet(_playerTransform, _planets[_currentPlanetIndex].transform);
            FlyingAngle();  
        }
        else
        {
            if (_flyToEdge.FlyingToEdge(_playerTransform, deltaTime))
            {
                _cameraTopDown.FollowPlayer(_playerTransform, 10f, deltaTime);
            }
            else
            {
                _flyToNextPlanet.Move(_playerTransform, deltaTime);
                _cameraTopDown.FollowPlayer(_playerTransform, 10f, deltaTime);
            }
        }
    }

    private void SignetToPlanet(int index)
    {
        _planets[index].GetComponent<PlanetCollider>().OnPlayerPlanetEnter += PlayerEnteredPlanet;
        _planets[index].GetComponent<PlanetCollider>().OnPlayerPlanetExit += PlayerExitedPlanet;
        _gravityFields[index].GetComponent<GravityCollider>().OnPlayerFirstGravityEnter += PlayerFirstEnteredGravity;
        _gravityFields[index].GetComponent<GravityCollider>().OnPlayerGravityEnter += PlayerEnteredGravity;
        _gravityFields[index].GetComponent<GravityCollider>().OnPlayerGravityExit += PlayerExitedGravity;
    }

    private void UnsignetFromPlanet(int index)
    {
        _planets[index].GetComponent<PlanetCollider>().OnPlayerPlanetEnter -= PlayerEnteredPlanet;
        _planets[index].GetComponent<PlanetCollider>().OnPlayerPlanetExit -= PlayerExitedPlanet;
        _gravityFields[index].GetComponent<GravityCollider>().OnPlayerFirstGravityEnter -= PlayerFirstEnteredGravity;
        _gravityFields[index].GetComponent<GravityCollider>().OnPlayerGravityEnter -= PlayerEnteredGravity;
        _gravityFields[index].GetComponent<GravityCollider>().OnPlayerGravityExit -= PlayerExitedGravity;
    }
    
    public void Clean()
    {
        _inputTouchDown.OnChange -= OnTouchedDown;
        _inputTouchUp.OnChange -= OnTouchedUp;
        _inputTouchHold.OnChange -= OnTouchedHold;
    }
}