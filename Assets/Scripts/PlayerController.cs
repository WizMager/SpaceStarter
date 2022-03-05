using System.Collections;
using System.Collections.Generic;
using Controller;
using UnityEngine;
using View;

public class PlayerController : IExecute, IClean
{
    private readonly IUserInput<Vector3> _inputTouchDown;
    private readonly IUserInput<Vector3> _inputTouchUp;
    private readonly IUserInput<Vector3> _inputTouchHold;
    private readonly PlayerView _playerView;
    private readonly Transform _playerTransform;
    private readonly PlanetView[] _planetsViews;
    private readonly GravityView[] _gravityViews;
    private readonly float _playerEndFlyingAngle;

    private float _playerCurrentFlyingAngle;
    private Vector3 _playerStartFlying;
    private Vector3 _playerEndFlying;
    private bool _isPathFinished;
    private int _currentPlanetIndex = 0;
    private bool _isEdgeAchived;
    private readonly Transform[] _planetsTransforms;

    private readonly MovementController _movementController;
    private readonly CameraController _cameraController;

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
        _playerView = playerView;
        _playerTransform = playerView.transform;
        _planetsViews = planetsViews;
        _gravityViews = gravityViews;
        SignetToPlanet(_currentPlanetIndex);
        _playerEndFlyingAngle = data.Player.flyingAroundPlanetAngle;
        _planetsTransforms = SetPlanetsTransform(planetsViews);

        _movementController = new MovementController(data.Player.engineForce, data.Player.gravity, 
            data.Player.speedRotationAroundPlanet, playerView, data.Player.speedRotationToEdgeGravity, 
            data.Player.speedMoveToEdgeGravity);
        _cameraController = new CameraController(camera, data.Player.cameraStartUpDivision, data.Player.cameraUpMultiply);
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
        _movementController.PlayerTouched(true);
    }
    
    private void OnTouchedUp(Vector3 touchPosition)
    {
        if (!_isPathFinished)
        {
            _movementController.PlayerTouched(false);
        }
        else
        {
            if (_isEdgeAchived)
            {
                
            }
        }
        
    }
    
    private void OnTouchedHold(Vector3 touchPosition)
    {
        
    }
    
    private void PlayerEnteredPlanet()
    {
        _movementController.InsidePlanet(true);
    }

    private void PlayerExitedPlanet()
    {
        _movementController.InsidePlanet(false);
    }

    private void PlayerFirstEnteredGravity(Vector3 contact)
    {
        _playerEndFlying = _planetsTransforms[_currentPlanetIndex].transform.position - contact;
    }

    private void PlayerEnteredGravity()
    {
        if (!_isPathFinished)
        {
            _movementController.EdgeGravityState(false); 
        }
        else
        {
            
        }
    }
    
    private void PlayerExitedGravity()
    {
        if (!_isPathFinished)
        {
            _movementController.EdgeGravityState(true);
        }
        else
        {
            _isEdgeAchived = true;
            UnsignetFromPlanet(_currentPlanetIndex);
            SignetToPlanet(+_currentPlanetIndex);
        }
    }
    
    private void FlyingAngle()
    {
        _playerStartFlying = _planetsTransforms[_currentPlanetIndex].transform.position - _playerTransform.position;
        if (_playerCurrentFlyingAngle >= _playerEndFlyingAngle)
        {
            var lookDirection = (_playerTransform.position - _planetsTransforms[_currentPlanetIndex].transform.position).normalized;
            _isPathFinished = true;
            _movementController.SetDirection(lookDirection);
            //_playerView.StartCoroutine(_moveToPoint.Rotate());
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
            _movementController.MoveAroundPlanet(deltaTime, _planetsTransforms[_currentPlanetIndex]);
            _cameraController.RotateAroundPlanet(_playerTransform, _planetsTransforms[_currentPlanetIndex]);
            FlyingAngle();  
        }
        else
        {
            _movementController.MoveToPoint(deltaTime);
            _cameraController.FollowPlayer(_playerTransform, 10f, deltaTime);
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