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
        private States _state;
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

        private void ChangeState(States state)
        {
            _state = state;
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
            switch (_state)
            {
                case States.EdgeGravityToPlanet:
                    FollowPlayer();
                    break;
                case States.ToCenterGravity:
                    FollowPlayer();
                    break;
                case States.FlyAroundPlanet:
                    FollowPlayer();
                    break;
                case States.EdgeGravityFromPlanet:
                    FollowPlayer();
                    break;
                case States.LookToPlanet:
                    FollowPlayer();
                    break;
                case States.ShootPlanet:
                    FirstPerson();
                    break;
                case States.FlyIntoSunset:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}