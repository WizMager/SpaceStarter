namespace DefaultNamespace
{
    public class FlyAroundPlanetState : State
    {

        public override void Move(float deltaTime)
        {
            if (!_stateContext.FlyAroundPlanet(deltaTime)) return;
            _stateContext.TransitionTo(new FlyToEdgeGravityState());
        }
    }
}