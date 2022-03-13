using DefaultNamespace;

namespace State
{
    public class FlyCenterGravityPlayerState : PlayerState
    {
        private readonly CameraController _cameraController;
        
        public FlyCenterGravityPlayerState(PlayerController context, CameraController cameraController)
        {
            PlayerController = context;
            _cameraController = cameraController;
            PlayerController.FlyCenterGravityActivate();
        }
        public override void Move(float deltaTime)
        {
            _cameraController.CameraDown(deltaTime);
            if (!PlayerController.FlyCenterGravity(deltaTime)) return;
            
            PlayerController.TransitionTo(new FlyAroundPlanetPlayerState(PlayerController, _cameraController));
        }
    }
}