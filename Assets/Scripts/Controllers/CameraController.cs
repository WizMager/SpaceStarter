using System;
using InputClasses;
using Interface;
using ScriptableData;
using UnityEngine;
using Utils;
using View;

namespace Controllers
{
    public class CameraController : IExecute, IClean
    {
        private readonly StateController _stateController;
        private GameState _gameState;
        private readonly Transform _player;
        private readonly Transform _camera;
        private readonly Transform _planet;
        private readonly IUserInput<SwipeData> _swipeInput;

        private readonly Vector3 _planetCenter;
        private readonly float _firstPersonRotationSpeed;
        private readonly float _firstPersonDriftSpeed;
        private bool _isFirstPerson;
        private bool _temporarilyStopDrift;
        private readonly float _cooldownDrift;
        private float _pastTimeSwipe;

        private readonly float _cameraCenterGravityDownPosition;
        private readonly float _cameraCenterGravityDownSpeed;

        private Vector3 _startVectorAround;
        private Vector3 _endVectorAround;

        private readonly float _cameraCenterGravityUpPosition;
        private readonly float _cameraCenterGravityUpSpeed;

        private readonly float _rotationSpeedFlyRadius;
        private readonly float _moveSpeedFlyRadius;
        private readonly float _offsetBackFlyRadius;
        private readonly float _offsetUpFlyRadius;
        
        private readonly float _moveSpeedFlyFromPlanet;
        private readonly float _offsetUpFlyFromPlanet;
        private readonly float _offsetBackFlyFromPlanet;

        private readonly float _rotationSpeedFlyCameraDown;
        private readonly float _moveSpeedFlyCameraDown;
        private readonly float _offsetBackFlyCameraDown;
        private readonly float _offsetUpFlyCameraDown;

        private readonly float _rotationSpeedFlyFirstPerson;
        private readonly float _moveSpeedFlyFirstPerson;
        private readonly float _offsetBackFlyFirstPerson;
        private readonly float _offsetUpFlyFirstPerson;
        
        private readonly float _timeFlyAway1;
        private readonly float _rotationSpeedFlyAway1;
        private readonly float _moveSpeedFlyAway1;
        private readonly float _offsetBackFlyAway1;
        private readonly float _offsetUpFlyAway1;
        
        private readonly float _rotationSpeedFlyAway2;
        private readonly float _moveSpeedFlyAway2;
        private readonly float _offsetBackFlyAway2;
        private readonly float _offsetUpFlyAway2;
        
        private float _stageTime;

        public CameraController(StateController stateController, Transform playerTransform, Transform cameraTransform,
            Transform planetTransform, AllData data, IUserInput<SwipeData> swipeInput)
        {
            _stateController = stateController;
            _player = playerTransform;
            _camera = cameraTransform;
            _planet = planetTransform;
            _swipeInput = swipeInput;

            _planetCenter = _planet.position;
            _firstPersonRotationSpeed = data.Camera.firstPersonRotationSpeed;
            _cooldownDrift = data.Planet.timeToDriftAgain;
            _firstPersonDriftSpeed = data.Camera.firstPersonDriftSpeed;

            _cameraCenterGravityDownPosition = data.Camera.cameraDownPosition;
            _cameraCenterGravityDownSpeed = data.Camera.cameraDownSpeed;

            _cameraCenterGravityUpPosition = data.Camera.upOffsetFromPlayer;
            _cameraCenterGravityUpSpeed = data.Camera.upSpeed;

            _rotationSpeedFlyRadius = data.Camera.rotationSpeedFlyRadius;
            _moveSpeedFlyFromPlanet = data.Camera.moveSpeedFlyFromPlanet;
            _offsetBackFlyFromPlanet = data.Camera.offsetBackFlyFromPlanet;
            _offsetUpFlyFromPlanet = data.Camera.offsetUpFlyFromPlanet;
            
            _moveSpeedFlyRadius = data.Camera.moveSpeedFlyRadius;
            _offsetBackFlyRadius = data.Camera.offsetBackFlyRadius;
            _offsetUpFlyRadius = data.Camera.offsetUpFlyRadius;
            
            _rotationSpeedFlyCameraDown = data.Camera.rotationSpeedFlyCameraDown;
            _moveSpeedFlyCameraDown = data.Camera.moveSpeedFlyCameraDown;
            _offsetBackFlyCameraDown = data.Camera.offsetBackFlyCameraDown;
            _offsetUpFlyCameraDown = data.Camera.offsetUpFlyCameraDown;
            
            _rotationSpeedFlyFirstPerson = data.Camera.rotationSpeedFlyFirstPerson;
            _moveSpeedFlyFirstPerson = data.Camera.moveSpeedFlyFirstPerson;
            _offsetBackFlyFirstPerson = data.Camera.offsetBackFlyFirstPerson;
            _offsetUpFlyFirstPerson = data.Camera.offsetUpFlyFirstPerson;

            _timeFlyAway1 = data.Camera.timeFlyAway1;
            _rotationSpeedFlyAway1 = data.Camera.rotationSpeedFlyAway1;
            _moveSpeedFlyAway1 = data.Camera.moveSpeedFlyAway1;
            _offsetBackFlyAway1 = data.Camera.offsetBackFlyAway1;
            _offsetUpFlyAway1 = data.Camera.offsetUpFlyAway1;
            
            _rotationSpeedFlyAway2 = data.Camera.rotationSpeedFlyAway2;
            _moveSpeedFlyAway2 = data.Camera.moveSpeedFlyAway2;
            _offsetBackFlyAway2 = data.Camera.offsetBackFlyAway2;
            _offsetUpFlyAway2 = data.Camera.offsetUpFlyAway2;

            _stateController.OnStateChange += ChangeState;
            _swipeInput.OnChange += CameraSwipeRotate;
            
            _stageTime = 0f;
        }

