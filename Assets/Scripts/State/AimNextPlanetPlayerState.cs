using Controller;

namespace State
{
    public class AimNextPlanetPlayerState : PlayerState
    {
        public AimNextPlanetPlayerState(PlayerController context)
        {
            PlayerController = context;
            PlayerController.AimNextPlanetActive(true);
        }
        public override void Move(float deltaTime)
        {
            if (!PlayerController.AimNextPlanet()) return;
            
            PlayerController.AimNextPlanetActive(false);
            if (PlayerController.ChangeCurrentPlanet())
            {
                PlayerController.TransitionTo(new LastPlanetFlyPlayerState(PlayerController));
            }
            else
            {
                PlayerController.TransitionTo(new FlyToNextPlanetPlayerState(PlayerController));  
            }
        }
    }
}