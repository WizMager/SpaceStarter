using Controller;
using Utils;

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
            PlayerController.CameraState(CameraState.Follow, deltaTime);
            if (!PlayerController.FlyToNextPlanet(deltaTime)) return;
            PlayerController.CalculateAngle();
            PlayerController.FlyToNextPlanetActive(false);
            PlayerController.TransitionTo(new FlyToCenterGravityPlayerState(PlayerController));
        }
    }
}