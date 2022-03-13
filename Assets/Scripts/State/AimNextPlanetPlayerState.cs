using DefaultNamespace;

namespace State
{
    public class AimNextPlanetPlayerState : PlayerState
    {
        private readonly CameraController _cameraController;
        public AimNextPlanetPlayerState(PlayerController context, CameraController cameraController)
        {
            PlayerController = context;
            _cameraController = cameraController;
            PlayerController.AimNextPlanetActive(true);
        }
        public override void Move(float deltaTime)
        {
            _cameraController.FollowPlayer();
            if (!PlayerController.AimNextPlanet()) return;
            PlayerController.AimNextPlanetActive(false);
            var isLastPlanet = PlayerController.ChangeCurrentPlanet();
            PlayerController.TransitionTo(new FlyNextPlanetPlayerState(PlayerController, isLastPlanet, _cameraController));
        }
    }
}