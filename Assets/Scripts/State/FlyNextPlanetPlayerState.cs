using DefaultNamespace;

namespace State
{
    public class FlyNextPlanetPlayerState : PlayerState
    {
        private readonly CameraController _cameraController;
        private readonly bool _isLastPlanet;
        
        public FlyNextPlanetPlayerState(PlayerController context, bool isLastPlanet, CameraController cameraController)
        {
            PlayerController = context;
            _isLastPlanet = isLastPlanet;
            _cameraController = cameraController;
            PlayerController.FlyNextPlanetActive(true);
        }
        
        public override void Move(float deltaTime)
        {
            _cameraController.FollowPlayer();
            if (!PlayerController.FlyNextPlanet()) return;
            
            PlayerController.FlyNextPlanetActive(false);
            if (_isLastPlanet)
            {
                PlayerController.TransitionTo(new LastPlanetPlayerState(PlayerController, _cameraController)); 
            }
            else
            {
                PlayerController.TransitionTo(new FlyCenterGravityPlayerState(PlayerController, _cameraController));  
            }
        }
    }
}