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
    private bool _isRightRotated;
    private bool _firstStart = true;

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
            planetTransforms[i] = planetViews[i].transform;
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
                var lookDirection = (_planetsTransforms[_currentPlanetIndex].transform.position - _playerTransform.position).normalized;
                _movementController.SetDirection(lookDirection);
                _isEdgeAchived = false;
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
        _playerCurrentFlyingAngle = 0;
        _playerEndFlying = _planetsTransforms[_currentPlanetIndex].position - contact;
    }

    private void PlayerEnteredGravity()
    {
        _firstStart = false;
        if (!_isPathFinished)
        {
            _movementController.EdgeGravityState(false); 
        }
        else
        {
            _isPathFinished = false;
        }
    }
    
    private void PlayerExitedGravity()
    {
        _movementController.EdgeGravityState(true);
        
        if (!_isPathFinished) return;
        
        _isEdgeAchived = true;
        UnsignetFromPlanet(_currentPlanetIndex);
        _currentPlanetIndex++;
        SignetToPlanet(_currentPlanetIndex);

    }
    
    private void FlyingAngle()
    {
        _playerStartFlying = _planetsTransforms[_currentPlanetIndex].position - _playerTransform.position;
        if (_playerCurrentFlyingAngle >= _playerEndFlyingAngle)
        {
            var lookDirection = (_playerTransform.position - _planetsTransforms[_currentPlanetIndex].position).normalized;
            _isPathFinished = true;
            _movementController.SetDirection(lookDirection);
            Debug.Log(_playerCurrentFlyingAngle);
        }
        else
        {
             _playerCurrentFlyingAngle += Vector3.Angle(_playerStartFlying, _playerEndFlying);
             _playerEndFlying = _playerStartFlying;
        }
    }

    private void RotateBeforeAround()
    {
        var direction = _planetsTransforms[_currentPlanetIndex].localPosition - _playerTransform.localPosition;
        _playerTransform.right = direction;
        _isRightRotated = true;
    }

    public void Execute(float deltaTime)
    {
        if (!_isPathFinished)
        {
            if (_isRightRotated)
            {
                _movementController.MoveAroundPlanet(deltaTime, _planetsTransforms[_currentPlanetIndex]);
                _cameraController.RotateAroundPlanet(_playerTransform, _planetsTransforms[_currentPlanetIndex]);
                FlyingAngle();  
            }
            else
            {
                if (_firstStart)
                {
                    //RotateBeforeAround(); 
                    _movementController.FirstMove(deltaTime);
                    _cameraController.FollowPlayer(_playerTransform, 6f, deltaTime);
                }
                else
                {
                    RotateBeforeAround(); 
                }
            }
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