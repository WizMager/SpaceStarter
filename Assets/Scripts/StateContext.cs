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
        

        private Vector3 _lookDirection;
        
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
                new FlyPlanetAngle(planetViews[0].transform, playerView.transform, planetViews[1].transform);
            _moveToDirection = new MoveToDirection(data.Planet.rotationSpeedToDirection,
                data.Planet.moveSpeedToDirection, gravityViews[0], playerView.transform);
            _aimNextPlanet = new AimNextPlanet(touchInput, playerView.transform);
            _cameraController = new CameraController(camera, data.Camera.startUpDivision, data.Camera.upSpeed,
                data.Camera.upOffsetFromPlayer, axisInput, data.LastPlanet.center,
                data.Camera.firstPersonRotationSpeed);

            _state = new AimNextPlanetState();
            _state.SetContext(this);
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
        }
        
        public Vector3 FlyAroundPlanet(float deltaTime)
        {
            _rotationAroundPlanet.Move(deltaTime);
            _upAndDownAroundPlanet.Move(deltaTime);
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

        public Vector3 AimNextPlanet()
        {
            return _aimNextPlanet.Aim();
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