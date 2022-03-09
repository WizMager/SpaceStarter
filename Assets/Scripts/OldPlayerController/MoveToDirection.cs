using System.Collections;
using UnityEngine;
using View;

namespace OldPlayerController
{
    public class MoveToDirection
    {
        private readonly float _rotationSpeed;
        private readonly float _moveSpeed;
        private readonly PlayerView _playerView;
        private readonly Transform _playerTransform;

        private bool _isRotated;
        private bool _isActive;
        private Quaternion _lookRotation;
        private Vector3 _lookDirection;

        public MoveToDirection(float rotationSpeed, float moveSpeed, PlayerView playerView)
        {
            _rotationSpeed = rotationSpeed;
            _moveSpeed = moveSpeed;
            _playerView = playerView;
            _playerTransform = playerView.transform;
        }

        public void MovingToPoint(float deltaTime)
        {
            if (!_isActive) return;
            if (!_isRotated) return;

            _playerTransform.Translate(_lookDirection * deltaTime * _moveSpeed, Space.World);
        }

        private IEnumerator Rotate()
        {
            var startRotation = _playerTransform.rotation;
            for (float i = 0; i < _rotationSpeed; i += Time.deltaTime)
            {
                _playerTransform.rotation = Quaternion.Lerp(startRotation, _lookRotation, i / _rotationSpeed);
                yield return null;
            }

            _isRotated = true;
            _playerView.StopCoroutine(Rotate());
        }

        public void SetDirection(Vector3 lookDirection)
        {
            _isActive = true;
            _lookDirection = lookDirection;
            _lookRotation = Quaternion.LookRotation(new Vector3(lookDirection.x, 0, lookDirection.z));
            _playerView.StartCoroutine(Rotate());
        }

        public void Activation(bool isActive)
        {
            _isActive = isActive;
        }
    }
}