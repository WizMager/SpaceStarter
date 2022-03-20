using Controller;
using UnityEngine;

namespace State
{
    public class FlyAroundPlanetPlayerState : PlayerState
    {
        private readonly CameraController _cameraController;
        public FlyAroundPlanetPlayerState(PlayerController context, CameraController cameraController)
        {
            PlayerController = context;
            _cameraController = cameraController;
        }
        public override void Move(float deltaTime)
        {
            _cameraController.FollowPlayer();
            var lookDirection = PlayerController.FlyAroundPlanet(deltaTime);
            if (lookDirection == Vector3.zero) return;
            PlayerController.TransitionTo(new FlyToEdgeGravityPlayerState(lookDirection, PlayerController, _cameraController));
        }
    }
}