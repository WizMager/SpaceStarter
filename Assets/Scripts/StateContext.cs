﻿using Data;
using UnityEngine;
using Utils;
using View;

namespace DefaultNamespace
{
    public class StateContext : IExecute, IClean
    {
        private State _state;
        private int _planetIndex = 1;
        private readonly PlanetView[] _planetViews;
        private readonly GravityView[] _gravityViews;
        private readonly PlayerView _playerView;
        

        private Vector3 _lookDirection;
        private bool _isRightRotation;
        
        private RotationAroundPlanet _rotationAroundPlanet;
        private UpAndDownAroundPlanet _upAndDownAroundPlanet;
        private FlyPlanetAngle _flyPlanetAngle;
        private MoveToDirection _moveToDirection;
        private AimNextPlanet _aimNextPlanet;
        private CameraController _cameraController;

        public StateContext(ScriptableData data, PlayerView playerView, IUserInput<Vector3>[] touchInput, 
            IUserInput<float>[] axisInput, PlanetView[] planetViews, GravityView[] gravityViews, Camera camera)
        {
            
            _planetViews = planetViews;
            _gravityViews = gravityViews;
            _playerView = playerView;

            _upAndDownAroundPlanet = new UpAndDownAroundPlanet(data.Planet.engineForce, data.Planet.gravity,
                playerView.transform, planetViews[_planetIndex], gravityViews[_planetIndex], touchInput);
            _rotationAroundPlanet = new RotationAroundPlanet(data.Planet.speedRotationAroundPlanet,
                playerView.transform, planetViews[_planetIndex].transform);
            _flyPlanetAngle =
                new FlyPlanetAngle(planetViews[_planetIndex].transform, playerView.transform, planetViews[_planetIndex + 1].transform);
            _moveToDirection = new MoveToDirection(data.Planet.rotationSpeedToDirection,
                data.Planet.moveSpeedToDirection, gravityViews[_planetIndex], playerView.transform);
            _aimNextPlanet = new AimNextPlanet(touchInput, playerView.transform, camera);
            _cameraController = new CameraController(camera, data.Camera.startUpDivision, data.Camera.upSpeed,
                data.Camera.upOffsetFromPlayer, axisInput, data.LastPlanet.center,
                data.Camera.firstPersonRotationSpeed);

            _state = new AimNextPlanetState(this);
            //_state = new FlyNextPlanetState(_planetViews[_planetIndex].transform.position - _playerView.transform.position, this);
        }

        public void TransitionTo(State state)
        {
            _state = state;
            _state.SetContext(this);
        }

        public void ChangeCurrentPlanet()
        {
            _planetIndex += 1;
            _moveToDirection.ChangePlanet(_gravityViews[_planetIndex]);
            _flyPlanetAngle.ChangePlanet(_planetViews[_planetIndex].transform, _planetViews[_planetIndex + 1].transform);
            _rotationAroundPlanet.ChangePlanet(_planetViews[_planetIndex].transform);
            _upAndDownAroundPlanet.ChangePlanet(_planetViews[_planetIndex], _gravityViews[_planetIndex]);
            _isRightRotation = false;
        }
        
        public Vector3 FlyAroundPlanet(float deltaTime)
        {
            if (!_isRightRotation)
            {
                var direction = _planetViews[_planetIndex].transform.position - _playerView.transform.position;
                _playerView.transform.right = direction;
                _isRightRotation = true;
            }
            _upAndDownAroundPlanet.Move(deltaTime);
            _rotationAroundPlanet.Move(deltaTime);
            return _flyPlanetAngle.FlewAngle();
        }

        public void SetDirectionToEdge(Vector3 lookDirection)
        {
            _moveToDirection.SetDirection(lookDirection);
        }
        
        public bool FlyToEdgeGravity()
        {
            return _moveToDirection.IsFinished();
        }

        public Vector3 AimNextPlanet(float deltaTime)
        {
            return _aimNextPlanet.Aim(deltaTime);
        }
        
        public void Execute(float deltaTime)
        {
            _cameraController.FollowPlayer(_playerView.transform, deltaTime);
            _state.Move(deltaTime);
        }

        public void Clean()
        {
            _upAndDownAroundPlanet.OnDestroy();
            _moveToDirection.OnDestroy();
            _aimNextPlanet.OnDestroy();
        }
    }
}