using DefaultNamespace;
using InputClasses;
using Interface;
using Model;
using State;
using UnityEngine;
using Utils;
using View;

namespace Controller
{
    public class PlayerController : IExecute, IClean
    {
        private PlayerState _playerState;
        private int _planetIndex;
        private readonly PlanetView[] _planetViews;
        private readonly GravityView[] _gravityViews;
        private readonly PlayerModel _playerModel;
        private readonly DeadScreenView _deadScreenView;

        private readonly RotationAroundPlanet _rotationAroundPlanet;
        private readonly UpAndDownAroundPlanet _upAndDownAroundPlanet;
        private readonly FlyPlanetAngle _flyPlanetAngle;
        private readonly FlyToEdgeGravity _flyToEdgeGravity;
        private readonly AimNextPlanet _aimNextPlanet;
        private readonly FlyToNextPlanet _flyToNextPlanet;
        private readonly TapExplosionController _tapExplosionController;
        private readonly FlyToCenterGravity _flyToCenterGravity;
        private readonly LastPlanet _lastPlanet;

        public PlayerController(ScriptableData.ScriptableData data, PlayerView playerView, IUserInput<Vector3>[] touchInput, 
            IUserInput<SwipeData> swipeInput, PlanetView[] planetViews, GravityView[] gravityViews, Camera camera, 
            CameraColliderView cameraColliderView, PlayerModel playerModel, DeadScreenView deadScreenView)
        {
            
            _planetViews = planetViews;
            _gravityViews = gravityViews;
            _playerModel = playerModel;
            _deadScreenView = deadScreenView;

            var playerTransform = playerView.transform;
            var trajectoryCalculate = new TrajectoryCalculate(playerTransform, data.Planet.moveSpeedToNextPlanet, 
                data.Planet.iterationsCount, data.Planet.oneStepTimeIteration);
            _upAndDownAroundPlanet = new UpAndDownAroundPlanet(data.Planet.engineForce, data.Planet.gravity,
                playerTransform, _planetViews[_planetIndex], gravityViews[_planetIndex], touchInput);
            _rotationAroundPlanet = new RotationAroundPlanet(data.Planet.speedRotationAroundPlanet,
                playerTransform, _planetViews[_planetIndex].transform);
            _flyPlanetAngle =
                new FlyPlanetAngle(_planetViews[_planetIndex].transform, playerTransform, 
                    _planetViews[_planetIndex + 1].transform); 
            _flyToEdgeGravity = new FlyToEdgeGravity(data.Planet.rotationSpeedToEdgeGravity,
                data.Planet.moveSpeedToEdgeGravity, _gravityViews[_planetIndex], playerTransform);
            _aimNextPlanet = new AimNextPlanet(touchInput, playerView, camera, trajectoryCalculate);
            _flyToNextPlanet =
                new FlyToNextPlanet(_gravityViews[_planetIndex], trajectoryCalculate);
            _tapExplosionController = new TapExplosionController( touchInput, camera, data.LastPlanet.explosionArea,
                data.LastPlanet.explosionForce, data.LastPlanet.explosionParticle);
            _flyToCenterGravity = new FlyToCenterGravity(playerView,
                data.Planet.rotationInGravitySpeed, data.Planet.moveSpeedCenterGravity, _planetViews[_planetIndex].transform);
            _lastPlanet = new LastPlanet(gravityViews[(int)PlanetNumber.Last], trajectoryCalculate);
            
            _playerState = new AimNextPlanetPlayerState(this, new CameraController(camera, 
                data.Camera.upSpeed, data.Camera.upOffsetFromPlayer, swipeInput, data.LastPlanet.center,
                data.Camera.firstPersonRotationSpeed, playerView,  data.Camera.cameraDownPosition, data.Camera.cameraDownSpeed, 
                cameraColliderView, data.LastPlanet.cameraDownPosition, data.LastPlanet.cameraDownSpeed,
                data.LastPlanet.distanceFromLastPlanetToStop, data.LastPlanet.moveSpeedToLastPlanet, planetViews[(int)PlanetNumber.Last].transform));

            _playerModel.OnZeroHealth += ChangeDeadState;
        }

        public void TransitionTo(PlayerState playerState)
        {
            _playerState = playerState;
            _playerState.SetContext(this);
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
                _flyToNextPlanet.ChangePlanet(_gravityViews[_planetIndex]);
                
                _flyToEdgeGravity.ChangePlanet(_gravityViews[_planetIndex]);
                 _flyPlanetAngle.ChangePlanet(_planetViews[0].transform,
                     _planetViews[0].transform);
                 _rotationAroundPlanet.ChangePlanet(_planetViews[0].transform);
                 _upAndDownAroundPlanet.ChangePlanet(_planetViews[0], _gravityViews[0]);
                return true;
            }
            
            _flyToEdgeGravity.ChangePlanet(_gravityViews[_planetIndex]);
            _flyPlanetAngle.ChangePlanet(_planetViews[_planetIndex].transform,
                _planetViews[_planetIndex + 1].transform);
            _rotationAroundPlanet.ChangePlanet(_planetViews[_planetIndex].transform);
            _upAndDownAroundPlanet.ChangePlanet(_planetViews[_planetIndex], _gravityViews[_planetIndex]);
            _flyToNextPlanet.ChangePlanet(_gravityViews[_planetIndex]);
            _flyToCenterGravity.ChangePlanet(_planetViews[_planetIndex].transform);
            return false;
            }

        public void FlyCenterGravityActivate()
        {
            _flyToCenterGravity.Active();
        }
        
        public bool FlyCenterGravity(float deltaTime)
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

        public bool FlyNextPlanet(float deltaTime)
        {
            return _flyToNextPlanet.IsFinished(deltaTime);
        }

        public void FlyNextPlanetActive(bool isActive)
        {
            _flyToNextPlanet.SetActive(isActive);
        }

        public void CalculateAngle()
        {
            _flyPlanetAngle.CalculateAngle();
        }
        
        public bool LastPlanet(float deltaTime)
        {
            return _lastPlanet.FlyLastPlanet(deltaTime);
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
            _playerModel.OnZeroHealth -= ChangeDeadState;
        }
    }
}