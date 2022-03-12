using UnityEngine;

namespace DefaultNamespace
{
    public class FlyToEdgeGravityState : State
    {
        public FlyToEdgeGravityState(Vector3 lookDirection, StateContext context)
        {
            _stateContext = context;
            _stateContext.SetDirectionToEdge(lookDirection);
        }

        public override void Move(float deltaTime)
        {
            if (!_stateContext.FlyToEdgeGravity()) return;
            _stateContext.TransitionTo(new AimNextPlanetState(_stateContext));
        }
    }
}