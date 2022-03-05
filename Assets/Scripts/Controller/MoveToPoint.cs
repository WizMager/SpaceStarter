using UnityEngine;

namespace Controller
{
    public class MoveToPoint
    {
        private readonly float _rotationSpeed;
        private readonly float _moveSpeed;
        private readonly Transform _playerTransform;

        private bool _isRotation = true;
        private Vector3 _lookDirection;

        public MoveToPoint(float rotationSpeed, float moveSpeed, Transform playerTransform)
        {
            _rotationSpeed = rotationSpeed;
            _moveSpeed = moveSpeed;
            _playerTransform = playerTransform;
        }

        public void MovingToPoint(float deltaTime)
        {
            if (_isRotation)
            {
                var direction = (_playerTransform.position - _lookDirection).normalized;
                var lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                var rotation = Quaternion.Lerp(_playerTransform.rotation, lookRotation, deltaTime * _rotationSpeed);
                _playerTransform.rotation = rotation;
            }
            else
            {
                
            }
        }

        public void SetDirection(Vector3 lookDirection)
        {
            _lookDirection = lookDirection;
        }
    }
}