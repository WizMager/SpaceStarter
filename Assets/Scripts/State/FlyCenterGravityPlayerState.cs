using Controller;

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
            var finishDown = _cameraController.CameraDownPlanet(deltaTime);
            if (!PlayerController.FlyCenterGravity(deltaTime)) return;
            if (!finishDown) return;
            
            PlayerController.TransitionTo(new FlyAroundPlanetPlayerState(PlayerController, _cameraController));
        }
    }
}