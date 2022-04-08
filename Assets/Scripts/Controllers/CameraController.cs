using System;
using Interface;
using ScriptableData;
using UnityEngine;
using Utils;
using View;

namespace Controllers
{
    public class CameraController : IExecute
    {
        private readonly StateController _stateController;
        private GameState _gameState;
        private readonly Transform _player;
        private readonly Transform _camera;
        private readonly Transform _planet;

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
            Transform planet, AllData data)
        {
            _stateController = stateController;
            _player = playerTransform;
            _camera = cameraTransform;
            _planet = planet;
            
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
        }

        private void ChangeState(GameState gameState)
        {
            _gameState = gameState;
        }

        private void FollowPlayer()
        {
            var offsetPosition = _player.position;
            offsetPosition.y = _camera.position.y;
            _camera.position = offsetPosition;
        }

        private void FirstPerson()
        {
            _camera.position = _player.position;
            _camera.LookAt(_planet.position);
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
        
        public void Execute(float deltaTime)
        {
            switch (_gameState)
            {
                case GameState.EdgeGravityToPlanet:
                    FollowPlayer();
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
                    FirstPerson();
                    break;
                case GameState.FlyAway:
                    FollowPlayer();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}