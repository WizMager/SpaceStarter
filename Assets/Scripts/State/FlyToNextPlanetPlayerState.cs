using Controller;

namespace State
{
    public class FlyToNextPlanetPlayerState : PlayerState
    {
        private readonly CameraController _cameraController;

        public FlyToNextPlanetPlayerState(PlayerController context, CameraController cameraController)
        {
            PlayerController = context;
            _cameraController = cameraController;
            PlayerController.FlyToNextPlanetActive(true);
        }
        
        public override void Move(float deltaTime)
        {
            _cameraController.FollowPlayer();
            if (!PlayerController.FlyToNextPlanet(deltaTime)) return;
            PlayerController.CalculateAngle();
            PlayerController.FlyToNextPlanetActive(false);
            PlayerController.TransitionTo(new FlyToCenterGravityPlayerState(PlayerController, _cameraController));
        }
    }
}