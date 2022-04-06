using System;
using Interface;
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

        public CameraController(StateController stateController, Transform playerTransform, Transform cameraTransform,
            Transform planet)
        {
            _stateController = stateController;
            _player = playerTransform;
            _camera = cameraTransform;
            _planet = planet;

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