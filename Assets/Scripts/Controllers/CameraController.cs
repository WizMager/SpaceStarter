using System;
using InputClasses;
using Interface;
using ScriptableData;
using UnityEngine;
using Utils;

namespace Controllers
{
    public class CameraController : IExecute, IClean
    {
        private readonly StateController _stateController;
        private GameState _gameState;
        private readonly Transform _player;
        private readonly Transform _camera;
        private readonly Transform _planetTransform;
        private readonly IUserInput<SwipeData> _swipeInput;
        private readonly Transform _gravityTransform;
        private readonly float _startDistanceFromPlanet;

        private readonly Vector3 _planetCenter;
        private readonly float _firstPersonRotationSpeed;
        private readonly float _firstPersonDriftSpeed;
        private bool _isFirstPerson;
        private bool _temporarilyStopDrift;
        private readonly float _cooldownDrift;
        private float _pastTimeSwipe;

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


        public CameraController(StateController stateController, Transform playerTransform, Transform cameraTransform,
            Transform planetTransform, AllData data, IUserInput<SwipeData> swipeInput, Transform gravityTransform)
        {
            _stateController = stateController;
            _player = playerTransform;
            _camera = cameraTransform;
            _planetTransform = planetTransform;
            _swipeInput = swipeInput;
            _gravityTransform = gravityTransform;
            _startDistanceFromPlanet = data.Planet.distanceFromCenterPlanetToSpawn;

            _planetCenter = _planetTransform.position;
            _firstPersonRotationSpeed = data.Camera.firstPersonRotationSpeed;
            _cooldownDrift = data.Planet.timeToDriftAgain;
            _firstPersonDriftSpeed = data.Camera.firstPersonDriftSpeed;

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
            
            _stateController.OnStateChange += ChangeState;
            _swipeInput.OnChange += CameraSwipeRotate;
            
            //SetStartPosition();
        }

        private void ChangeState(GameState gameState)
        {
            _gameState = gameState;
            _isFirstPerson = gameState == GameState.ShootPlanet;
        }

        private void SetStartPosition()
        {
            var ray = new Ray(_player.position, _player.forward);
            var planetRadius = _planetTransform.GetComponent<SphereCollider>().radius;
            var distanceHalfGravity = (_gravityTransform.GetComponent<MeshCollider>().bounds.size.x / 2 - planetRadius) / 2;
            var distanceToCenterGravity = _startDistanceFromPlanet - planetRadius - distanceHalfGravity;
            var startPosition = ray.GetPoint(distanceToCenterGravity);
            startPosition.y = 40f;
            var startRotation = Quaternion.Euler(new Vector3(90, 90, 180));
            _camera.SetPositionAndRotation(startPosition, startRotation);
        }
        
        private void FollowPlayer()
        {
            var offsetPosition = _player.position;
            offsetPosition.y = _camera.position.y;
            _camera.position = offsetPosition;
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
            var planet = _planetTransform.position;
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
        
        public void Execute(float deltaTime)
        {
            switch (_gameState)
            {
                case GameState.EdgeGravityToPlanet:
                    //FollowPlayer();
                    break;
                case GameState.ToCenterGravity:
                    FollowPlayer();
                    break;
                case GameState.FlyAroundPlanet:
                    FollowPlayer();
                    break;
                case GameState.EdgeGravityFromPlanet:
                    FollowPlayer();
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
                    FollowPlayer();
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