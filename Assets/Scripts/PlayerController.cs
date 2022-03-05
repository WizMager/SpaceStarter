using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using View;

public class PlayerController : IExecute, IClean
{
    private readonly IUserInput<Vector3> _inputTouchDown;
    private readonly IUserInput<Vector3> _inputTouchUp;
    private readonly IUserInput<Vector3> _inputTouchHold;
    private readonly PlanetView[] _planetsViews;
    private readonly Transform _playerTransform;
    private readonly GravityView[] _gravityViews;
    private readonly float _playerEndFlyingAngle;

    private float _playerCurrentFlyingAngle;
    private Vector3 _playerStartFlying;
    private Vector3 _playerEndFlying;
    private bool _isPathFinished;
    private int _currentPlanetIndex = 0;
    private bool _isEdgeAchived;
    private readonly Transform[] _planetsTransforms;

    private readonly PlayerMovement _playerMovement;
    private readonly CameraController _cameraController;
    private readonly FlyToEdge _flyToEdge;
    private readonly FlyToNextPlanet _flyToNextPlanet;

    public PlayerController(Data data, 
        (IUserInput<Vector3> inputTouchDownDown, IUserInput<Vector3> inputTouchUp, IUserInput<Vector3> inputTouchHold) touchInput, 
        PlayerView playerView, PlanetView[] planetsViews, GravityView[] gravityViews, Camera camera)
    {
        _inputTouchDown = touchInput.inputTouchDownDown;
        _inputTouchUp = touchInput.inputTouchUp;
        _inputTouchHold = touchInput.inputTouchHold;
        _inputTouchDown.OnChange += OnTouchedDown;
        _inputTouchUp.OnChange += OnTouchedUp;
        _inputTouchHold.OnChange += OnTouchedHold;
        _playerTransform = playerView.transform;
        _planetsViews = planetsViews;
        _gravityViews = gravityViews;
        SignetToPlanet(_currentPlanetIndex);
        _playerEndFlyingAngle = data.Player.flyingAroundPlanetAngle;
        _planetsTransforms = SetPlanetsTransform(planetsViews);

        _playerMovement = new PlayerMovement(data.Player.engineForce, data.Player.gravity, 
            data.Player.speedRotationAroundPlanet, playerView.transform);
        _cameraController = new CameraController(camera, data.Player.cameraStartUpDivision, data.Player.cameraUpMultiply);
        _flyToEdge = new FlyToEdge(data.Player.speedRotationToEdgeGravity, data.Player.speedMoveToEdgeGravity);
        _flyToNextPlanet = new FlyToNextPlanet(data.Player.speedMoveToEdgeGravity, data.Player.speedRotationToEdgeGravity);
    }

    private Transform[] SetPlanetsTransform(PlanetView[] planetViews)
    {
        var planetTransforms = new Transform[planetViews.Length];
        for (int i = 0; i < planetViews.Length; i++)
        {
            planetTransforms[i] = planetViews[0].transform;
        }

        return planetTransforms;
    }
    
    private void OnTouchedDown(Vector3 touchPosition)
    {
        _playerMovement.PlayerTouched(true);
    }
    
    private void OnTouchedUp(Vector3 touchPosition)
    {
        if (!_isPathFinished)
        {
            _playerMovement.PlayerTouched(false);
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
        _playerMovement.InsidePlanet(true);
    }

    private void PlayerExitedPlanet()
    {
        _playerMovement.InsidePlanet(false);
    }

    private void PlayerFirstEnteredGravity(Vector3 contact)
    {
        _playerEndFlying = _planetsTransforms[_currentPlanetIndex].transform.position - contact;
    }

    private void PlayerEnteredGravity()
    {
        if (!_isPathFinished)
        {
            _playerMovement.EdgeGravityState(false); 
        }
        else
        {
            
        }
    }
    
    private void PlayerExitedGravity()
    {
        if (!_isPathFinished)
        {
            _playerMovement.EdgeGravityState(true);
        }
        else
        {
            _isEdgeAchived = true;
           _flyToEdge.Deactivate();
           _flyToNextPlanet.SetDirection(_planetsTransforms[_currentPlanetIndex + 1].transform.position);
           UnsignetFromPlanet(_currentPlanetIndex);
           SignetToPlanet(+_currentPlanetIndex);
        }
    }
    
    private void FlyingAngle()
    {
        _playerStartFlying = _planetsTransforms[_currentPlanetIndex].transform.position - _playerTransform.position;
        if (_playerCurrentFlyingAngle >= _playerEndFlyingAngle)
        {
            var endDirection = _playerTransform.position - _planetsTransforms[_currentPlanetIndex].transform.position;
            var startDirection = _playerTransform.forward;
            _isPathFinished = true;
            _flyToEdge.Activate(startDirection, endDirection);
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
            _playerMovement.MoveAroundPlanet(deltaTime, _planetsTransforms[_currentPlanetIndex].transform);
            _cameraController.RotateAroundPlanet(_playerTransform, _planetsTransforms[_currentPlanetIndex].transform);
            FlyingAngle();  
        }
        else
        {
            if (_flyToEdge.FlyingToEdge(_playerTransform, deltaTime))
            {
                _cameraController.FollowPlayer(_playerTransform, 10f, deltaTime);
            }
            else
            {
                _flyToNextPlanet.Move(_playerTransform, deltaTime);
                _cameraController.FollowPlayer(_playerTransform, 10f, deltaTime);
            }
        }
    }

    private void SignetToPlanet(int index)
    {
        _planetsViews[index].OnPlayerPlanetEnter += PlayerEnteredPlanet;
        _planetsViews[index].OnPlayerPlanetExit += PlayerExitedPlanet;
        _gravityViews[index].OnPlayerFirstGravityEnter += PlayerFirstEnteredGravity;
        _gravityViews[index].OnPlayerGravityEnter += PlayerEnteredGravity;
        _gravityViews[index].OnPlayerGravityExit += PlayerExitedGravity;
    }

    private void UnsignetFromPlanet(int index)
    {
        _planetsViews[index].OnPlayerPlanetEnter -= PlayerEnteredPlanet;
        _planetsViews[index].OnPlayerPlanetExit -= PlayerExitedPlanet;
        _gravityViews[index].OnPlayerFirstGravityEnter -= PlayerFirstEnteredGravity;
        _gravityViews[index].OnPlayerGravityEnter -= PlayerEnteredGravity;
        _gravityViews[index].OnPlayerGravityExit -= PlayerExitedGravity;
    }
    
    public void Clean()
    {
        _inputTouchDown.OnChange -= OnTouchedDown;
        _inputTouchUp.OnChange -= OnTouchedUp;
        _inputTouchHold.OnChange -= OnTouchedHold;
    }
}