using System;
using System.Collections;
using UnityEngine;
using View;

namespace Controller
{
    public class MoveToPoint
    {
        private readonly float _rotationSpeed;
        private readonly float _moveSpeed;
        private readonly PlayerView _playerView;
        private readonly Transform _playerTransform;
        
        private bool _isRotated;
        private Quaternion _lookRotation;
        private Vector3 _lookDirection;

        public MoveToPoint(float rotationSpeed, float moveSpeed, PlayerView playerView)
        {
            _rotationSpeed = rotationSpeed;
            _moveSpeed = moveSpeed;
            _playerView = playerView;
            _playerTransform = playerView.transform;
        }

        public void MovingToPoint(float deltaTime)
        {
            if (!_isRotated) return;
            
            _playerTransform.Translate(_playerTransform.forward * deltaTime * _moveSpeed);
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
            _lookDirection = lookDirection;
            _lookRotation = Quaternion.LookRotation(new Vector3(lookDirection.x, 0, lookDirection.z));
            _playerView.StartCoroutine(Rotate());
        }
    }
}