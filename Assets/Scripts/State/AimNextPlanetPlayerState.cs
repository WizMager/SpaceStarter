using Controller;

namespace State
{
    public class AimNextPlanetPlayerState : PlayerState
    {
        private readonly bool _restart;
        public AimNextPlanetPlayerState(PlayerController context, bool restart)
        {
            PlayerController = context;
            _restart = restart;
            PlayerController.AimNextPlanetActive(true);
        }
        public override void Move(float deltaTime)
        {
            if (!PlayerController.AimNextPlanet()) return;
            
            PlayerController.AimNextPlanetActive(false);
            
            if (_restart)
            {
                if (PlayerController.LastPlanetIndexCheck())
                {
                    PlayerController.TransitionTo(new LastPlanetFlyPlayerState(PlayerController));
                }
                else
                {
                    PlayerController.TransitionTo(new FlyToNextPlanetPlayerState(PlayerController)); 
                }
            }
            else
            {
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
}