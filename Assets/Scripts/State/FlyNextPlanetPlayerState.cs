using DefaultNamespace;

namespace State
{
    public class FlyNextPlanetPlayerState : PlayerState
    {
        private readonly CameraController _cameraController;

        public FlyNextPlanetPlayerState(PlayerController context, CameraController cameraController)
        {
            PlayerController = context;
            _cameraController = cameraController;
            PlayerController.FlyNextPlanetActive(true);
        }
        
        public override void Move(float deltaTime)
        {
            _cameraController.FollowPlayer();
            if (!PlayerController.FlyNextPlanet()) return;
            PlayerController.CalculateAngle();
            PlayerController.FlyNextPlanetActive(false);
            PlayerController.TransitionTo(new FlyCenterGravityPlayerState(PlayerController, _cameraController));
        }
    }
}