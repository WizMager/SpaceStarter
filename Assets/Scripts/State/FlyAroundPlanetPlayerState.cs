using Controllers;
using UnityEngine;

namespace State
{
    public class FlyAroundPlanetPlayerState : PlayerState
    {
        public FlyAroundPlanetPlayerState(PlayerController context)
        {
            PlayerController = context;
        }
        public override void Move(float deltaTime)
        {
            //var lookDirection = PlayerController.FlyAroundPlanet(deltaTime);
            //if (lookDirection == Vector3.zero) return;
           // PlayerController.TransitionTo(new FlyToEdgeGravityPlayerState(lookDirection, PlayerController));
        }
    }
}