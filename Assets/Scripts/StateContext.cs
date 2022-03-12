using Data;
using UnityEngine;
using Utils;
using View;

namespace DefaultNamespace
{
    public class StateContext : IExecute, IClean
    {
        private State _state;
        private int _planetIndex;
        private readonly PlanetView[] _planetViews;
        private readonly GravityView[] _gravityViews;
        private readonly PlayerView _playerView;
        
        private bool _isRightRotation;
        public bool IsLastPlanet;
        
        private RotationAroundPlanet _rotationAroundPlanet;
        private UpAndDownAroundPlanet _upAndDownAroundPlanet;
        private FlyPlanetAngle _flyPlanetAngle;
        private FlyToEdgeGravity _flyToEdgeGravity;
        private AimNextPlanet _aimNextPlanet;
        private FlyNextPlanet _flyNextPlanet;
        private TapExplosionController _tapExplosionController;
        
        private CameraController _cameraController;

        public StateContext(ScriptableData data, PlayerView playerView, IUserInput<Vector3>[] touchInput, 
            IUserInput<float>[] axisInput, PlanetView[] planetViews, GravityView[] gravityViews, Camera camera)
        {
            
            _planetViews = planetViews;
            _gravityViews = gravityViews;
            _playerView = playerView;

            var playerTransform = playerView.transform;
            _upAndDownAroundPlanet = new UpAndDownAroundPlanet(data.Planet.engineForce, data.Planet.gravity,
                playerTransform, _planetViews[_planetIndex], gravityViews[_planetIndex], touchInput);
            _rotationAroundPlanet = new RotationAroundPlanet(data.Planet.speedRotationAroundPlanet,
                playerTransform, _planetViews[_planetIndex].transform);
            _flyPlanetAngle =
                new FlyPlanetAngle(_planetViews[_planetIndex].transform, playerTransform, _planetViews[_planetIndex + 1].transform);
            _flyToEdgeGravity = new FlyToEdgeGravity(data.Planet.rotationSpeedToDirection,
                data.Planet.moveSpeedToDirection, _gravityViews[_planetIndex], playerTransform);
            _aimNextPlanet = new AimNextPlanet(touchInput, playerTransform, camera);
            _flyNextPlanet =
                new FlyNextPlanet(data.Planet.moveSpeedToDirection, _gravityViews[_planetIndex], playerTransform);
            _tapExplosionController = new TapExplosionController( touchInput, camera, data.LastPlanet.explosionArea,
                data.LastPlanet.explosionForce, data.LastPlanet.explosionParticle);
            _cameraController = new CameraController(camera, data.Camera.startUpDivision, data.Camera.upSpeed,
                data.Camera.upOffsetFromPlayer, axisInput, data.LastPlanet.center,
                data.Camera.firstPersonRotationSpeed, playerTransform);

            _state = new AimNextPlanetState(this);
        }

        public void TransitionTo(State state)
        {
            _state = state;
            _state.SetContext(this);
        }

        public bool ChangeCurrentPlanet()
        {
            _planetIndex += 1;
            if (_planetIndex == (int)ObjectNumber.Last)
            {
                _flyNextPlanet.ChangePlanet(_gravityViews[_planetIndex]);
                
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
            _flyNextPlanet.ChangePlanet(_gravityViews[_planetIndex]);
            _isRightRotation = false;
            return false;
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

        public bool FlyNextPlanet()
        {
            return _flyNextPlanet.IsFinished();
        }

        public void FlyNextPlanetActive(bool isActive)
        {
            _flyNextPlanet.SetActive(isActive);
        }

        public void LastPlanet()
        {
            _tapExplosionController.SetActive();
            _cameraController.FirstPersonActivation();
            Object.Destroy(_playerView.gameObject);
        }
        
        public void Execute(float deltaTime)
        {
            _cameraController.FollowPlayer(deltaTime);
            _state.Move(deltaTime);
        }

        public void Clean()
        {
            _upAndDownAroundPlanet.OnDestroy();
            _flyToEdgeGravity.OnDestroy();
            _aimNextPlanet.OnDestroy();
            _flyNextPlanet.OnDestroy();
            _tapExplosionController.OnDestroy();
        }
    }
}