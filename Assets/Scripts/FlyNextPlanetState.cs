using UnityEngine;

namespace DefaultNamespace
{
    public class FlyNextPlanetState : State
    {
        private readonly bool _isLastPlanet;
        
        public FlyNextPlanetState(StateContext context, bool isLastPlanet)
        {
            _stateContext = context;
            _isLastPlanet = isLastPlanet;
            _stateContext.FlyNextPlanetActive(true);
        }
        
        public override void Move(float deltaTime)
        {
            if (!_stateContext.FlyNextPlanet()) return;
            _stateContext.FlyNextPlanetActive(false);
            if (_isLastPlanet)
            {
                _stateContext.TransitionTo(new LastPlanetState(_stateContext)); 
            }
            else
            {
                _stateContext.TransitionTo(new FlyAroundPlanetState(_stateContext));  
            }
        }
    }
}