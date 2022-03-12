using DefaultNamespace;

namespace State
{
    public class LastPlanetPlayerState : PlayerState
    {
        private readonly CameraController _cameraController;
        public LastPlanetPlayerState(PlayerController context, CameraController cameraController)
        {
            PlayerController = context;
            _cameraController = cameraController;
            PlayerController.LastPlanet();
        }
        public override void Move(float deltaTime)
        {
            
        }
    }
}