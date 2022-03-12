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
            
            _cameraController.SetCameraDown(10);
        }
        public override void Move(float deltaTime)
        {
            if (!PlayerController.AimNextPlanet()) return;
            PlayerController.AimNextPlanetActive(false);
            var isLastPlanet = PlayerController.ChangeCurrentPlanet();
            PlayerController.TransitionTo(new FlyNextPlanetPlayerState(PlayerController, isLastPlanet, _cameraController));
        }
    }
}