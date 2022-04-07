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

        private readonly float _rotationSpeedFlyFromPlanet;
        private readonly float _moveSpeedFlyFromPlanet;
        private readonly float _offsetBackFromPlayer;
        private readonly float _offsetUpFromPlayer;
        private readonly float _offsetAroundPlanet;        

        public CameraController(StateController stateController, Transform playerTransform, Transform cameraTransform,
            Transform planet, AllData data)
        {
            _stateController = stateController;
            _player = playerTransform;
            _camera = cameraTransform;
            _planet = planet;

            _rotationSpeedFlyFromPlanet = data.Camera.rotationSpeedFlyFromPlanet;
            _moveSpeedFlyFromPlanet = data.Camera.moveSpeedFlyFromPlanet;
            _offsetBackFromPlayer = data.Camera.offsetBackFromPlayer;
            _offsetUpFromPlayer = data.Camera.offsetUpFromPlayer;
            _offsetAroundPlanet = data.Camera.offsetAroundPlanet;
            
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
        
        private void FreeFly()
        {
            var look = Quaternion.LookRotation(_planet.position - _camera.position);
            _camera.rotation = Quaternion.Lerp(_camera.rotation, look, _rotationSpeedFlyFromPlanet * Time.deltaTime);
                        
            Vector3 dirNorm = (_player.position - _planet.position).normalized;
            Vector3 target = _player.position + dirNorm * _offsetBackFromPlayer + Vector3.up * _offsetUpFromPlayer;
            _camera.position = Vector3.Lerp(_camera.position, target,  Time.deltaTime * _moveSpeedFlyFromPlanet);
            //_camera.transform.RotateAround(_planet.position, _planet.up, _offsetAroundPlanet);
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
                    FreeFly();
                    break;
                case GameState.ArcFlyRadius:
                    FollowPlayer();
                    break;
                case GameState.ArcFlyCameraDown:
                    FollowPlayer();
                    break;
                case GameState.ArcFlyFirstPerson:
                    FirstPerson();
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