        private void ChangeState(GameState gameState)
        {
            _gameState = gameState;
            
            switch (gameState)
            {
                case GameState.FlyAroundPlanet:
                    FlyAroundPlanet(7f);
                    break;
                case GameState.EdgeGravityFromPlanet:
                    FlyAroundPlanet(-7f);
                    break;
                case GameState.ShootPlanet:
                    _isFirstPerson = true;
                    break;
                case GameState.FlyAway:
                    _isFirstPerson = false;
                    break;
            }
        }

        private void FollowPlayer()
        {
            var offsetPosition = _player.position;
            offsetPosition.y = _camera.position.y;
            _camera.position = offsetPosition;
        }

        private void ToCenterGravity(float deltaTime)
        {

            if (_camera.position.y <= _cameraCenterGravityDownPosition) return;
            var cameraY = _cameraCenterGravityDownSpeed * deltaTime;
            _camera.position -= new Vector3(0, cameraY, 0);
        }

        private void FlyAroundPlanet()
        {
            _startVectorAround = _player.position - _planet.position;
            var angle = Vector3.Angle(_startVectorAround, _endVectorAround);
            _camera.RotateAround(_planetCenter, _planet.up, angle);
            _endVectorAround = _startVectorAround;
        }

        private void FlyAroundPlanet(float angle)
        {
            _startVectorAround = _player.position - _planet.position;
            _camera.RotateAround(_planetCenter, _planet.up, angle);
            _endVectorAround = _startVectorAround;
        }
        
        private void EdgeGravityFromPlanet(float deltaTime)
        {
            if (_camera.position.y >= _cameraCenterGravityUpPosition) return;
            var cameraY = deltaTime * _cameraCenterGravityUpSpeed;
            _camera.position += new Vector3(0, cameraY, 0);
        }
        
        private void ShootPlanet(float deltaTime)
        {
            if (_temporarilyStopDrift)
            {
                if (_pastTimeSwipe < _cooldownDrift)
                {
                    _pastTimeSwipe += deltaTime;
                }
                else
                {
                    _pastTimeSwipe = 0;
                    _temporarilyStopDrift = false;
                }
            }
            else
            {
                _camera.RotateAround(_planetCenter, _camera.up, _firstPersonDriftSpeed * deltaTime);
            }
        }

        private void Fly(float moveSpeed, float offsetUp, float offsetBack)
        {
            Fly(0f, moveSpeed, offsetUp, offsetBack);
        }
        
        private void Fly(float rotationSpeed, float moveSpeed, float offsetUp, float offsetBack)
        {
            var planet = _planet.position;
            var player = _player.position;
            
            if (rotationSpeed != 0)
            {
                var look = Quaternion.LookRotation(planet - _camera.position);
                _camera.rotation = Quaternion.Lerp(_camera.rotation, look, Time.deltaTime * rotationSpeed);
            }
            
            Vector3 dirNorm = (player - planet).normalized;
            Vector3 target = player + dirNorm * offsetBack + Vector3.up * offsetUp;
            _camera.position = Vector3.Lerp(_camera.position, target,  Time.deltaTime * moveSpeed);
        }
        private void FlyFromPlanet()
        {
            Fly(_moveSpeedFlyFromPlanet, _offsetUpFlyFromPlanet, _offsetBackFlyFromPlanet);
        }
        private void FlyRadius()
        {
            Fly(_rotationSpeedFlyRadius, _moveSpeedFlyRadius, _offsetUpFlyRadius, _offsetBackFlyRadius);
        }

