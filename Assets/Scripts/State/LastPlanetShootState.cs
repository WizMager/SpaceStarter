using Controller;
using Utils;

namespace State
{
	public class LastPlanetShootState : PlayerState
    {
        private bool _isActivated;

        public LastPlanetShootState(PlayerController context)
        {
            PlayerController = context;
            PlayerController.FirstPersonActivation();
        }
        
        public override void Move(float deltaTime)
        {
            if (_isActivated)
			{
                PlayerController.CameraState(CameraState.CameraDrift, deltaTime);
                return;
            }

            if (!PlayerController.CameraState(CameraState.LastPlanetFirstPerson, deltaTime)) return;
            
            PlayerController.ShootLastPlanet();
            _isActivated = true;
        }
    }
}