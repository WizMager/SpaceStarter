using System;
using InputClasses;
using Interface;
using Model;
using State;
using UnityEngine;
using Utils;
using View;
using CameraState = Utils.CameraState;

namespace Controllers
{
    public class PlayerController : IExecute, IClean
    {
        private PlayerState _playerState;
        private int _planetIndex;
        private readonly PlanetView[] _planetViews;
        private readonly PlayerModel _playerModel;
        private readonly DeadScreenView _deadScreenView;
        private readonly float _cameraBeforeRotateOffset;

        private readonly RotationAroundPlanet _rotationAroundPlanet;
        private readonly UpAndDownAroundPlanet _upAndDownAroundPlanet;
        //private readonly FlyPlanetAngle _flyPlanetAngle;
        private readonly FlyToEdgeGravity _flyToEdgeGravity;
        private readonly TapExplosionController _tapExplosionController;
        private readonly FlyToCenterGravity _flyToCenterGravity;
        private readonly CameraMove _cameraMove;

        public PlayerController(ScriptableData.AllData data, PlayerView playerView, IUserInput<Vector3>[] touchInput, 
            IUserInput<SwipeData> swipeInput, PlanetView[] planetViews, GravityOutColliderView gravityViews, GravityOutColliderView gravityEnterColliderViews,
            Camera camera, PlayerModel playerModel, DeadScreenView deadScreenView, Transform missilePosition)
        {
            
            _planetViews = planetViews;
            _playerModel = playerModel;
            _deadScreenView = deadScreenView;
            _cameraBeforeRotateOffset = data.Camera.cameraOffsetBeforeRotation;

            var playerTransform = playerView.transform;
            // _upAndDownAroundPlanet = new UpAndDownAroundPlanet(data.Planet.startEngineForce, data.Planet.startGravity,
            //     playerTransform, _planetViews[_planetIndex], gravityViews, touchInput, 
            //     data.Planet.maxGravity, data.Planet.maxEngineForce, data.Planet.gravityAcceleration, 
            //     data.Planet.engineAcceleration);
            _rotationAroundPlanet = new RotationAroundPlanet(data.Planet.startSpeedRotationAroundPlanet,
                playerTransform, _planetViews[_planetIndex].transform);
            // _flyPlanetAngle =
            //     new FlyPlanetAngle(_planetViews[_planetIndex].transform, playerTransform, 
            //         _planetViews[_planetIndex + 1].transform); 
            _flyToEdgeGravity = new FlyToEdgeGravity(data.Planet.rotationSpeedToEdgeGravity,
                data.Planet.moveSpeedToEdgeGravity, gravityViews, playerTransform);
            _tapExplosionController = new TapExplosionController( touchInput, camera, data, missilePosition);
            // _flyToCenterGravityController = new FlyToCenterGravityController(playerView,
            //     data.Planet.rotationInGravitySpeed, data.Planet.moveSpeedCenterGravity, _planetViews[_planetIndex].transform);
            //
            

            _playerModel.OnZeroHealth += ChangeDeadState;
        }

        public void TransitionTo(PlayerState playerState)
        {
            _playerState = playerState;
            _playerState.SetContext(this);
        }

        public bool CameraState(CameraState state, float deltaTime)
        {
            switch (state)
            {
                case Utils.CameraState.Follow:
                    _cameraMove.FollowPlayer();
                    return true;
                case Utils.CameraState.CameraUp:
                    return _cameraMove.CameraUp(deltaTime);
                case Utils.CameraState.CameraDown:
                    return _cameraMove.CameraDownPlanet(deltaTime);
                case Utils.CameraState.FlyToLastPlanet:
                    _cameraMove.FlyToLastPlanet(deltaTime);
                    return true;
                case Utils.CameraState.RotateAroundPlanet:
                    _cameraMove.RotateAroundPlanet(_cameraBeforeRotateOffset);
                    return true;
                case Utils.CameraState.LastPlanetFirstPerson:
                    return _cameraMove.CameraFlyStopped();
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        private void ChangeDeadState()
        {
            _deadScreenView.OnDead();
            TransitionTo(new DeadState());
        }

        public void FlyCenterGravityActivate()
        {
            //_flyToCenterGravityController.Active();
        }
        
        // public bool FlyToCenterGravity(float deltaTime)
        // {
        //     return _flyToCenterGravityController.IsFinished(deltaTime);
        // }
        
        // public Vector3 FlyAroundPlanet(float deltaTime)
        // {
        //     _upAndDownAroundPlanet.Move(deltaTime);
        //     _rotationAroundPlanet.Move(deltaTime);
        //     return _flyPlanetAngle.FlewAngle();
        // }

        public void SetDirectionToEdge(Vector3 lookDirection)
        {
            _flyToEdgeGravity.SetDirection(lookDirection);
        }
        
        public bool FlyToEdgeGravity()
        {
            return _flyToEdgeGravity.IsFinished();
        }
        

        // public void CalculateAngle()
        // {
        //     _flyPlanetAngle.CalculateAngle();
        // }
        
        
        public void FirstPersonActivation()
        {
            _cameraMove.FirstPersonActivation();
        }
        
        public void ShootLastPlanet()
        {
            _tapExplosionController.SetActive();
        }
        
        public void Execute(float deltaTime)
        {
            _playerState.Move(deltaTime);
        }

        public void Clean()
        {
            _flyToEdgeGravity.OnDestroy();
            _tapExplosionController.OnDestroy();
            _cameraMove.OnDestroy();
            _playerModel.OnZeroHealth -= ChangeDeadState;
        }
    }
}