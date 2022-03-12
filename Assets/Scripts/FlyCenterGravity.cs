using System.Collections;
using UnityEngine;
using View;

namespace DefaultNamespace
{
    public class FlyCenterGravity
    {
        private readonly PlayerView _playerView;
        private readonly float _rotationSpeed;
        private readonly float _moveSpeed;
        
        private readonly Transform _playerTransform;
        private Vector3 _direction;
        private float _pathCenter;
        private bool _isRotated;
        
        public FlyCenterGravity(PlayerView playerView, float rotationSpeed, float moveSpeed, Transform currentPlanet)
        {
            _playerView = playerView;
            _rotationSpeed = rotationSpeed;
            _moveSpeed = moveSpeed;

            _playerTransform = _playerView.transform;
            _direction = (currentPlanet.position - _playerTransform.position).normalized;
            _pathCenter = Vector3.Distance(_playerTransform.position, currentPlanet.position) / 2;
        }

        private IEnumerator Rotate()
        {
            for (float i = 0; i < _rotationSpeed; i += Time.deltaTime)
            {
                _playerTransform.transform.right = _direction * i / _rotationSpeed;
                yield return null;
            }

            _isRotated = true;
            _playerView.StopCoroutine(Rotate());
        }

        private IEnumerator Move()
        {
            for (float i = 0; i < _pathCenter; i += Time.deltaTime * _moveSpeed)
            {
                _playerTransform.Translate(_direction * i / _pathCenter);
                yield return null;
            }
        }
    }
}