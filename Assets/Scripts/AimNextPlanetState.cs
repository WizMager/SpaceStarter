using UnityEngine;

namespace DefaultNamespace
{
    public class AimNextPlanetState : State
    {
        public AimNextPlanetState(StateContext context)
        {
            _stateContext = context;
            _stateContext.AimNextPlanetActive(true);
        }
        public override void Move(float deltaTime)
        {
            if (!_stateContext.AimNextPlanet()) return;
            _stateContext.AimNextPlanetActive(false);
            var isLastPlanet = _stateContext.ChangeCurrentPlanet();
            _stateContext.TransitionTo(new FlyNextPlanetState(_stateContext, isLastPlanet));
        }
    }
}