using UnityEngine;

namespace DefaultNamespace
{
    public class FlyNextPlanetState : State
    {
        public FlyNextPlanetState(Vector3 direction, StateContext context)
        {
            _stateContext = context;
            _stateContext.SetDirectionToEdge(direction);
        }
        
        public override void Move(float deltaTime)
        {
            if (!_stateContext.FlyToEdgeGravity()) return;
            _stateContext.TransitionTo(new FlyAroundPlanetState(_stateContext));
        }
    }
}