        private void FlyCameraDown()
        {
            Fly(_rotationSpeedFlyCameraDown, _moveSpeedFlyCameraDown, _offsetUpFlyCameraDown, _offsetBackFlyCameraDown);
        }
        
        private void ToFirstPerson()
        {
            Fly(_rotationSpeedFlyFirstPerson, _moveSpeedFlyFirstPerson, _offsetUpFlyFirstPerson, _offsetBackFlyFirstPerson);
        }
        
        private void CameraSwipeRotate(SwipeData swipeData)
        {
            if (!_isFirstPerson) return;
            _temporarilyStopDrift = true;
            switch (swipeData.Direction)
            {
                case SwipeDirection.Left:
                    _camera.RotateAround(_planetCenter, _camera.up, 
                        swipeData.Value * _firstPersonRotationSpeed); 
                    break;

                case SwipeDirection.Right:
                    _camera.RotateAround(_planetCenter, -_camera.up, 
                        swipeData.Value * _firstPersonRotationSpeed);
                    break;

                case SwipeDirection.Up:
                    _camera.RotateAround(_planetCenter, _camera.right, 
                        swipeData.Value * _firstPersonRotationSpeed);
                    break;

                case SwipeDirection.Down:
                    _camera.RotateAround(_planetCenter, -_camera.right, 
                        swipeData.Value * _firstPersonRotationSpeed);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("Unknown direction in SwipeData in Swipe Camera Rotate");
            }
        }
        
        private void FlyAway()
        {
            var planet = _planet.position;
            var player = _player.position;
            Quaternion look;
            
            if (_timeFlyAway1 > _stageTime)
            {
                look = Quaternion.LookRotation(planet - _camera.position);
                _camera.rotation = Quaternion.Lerp(_camera.rotation, look, Time.deltaTime * _rotationSpeedFlyAway1);

                Vector3 dirNorm = (player - planet).normalized;
                Vector3 target = player + dirNorm * _offsetBackFlyAway1 + Vector3.up * _offsetUpFlyAway1;
                
                _camera.position = Vector3.Lerp(_camera.position, target, Time.deltaTime * _moveSpeedFlyAway1);
            }
            else
            {
                look = Quaternion.LookRotation(player - _camera.position);
                _camera.rotation = Quaternion.Lerp(_camera.rotation, look, Time.deltaTime * _rotationSpeedFlyAway2);

                Vector3 dirNorm = (player - planet).normalized;
                Vector3 target = player + dirNorm * _offsetBackFlyAway2 + Vector3.up * _offsetUpFlyAway2;
                _camera.position = Vector3.Lerp(_camera.position, target, Time.deltaTime * _moveSpeedFlyAway2);
            }
            _stageTime += Time.deltaTime;
        }
        
        
        public void Execute(float deltaTime)
        {
            switch (_gameState)
            {
                case GameState.EdgeGravityToPlanet:
                    //FollowPlayer();
                    break;
                case GameState.ToCenterGravity:
                    ToCenterGravity(deltaTime);
                    break;
                case GameState.FlyAroundPlanet:
                    FlyAroundPlanet();
                    break;
                case GameState.EdgeGravityFromPlanet:
                    EdgeGravityFromPlanet(deltaTime);
                    break;
                case GameState.ArcFlyFromPlanet:
                    FlyFromPlanet();
                    break;
                case GameState.ArcFlyRadius:
                    FlyRadius();
                    break;
                case GameState.ArcFlyCameraDown:
                    FlyCameraDown();
                    break;
                case GameState.ArcFlyFirstPerson:
                    ToFirstPerson();
                    break;
                case GameState.ShootPlanet:
                    ShootPlanet(deltaTime);
                    break;
                case GameState.FlyAway:
                    FlyAway();
                    break;
                case GameState.EndFlyAway:
                    break;
                case GameState.RocketCrushed:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Clean()
        {
            _stateController.OnStateChange -= ChangeState;
            _swipeInput.OnChange -= CameraSwipeRotate; 
        }
    }
}