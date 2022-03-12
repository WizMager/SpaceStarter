namespace DefaultNamespace
{
    public class LastPlanetState : State
    {
        public LastPlanetState(StateContext context)
        {
            _stateContext = context;
            _stateContext.LastPlanet();
        }
        public override void Move(float deltaTime)
        {
            
        }
    }
}