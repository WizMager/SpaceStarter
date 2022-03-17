using System;
using UnityEngine;
using View;

namespace DefaultNamespace
{
    public class TrajectoryCalculate
    {
        private AsteroidView[] _asteroidViews;
        private readonly Transform _playerTransform;
        private readonly float _moveSpeed;
        
        private bool _isAngleCalculated;
        private bool _colliderEntered;
        private Vector3 _reflectVector;

        public TrajectoryCalculate(AsteroidView[] asteroidViews, Transform playerTransform, float moveSpeed)
        {
            _asteroidViews = asteroidViews;
            _playerTransform = playerTransform;
            _moveSpeed = moveSpeed;
            
            SubscribeToAsteroids();
        }

        private void SubscribeToAsteroids()
        {
            foreach (var asteroidView in _asteroidViews)
            {
                asteroidView.OnColliderEnter += ColliderEntered;
                asteroidView.OnColliderExit += ColliderExited;
            }
        }

        private void ColliderEntered()
        {
            if (_colliderEntered) return;
        
            _playerTransform.LookAt(_reflectVector);
            _colliderEntered = true;
        }
        
        private void ColliderExited()
        {
            _colliderEntered = false;
            _isAngleCalculated = false;
        }
        
        public void Move()
        {
            _playerTransform.Translate(_playerTransform.forward, Space.World);

            if (_isAngleCalculated) return;
        
            var ray = new Ray(_playerTransform.position, _playerTransform.forward);
            var raycastHit = new RaycastHit[1];
            if (Physics.RaycastNonAlloc(ray, raycastHit) <= 0) return;
        
            var currentDirection = _playerTransform.forward.normalized;
            var normal = raycastHit[0].normal;
            _reflectVector = raycastHit[0].point + Vector3.Reflect(currentDirection, normal);
            _isAngleCalculated = true;
        }
        
        public void Move(float deltaTime)
        {
            _playerTransform.Translate(_playerTransform.forward * Time.deltaTime * _moveSpeed, Space.World);

            if (_isAngleCalculated) return;
        
            var ray = new Ray(_playerTransform.position, _playerTransform.forward);
            var raycastHit = new RaycastHit[1];
            if (Physics.RaycastNonAlloc(ray, raycastHit) <= 0) return;
        
            var currentDirection = _playerTransform.forward.normalized;
            var normal = raycastHit[0].normal;
            _reflectVector = raycastHit[0].point + Vector3.Reflect(currentDirection, normal);
            _isAngleCalculated = true;
        }

        private void UnSubscribeToAsteroids()
        {
            foreach (var asteroidView in _asteroidViews)
            {
                asteroidView.OnColliderEnter -= ColliderEntered;
                asteroidView.OnColliderExit += ColliderExited;
            }
        }

        public void ChangePlanet(AsteroidView[] asteroidViews)
        {
            UnSubscribeToAsteroids();
            _asteroidViews = asteroidViews;
            SubscribeToAsteroids();
        }
        
    }
}