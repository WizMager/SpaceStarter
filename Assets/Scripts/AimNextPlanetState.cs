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
            if (!_stateContext.AimNextPlanet()) return;
            
            _stateContext.TransitionTo(new FlyNextPlanetState(_stateContext));
        }
    }
}