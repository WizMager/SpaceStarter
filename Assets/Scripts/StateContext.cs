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

        private Vector3 _lookDirection;
        
        private RotationAroundPlanet _rotationAroundPlanet;
        private UpAndDownAroundPlanet _upAndDownAroundPlanet;
        private FlyPlanetAngle _flyPlanetAngle;
        

        public StateContext(ScriptableData data, PlayerView playerView, IUserInput<Vector3>[] touchInput, 
            IUserInput<float>[] axisInput, PlanetView[] planetViews, GravityView[] gravityViews, Camera camera)
        {
            _upAndDownAroundPlanet = new UpAndDownAroundPlanet(data.Planet.engineForce, data.Planet.gravity,
                playerView.transform, planetViews[_planetIndex], gravityViews[_planetIndex], touchInput);
            _rotationAroundPlanet = new RotationAroundPlanet(data.Planet.speedRotationAroundPlanet,
                playerView.transform, planetViews[_planetIndex].transform);
            _flyPlanetAngle =
                new FlyPlanetAngle(planetViews[0].transform, playerView.transform, planetViews[1].transform);
            
        }

        public void TransitionTo(State state)
        {
            _state = state;
            _state.SetContext(this);
        }
        
        public bool FlyAroundPlanet(float deltaTime)
        {
            _rotationAroundPlanet.Move(deltaTime);
            _upAndDownAroundPlanet.Move(deltaTime);
            if (_flyPlanetAngle.FlewAngle() == Vector3.zero)
            {
               return false; 
            }
            else
            {
                _lookDirection = _flyPlanetAngle.FlewAngle();
                return true;
            }
        }

        public void FlyToEdgeGravity(float deltaTime)
        {
            
        }

        public void Execute(float deltaTime)
        {
            _state.Move(deltaTime);
        }

        public void Clean()
        {
            _upAndDownAroundPlanet.OnDestroy();
        }
    }
}