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
            PlayerController.FlyNextPlanetActive(true);
        }
        
        public override void Move(float deltaTime)
        {
            _cameraController.FollowPlayer();
            if (!PlayerController.FlyNextPlanet(deltaTime)) return;
            PlayerController.CalculateAngle();
            PlayerController.FlyNextPlanetActive(false);
            PlayerController.TransitionTo(new FlyToCenterGravityPlayerState(PlayerController, _cameraController));
        }
    }
}