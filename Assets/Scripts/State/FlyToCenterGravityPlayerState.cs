using Controllers;
using Utils;

namespace State
{
    public class FlyToCenterGravityPlayerState : PlayerState
    {
        public FlyToCenterGravityPlayerState(PlayerController context)
        {
            PlayerController = context;
            PlayerController.FlyCenterGravityActivate();
        }
        public override void Move(float deltaTime)
        {
            var finishDown = PlayerController.CameraState(CameraState.CameraDown, deltaTime);
            //if (!PlayerController.FlyToCenterGravity(deltaTime)) return;
            if (!finishDown) return;
            
            PlayerController.CameraState(CameraState.RotateAroundPlanet, deltaTime);
            PlayerController.TransitionTo(new FlyAroundPlanetPlayerState(PlayerController));
        }
    }
}