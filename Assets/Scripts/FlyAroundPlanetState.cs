using UnityEngine;

namespace DefaultNamespace
{
    public class FlyAroundPlanetState : State
    {
        public FlyAroundPlanetState(StateContext context)
        {
            _stateContext = context;
        }
        public override void Move(float deltaTime)
        {
            var lookDirection = _stateContext.FlyAroundPlanet(deltaTime);
            if (lookDirection == Vector3.zero) return;
            _stateContext.TransitionTo(new FlyToEdgeGravityState(lookDirection, _stateContext));
        }
    }
}