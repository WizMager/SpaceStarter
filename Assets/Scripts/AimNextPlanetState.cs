using UnityEngine;

namespace DefaultNamespace
{
    public class AimNextPlanetState : State
    {
        public AimNextPlanetState(StateContext context)
        {
            _stateContext = context;
        }
        public override void Move(float deltaTime)
        {
            var direction = _stateContext.AimNextPlanet();
            if (direction == Vector3.zero) return;
            _stateContext.TransitionTo(new FlyNextPlanetState(direction, _stateContext));
        }
    }
}