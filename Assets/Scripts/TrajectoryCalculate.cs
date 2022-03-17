using System;
using UnityEngine;
using View;

namespace DefaultNamespace
{
    public class TrajectoryCalculate : IDisposable
    {
        private readonly AsteroidView[] _asteroidViews;
        private Transform _playerTransform;
        private Vector3 _currentMoveDirection;

        public TrajectoryCalculate(AsteroidView[] asteroidViews, Transform playerTransform)
        {
            _asteroidViews = asteroidViews;
            _playerTransform = playerTransform; 
            SubscribeToAsteroids();
        }

        private void SubscribeToAsteroids()
        {
            foreach (var asteroidView in _asteroidViews)
            {
                //asteroidView.OnColliderEnter += ColliderEntered;
            }
        }

        private void ColliderEntered(Vector3 normal)
        {
            var angle = Vector3.Angle(-_currentMoveDirection, normal);
            _playerTransform.Rotate(_playerTransform.up, -angle * 2, Space.World);
        }
        
        public void Move(float deltaTime)
        {
            _playerTransform.Translate(_playerTransform.forward * Time.deltaTime * 5f, Space.World);
        }

        
        private void UnSubscribeToAsteroids()
        {
            foreach (var asteroidView in _asteroidViews)
            {
                //asteroidView.OnColliderEnter -= ColliderEntered;
            }
        }
        
        public void Dispose()
        {
            UnSubscribeToAsteroids();
        }
    }
}