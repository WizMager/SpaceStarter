using UnityEngine;

namespace DefaultNamespace
{
    public class MoveAroundPlanet
    {
        private readonly float _engineForce;
        private readonly float _gravityForce;
        private readonly float _speedRotation;
        private readonly Transform _playerTransform;

        private bool _isTouched;
        private bool _insidePlanet;
        private bool _outsideGravity;

        public MoveAroundPlanet(float engineForce, float gravityForce, float speedRotation, Transform playerTransform)
        {
            _engineForce = engineForce;
            _gravityForce = gravityForce;
            _speedRotation = speedRotation;
            _playerTransform = playerTransform;
        }

        public bool IsTouched
        {
            set => _isTouched = value;
        }

        public bool InsidePlanet
        {
            set => _insidePlanet = value;
        }

        public bool OutsideGravity
        {
            set => _outsideGravity = value;
        }

        public void MovementAroundPlanet(float deltaTime, Transform currentPlanet)
        {
            MovementAroundPlanet(deltaTime);
            RotationAroundPlanet(deltaTime, currentPlanet);
        }

        private void MovementAroundPlanet(float deltaTime)
        {
            var shipPositionAxisX = Vector3.zero;
            if (_isTouched && !_outsideGravity)
            {
                shipPositionAxisX.x = -_engineForce;
                _playerTransform.Translate(shipPositionAxisX * deltaTime);
            }
            else if (_insidePlanet && !_outsideGravity)
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

        private void RotationAroundPlanet(float deltaTime, Transform currentPlanet)
        {
            _playerTransform.RotateAround(currentPlanet.position, currentPlanet.up, _speedRotation * deltaTime);
        }
    }
}