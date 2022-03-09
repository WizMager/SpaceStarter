using UnityEngine;
using Utils;
using View;

namespace DefaultNamespace
{
    public class UpAndDownAroundPlanet
    {
        private readonly float _engineForce;
        private readonly float _gravityForce;
        private readonly Transform _playerTransform;
        private GravityView _gravityView;
        private PlanetView _planetView;
        private readonly IUserInput<Vector3>[] _touch;

        private bool _isTouched;
        private bool _isInsidePlanet;
        private bool _isOutsideGravity;

        public UpAndDownAroundPlanet(float engineForce, float gravityForce, Transform playerTransform, 
            PlanetView currentPlanet, GravityView currentGravity, IUserInput<Vector3>[] touch)
        {
            _engineForce = engineForce;
            _gravityForce = gravityForce;
            _playerTransform = playerTransform;
            _planetView = currentPlanet;
            _gravityView = currentGravity;
            _touch = touch;

            _planetView.OnPlayerPlanetEnter += PlanetEntered;
            _planetView.OnPlayerPlanetExit += PlanetExited;
            _gravityView.OnPlayerGravityEnter += GravityEntered;
            _gravityView.OnPlayerGravityExit += GravityExited;
            _touch[(int) TouchInput.InputTouchDown].OnChange += TouchedDown;
            _touch[(int) TouchInput.InputTouchUp].OnChange += TouchedUp;
        }
        
        public void Move(float deltaTime)
        {
            var shipPositionAxisX = Vector3.zero;
            if (_isTouched && !_isOutsideGravity)
            {
                shipPositionAxisX.x = -_engineForce;
                _playerTransform.Translate(shipPositionAxisX * deltaTime);
            }
            else if (_isInsidePlanet && !_isOutsideGravity)
            {
                shipPositionAxisX.x = -_engineForce;
                _playerTransform.Translate(shipPositionAxisX * deltaTime);
            }
            else
            {
                shipPositionAxisX.x = _gravityForce;
                _playerTransform.Translate(shipPositionAxisX * deltaTime);
            }
        }
        
        private void PlanetEntered()
        {
            _isInsidePlanet = true;
        }
        
        private void PlanetExited()
        {
            _isInsidePlanet = false;
        }
        
        private void GravityEntered()
        {
            _isOutsideGravity = false;
        }
        
        private void GravityExited()
        {
            _isOutsideGravity = true;
        }

        private void TouchedDown(Vector3 value)
        {
            _isTouched = true;
        }

        private void TouchedUp(Vector3 value)
        {
            _isTouched = false;
        }

        public void ChangePlanet(PlanetView currentPlanet, GravityView currentGravity)
        {
            _planetView.OnPlayerPlanetEnter -= PlanetEntered;
            _planetView.OnPlayerPlanetExit -= PlanetExited;
            _gravityView.OnPlayerGravityEnter -= GravityEntered;
            _gravityView.OnPlayerGravityExit -= GravityExited;

            _planetView = currentPlanet;
            _gravityView = currentGravity;
            
            _planetView.OnPlayerPlanetEnter += PlanetEntered;
            _planetView.OnPlayerPlanetExit += PlanetExited;
            _gravityView.OnPlayerGravityEnter += GravityEntered;
            _gravityView.OnPlayerGravityExit += GravityExited;
        }
        
        public void OnDestroy()
        {
            _planetView.OnPlayerPlanetEnter -= PlanetEntered;
            _planetView.OnPlayerPlanetExit -= PlanetExited;
            _gravityView.OnPlayerGravityEnter -= GravityEntered;
            _gravityView.OnPlayerGravityExit -= GravityExited;
            _touch[(int) TouchInput.InputTouchDown].OnChange -= TouchedDown;
            _touch[(int) TouchInput.InputTouchUp].OnChange -= TouchedUp;
        }
    }
}