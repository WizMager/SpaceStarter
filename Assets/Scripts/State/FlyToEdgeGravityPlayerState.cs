using Controller;
using UnityEngine;
using Utils;

namespace State
{
    public class FlyToEdgeGravityPlayerState : PlayerState
    {
        public FlyToEdgeGravityPlayerState(Vector3 lookDirection, PlayerController context)
        {
            PlayerController = context;
            PlayerController.SetDirectionToEdge(lookDirection);
        }

        public override void Move(float deltaTime)
        {
            var finishUp = PlayerController.CameraState(CameraState.CameraUp, deltaTime);
            if (!PlayerController.FlyToEdgeGravity()) return;
            if (!finishUp) return;
            
                //PlayerController.TransitionTo(new AimNextPlanetPlayerState(PlayerController, false));
        }
    }
}