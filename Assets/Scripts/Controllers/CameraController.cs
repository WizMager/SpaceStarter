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
        private readonly float _offsetBackFlyFromPlayer;
        private readonly float _offsetUpFlyFromPlayer;
        
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
            _offsetBackFlyFromPlayer = data.Camera.offsetBackFlyFromPlayer;
            _offsetUpFlyFromPlayer = data.Camera.offsetUpFlyFromPlayer;
            
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

        private void FlyFromPlanet()
        {
            Vector3 dirNorm = (_player.position - _planet.position).normalized;
            Vector3 target = _player.position + dirNorm * _offsetBackFlyFromPlayer + Vector3.up * _offsetUpFlyFromPlayer;
            _camera.position = Vector3.Lerp(_camera.position, target,  Time.deltaTime * _moveSpeedFlyFromPlanet);
        }
        private void FlyRadius()
        {
            var look = Quaternion.LookRotation(_planet.position - _camera.position);
            _camera.rotation = Quaternion.Lerp(_camera.rotation, look, _rotationSpeedFlyRadius * Time.deltaTime);
                        
            Vector3 dirNorm = (_player.position - _planet.position).normalized;
            Vector3 target = _player.position + dirNorm * _offsetBackFlyRadius + Vector3.up * _offsetUpFlyRadius;
            _camera.position = Vector3.Lerp(_camera.position, target,  Time.deltaTime * _moveSpeedFlyRadius);
        }

        private void FlyCameraDown()
        {
            var look = Quaternion.LookRotation(_planet.position - _camera.position);
            _camera.rotation = Quaternion.Lerp(_camera.rotation, look, _rotationSpeedFlyCameraDown * Time.deltaTime);
                        
            Vector3 dirNorm = (_player.position - _planet.position).normalized;
            Vector3 target = _player.position + dirNorm * _offsetBackFlyCameraDown + Vector3.up * _offsetUpFlyCameraDown;
            _camera.position = Vector3.Lerp(_camera.position, target,  Time.deltaTime * _moveSpeedFlyCameraDown);
        }
        
        private void ToFirstPerson()
        {
            var look = Quaternion.LookRotation(_planet.position - _camera.position);
            _camera.rotation = Quaternion.Lerp(_camera.rotation, look, _rotationSpeedFlyFirstPerson * Time.deltaTime);
                        
            Vector3 dirNorm = (_player.position - _planet.position).normalized;
            Vector3 target = _player.position + dirNorm * _offsetBackFlyFirstPerson + Vector3.up * _offsetUpFlyFirstPerson;
            _camera.position = Vector3.Lerp(_camera.position, target,  Time.deltaTime * _moveSpeedFlyFirstPerson);
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