using System;
using DefaultNamespace;
using InputClasses;
using Interface;
using Model;
using State;
using UnityEngine;
using Utils;
using View;
using CameraState = Utils.CameraState;

namespace Controller
{
    public class PlayerController : IExecute, IClean
    {
        private PlayerState _playerState;
        private int _planetIndex;
        private readonly PlanetView[] _planetViews;
        private readonly GravityView[] _gravityViews;
        private readonly GravityEnterView[] _gravityEnterViews;
        private readonly PlayerModel _playerModel;
        private readonly DeadScreenView _deadScreenView;
        private readonly float _cameraBeforeRotateOffset;

        private readonly RotationAroundPlanet _rotationAroundPlanet;
        private readonly UpAndDownAroundPlanet _upAndDownAroundPlanet;
        private readonly FlyPlanetAngle _flyPlanetAngle;
        private readonly FlyToEdgeGravity _flyToEdgeGravity;
        private readonly AimNextPlanet _aimNextPlanet;
        private readonly FlyToNextPlanet _flyToNextPlanet;
        private readonly TapExplosionController _tapExplosionController;
        private readonly FlyToCenterGravity _flyToCenterGravity;
        private readonly LastPlanet _lastPlanet;
        private readonly CameraMove _cameraMove;

        public PlayerController(ScriptableData.ScriptableData data, PlayerView playerView, IUserInput<Vector3>[] touchInput, 
            IUserInput<SwipeData> swipeInput, PlanetView[] planetViews, GravityView[] gravityViews, GravityEnterView[] gravityEnterViews,
            Camera camera, CameraColliderView cameraColliderView, PlayerModel playerModel, DeadScreenView deadScreenView,
            DeadZoneView[] deadZoneViews, Transform missilePosition)
        {
            
            _planetViews = planetViews;
            _gravityViews = gravityViews;
            _gravityEnterViews = gravityEnterViews;
            _playerModel = playerModel;
            _deadScreenView = deadScreenView;
            _cameraBeforeRotateOffset = data.Camera.cameraOffsetBeforeRotation;
            
            var playerTransform = playerView.transform;
            var trajectoryCalculate = new TrajectoryCalculate(playerTransform, data.Planet.moveSpeedToNextPlanet, 
                data.Planet.iterationsCount, data.Planet.oneStepTimeIteration);
            _upAndDownAroundPlanet = new UpAndDownAroundPlanet(data.Planet.startEngineForce, data.Planet.startGravity,
                playerTransform, _planetViews[_planetIndex], gravityViews[_planetIndex], touchInput, 
                data.Planet.maxGravity, data.Planet.maxEngineForce, data.Planet.gravityAcceleration, 
                data.Planet.engineAcceleration);
            _rotationAroundPlanet = new RotationAroundPlanet(data.Planet.startSpeedRotationAroundPlanet,
                playerTransform, _planetViews[_planetIndex].transform);
            _flyPlanetAngle =
                new FlyPlanetAngle(_planetViews[_planetIndex].transform, playerTransform, 
                    _planetViews[_planetIndex + 1].transform); 
            _flyToEdgeGravity = new FlyToEdgeGravity(data.Planet.rotationSpeedToEdgeGravity,
                data.Planet.moveSpeedToEdgeGravity, _gravityEnterViews[_planetIndex], playerTransform);
            _aimNextPlanet = new AimNextPlanet(touchInput, playerView, camera, trajectoryCalculate);
            _flyToNextPlanet =
                new FlyToNextPlanet(_gravityEnterViews[_planetIndex], trajectoryCalculate, playerTransform, deadZoneViews);
            _tapExplosionController = new TapExplosionController( touchInput, camera, data, missilePosition);
            _flyToCenterGravity = new FlyToCenterGravity(playerView,
                data.Planet.rotationInGravitySpeed, data.Planet.moveSpeedCenterGravity, _planetViews[_planetIndex].transform);
            _lastPlanet = new LastPlanet(gravityViews[(int)PlanetNumber.Last], trajectoryCalculate, data.LastPlanet.moveSpeedFromAbove);
            _cameraMove = new CameraMove(camera,
                data.Camera.upSpeed, data.Camera.upOffsetFromPlayer, swipeInput,
                _planetViews[(int) PlanetNumber.Last].transform.position,
                data.Camera.firstPersonRotationSpeed, playerView, data.Camera.cameraDownPosition,
                data.Camera.cameraDownSpeed,
                cameraColliderView, data.LastPlanet.cameraDownPosition, data.LastPlanet.cameraDownSpeed,
                data.LastPlanet.distanceFromLastPlanetToStop, data.LastPlanet.moveSpeedFirstPerson,
                planetViews[(int) PlanetNumber.Last].transform, _planetViews[_planetIndex].transform, _flyPlanetAngle, 
                data.Camera.moveSpeed, data.Camera.cameraOffsetBeforeRotation, _flyToCenterGravity,
                data.LastPlanet.minimalPercentMoveSpeedFirstPerson);
            _playerState = new AimNextPlanetPlayerState(this, false);

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

        public bool ChangeCurrentPlanet()
        {
            _planetIndex += 1;
            
            if (_planetIndex == (int)PlanetNumber.Last)
            {
                _flyToNextPlanet.ChangePlanet(_gravityEnterViews[_planetIndex]);
                _flyToEdgeGravity.ChangePlanet(_gravityEnterViews[_planetIndex]);
                
                 _flyPlanetAngle.ChangePlanet(_planetViews[0].transform,
                     _planetViews[0].transform);
                 _rotationAroundPlanet.ChangePlanet(_planetViews[0].transform);
                 _upAndDownAroundPlanet.ChangePlanet(_planetViews[0], _gravityViews[0]);
                return true;
            }
            
            _flyToEdgeGravity.ChangePlanet(_gravityEnterViews[_planetIndex]);
            _flyPlanetAngle.ChangePlanet(_planetViews[_planetIndex].transform,
                _planetViews[_planetIndex + 1].transform);
            _rotationAroundPlanet.ChangePlanet(_planetViews[_planetIndex].transform);
            _upAndDownAroundPlanet.ChangePlanet(_planetViews[_planetIndex], _gravityViews[_planetIndex]);
            _flyToNextPlanet.ChangePlanet(_gravityEnterViews[_planetIndex]);
            _flyToCenterGravity.ChangePlanet(_planetViews[_planetIndex].transform);
            _cameraMove.ChangePlanet(_planetViews[_planetIndex].transform);
            return false;
            }

        public void FlyCenterGravityActivate()
        {
            _flyToCenterGravity.Active();
        }
        
        public bool FlyToCenterGravity(float deltaTime)
        {
            return _flyToCenterGravity.IsFinished(deltaTime);
        }
        
        public Vector3 FlyAroundPlanet(float deltaTime)
        {
            _upAndDownAroundPlanet.Move(deltaTime);
            _rotationAroundPlanet.Move(deltaTime);
            return _flyPlanetAngle.FlewAngle();
        }

        public void SetDirectionToEdge(Vector3 lookDirection)
        {
            _flyToEdgeGravity.SetDirection(lookDirection);
        }
        
        public bool FlyToEdgeGravity()
        {
            return _flyToEdgeGravity.IsFinished();
        }

        public bool AimNextPlanet()
        {
            return _aimNextPlanet.Aim();
        }

        public void AimNextPlanetActive(bool isActive)
        {
            _aimNextPlanet.SetActive(isActive);
        }

        public bool FlyToNextPlanet(float deltaTime)
        {
            return _flyToNextPlanet.IsFinished(deltaTime);
        }

        public bool DeadZoneEnter()
        {
            return _flyToNextPlanet.IsInDeadZone();
        }

        public void FlyToNextPlanetActive(bool isActive)
        {
            _flyToNextPlanet.SetActive(isActive);
        }

        public void CalculateAngle()
        {
            _flyPlanetAngle.CalculateAngle();
        }
        
        public bool LastPlanet(float deltaTime)
        {
            return _lastPlanet.FlyToLastPlanet(deltaTime);
        }

        public bool LastPlanetIndexCheck()
        {
            return _planetIndex == (int)PlanetNumber.Last;
        }
        
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
            _upAndDownAroundPlanet.OnDestroy();
            _flyToEdgeGravity.OnDestroy();
            _aimNextPlanet.OnDestroy();
            _flyToNextPlanet.OnDestroy();
            _tapExplosionController.OnDestroy();
            _cameraMove.OnDestroy();
            _playerModel.OnZeroHealth -= ChangeDeadState;
        }
    }
}