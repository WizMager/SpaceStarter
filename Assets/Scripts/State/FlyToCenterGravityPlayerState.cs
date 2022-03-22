using Controller;

namespace State
{
    public class FlyToCenterGravityPlayerState : PlayerState
    {
        private readonly CameraController _cameraController;
        
        public FlyToCenterGravityPlayerState(PlayerController context, CameraController cameraController)
        {
            PlayerController = context;
            _cameraController = cameraController;
            PlayerController.FlyCenterGravityActivate();
        }
        public override void Move(float deltaTime)
        {
            var finishDown = _cameraController.CameraDownPlanet(deltaTime);
            if (!PlayerController.FlyToCenterGravity(deltaTime)) return;
            if (!finishDown) return;
            
            PlayerController.TransitionTo(new FlyAroundPlanetPlayerState(PlayerController, _cameraController));
        }
    }
}