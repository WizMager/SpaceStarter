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
        }
        public override void Move(float deltaTime)
        {
            
        }
    }
}