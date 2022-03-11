namespace DefaultNamespace
{
    public class FlyNextPlanetState : State
    {
        public FlyNextPlanetState(StateContext context)
        {
            _stateContext = context;
        }
        
        public override void Move(float deltaTime)
        {
            if (!_stateContext.FlyNextPlanet()) return;
            if (_stateContext.isLastPlanet)
            {
               _stateContext.TransitionTo(new LastPlanetState(_stateContext._cameraController)); 
            }
            else
            {
                _stateContext.TransitionTo(new FlyAroundPlanetState(_stateContext));  
            }
        }
    }
}