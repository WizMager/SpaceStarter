using Controller;

namespace State
{
    public class FlyToNextPlanetPlayerState : PlayerState
    {
        public FlyToNextPlanetPlayerState(PlayerController context)
        {
            PlayerController = context;
            PlayerController.FlyToNextPlanetActive(true);
        }
        
        public override void Move(float deltaTime)
        {
            if (PlayerController.DeadZoneEnter())
            {
                PlayerController.TransitionTo(new AimNextPlanetPlayerState(PlayerController, true));
            }
            
            
            if (!PlayerController.FlyToNextPlanet(deltaTime)) return;
            
            PlayerController.CalculateAngle();
            PlayerController.FlyToNextPlanetActive(false);
            PlayerController.TransitionTo(new FlyToCenterGravityPlayerState(PlayerController));
        }
    }
}