using Controller;
using Utils;

namespace State
{
    public class LastPlanetFlyPlayerState : PlayerState
    {
        private bool _flyEnd;
        
        public LastPlanetFlyPlayerState(PlayerController context)
        {
            PlayerController = context;
        }
        public override void Move(float deltaTime)
        {
            if (_flyEnd)
            {
                PlayerController.TransitionTo(new LastPlanetShootState(PlayerController));
            }
            else
            {
                PlayerController.CameraState(CameraState.FlyToLastPlanet, deltaTime);
                if (!PlayerController.LastPlanet(deltaTime)) return;
                
                _flyEnd = true;
            }
        }
    }
}