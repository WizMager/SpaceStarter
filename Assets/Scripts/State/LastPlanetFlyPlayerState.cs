using Controller;
using UnityEngine;
using Utils;

namespace State
{
    public class LastPlanetFlyPlayerState : PlayerState
    {
        private bool _flyEnd;
        
        public LastPlanetFlyPlayerState(PlayerController context)
        {
            PlayerController = context;
            PlayerController.FlyToNextPlanetActive(true);
        }
        public override void Move(float deltaTime)
        {
            if (_flyEnd)
            {
                PlayerController.FlyToNextPlanetActive(false);
                PlayerController.TransitionTo(new LastPlanetShootState(PlayerController));
            }
            else
            {
                PlayerController.CameraState(CameraState.FlyToLastPlanet, deltaTime);
                if (PlayerController.DeadZoneEnter())
                {
                    PlayerController.TransitionTo(new AimNextPlanetPlayerState(PlayerController, true));
                }
                
                if (!PlayerController.LastPlanet(deltaTime)) return;
                
                _flyEnd = true;
            }
        }
    }
}