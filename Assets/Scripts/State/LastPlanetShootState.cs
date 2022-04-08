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
            if (_isActivated) return;

            PlayerController.CameraDrift();

            if (!PlayerController.CameraState(CameraState.LastPlanetFirstPerson, deltaTime)) return;
            //TODO: realize CameraState.YourNewState after this if
            PlayerController.ShootLastPlanet();
            _isActivated = true;
        }
    }
}