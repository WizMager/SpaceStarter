using System;
using Interface;
using ScriptableData;
using UnityEngine;
using Utils;

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

        public CameraController(Transform cameraTransform, Transform playerTransform,
            Transform planetTransform, AllData data, StateController stateController)
        {
            _stateController = stateController;
            _player = playerTransform;
            _camera = cameraTransform;
            _planet = planetTransform;
            
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

            _stageTime = 0f;
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
                    FlyAway();
                    break;
                case GameState.EndFlyAway:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}