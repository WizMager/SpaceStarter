using Controller;
using UnityEngine;

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
            if (PlayerController.ChangeCurrentPlanet())
            {
                PlayerController.TransitionTo(new LastPlanetFlyPlayerState(PlayerController, _cameraController));
            }
            else
            {
                PlayerController.TransitionTo(new FlyToNextPlanetPlayerState(PlayerController, _cameraController));  
            }
        }
    }
}