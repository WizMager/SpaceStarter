using DefaultNamespace;

namespace State
{
    public class LastPlanetFlyPlayerState : PlayerState
    {
        private readonly CameraController _cameraController;
        private bool _flyEnd;
        
        public LastPlanetFlyPlayerState(PlayerController context, CameraController cameraController)
        {
            PlayerController = context;
            _cameraController = cameraController;
        }
        public override void Move(float deltaTime)
        {
            if (_flyEnd)
            {
                PlayerController.TransitionTo(new LastPlanetShootState(PlayerController, _cameraController));
            }
            else
            {
                _cameraController.FlyLastPlanet(deltaTime);
                if (!PlayerController.LastPlanet(deltaTime)) return;
                
                _flyEnd = true;
            }
            
        }
    }
}