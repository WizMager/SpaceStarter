using System;
using Controllers;
using UnityEngine;
using Utils;
using View;

namespace StateClasses
{
    public class ArcCameraDown : IDisposable
    {
        public event Action OnFinish;
    
        private readonly StateController _stateController;
        private readonly Transform _playerTransform;
        private readonly PlanetView _planetView;
        private readonly float _stopDistanceFromPlanet;
        private readonly int _percentPath;
        private readonly float _moveSpeed;
        private readonly float _rotationSpeed;

        private bool _isActive;
        private bool _isRotated;
        private float _pathToFly;
        private float _flewPath;
        private float _angleToRotate;
        private float _currentRotateAngle;

        public ArcCameraDown(StateController stateController, Transform playerTransform, PlanetView planetView, 
            float stopDistanceFromPlanet, int percentCameraDownPath, float moveSpeed, float rotationSpeed)
        {
            _stateController = stateController;
            _playerTransform = playerTransform;
            _planetView = planetView;
            _stopDistanceFromPlanet = stopDistanceFromPlanet;
            _percentPath = percentCameraDownPath;
            _moveSpeed = moveSpeed;
            _rotationSpeed = rotationSpeed;

            _stateController.OnStateChange += ChangeState;
        }

        private void ChangeState(GameState gameState)
        {
            if (gameState == GameState.ArcFlyCameraDown)
            {
                _isActive = true;
                SetupAndCalculate();
            }
            else
            {
                _isActive = false;
            }
        }

        private void SetupAndCalculate()
        {
            var planetPosition = _planetView.transform.position;
            var pathToCenterPlanet = Vector3.Distance(_playerTransform.position, planetPosition);
            var colliderRadius = _planetView.GetComponent<SphereCollider>().radius;
            _pathToFly = (pathToCenterPlanet - colliderRadius - _stopDistanceFromPlanet) / 100 * _percentPath;
            var rightDirection = planetPosition - _playerTransform.position;
            _angleToRotate = Vector3.Angle(_playerTransform.forward, rightDirection);
            _isRotated = false;
            _flewPath = 0;
            _currentRotateAngle = 0;
        }
    
        public void Move(float deltaTime)
        {
            if (!_isActive) return;
            if (_isRotated)
            {
                if (_flewPath < _pathToFly)
                { 
                    var distance = _moveSpeed * deltaTime;
                    _playerTransform.Translate(_playerTransform.forward * distance, Space.World);
                    _flewPath += distance;
                }
                else
                {
                    OnFinish?.Invoke();
                }
            }
            else
            {
                if (_currentRotateAngle < _angleToRotate)
                {
                    var rotation = _rotationSpeed * deltaTime;
                    _playerTransform.Rotate(_playerTransform.up, rotation);
                    _currentRotateAngle += rotation;
                }
                else
                {
                    _isRotated = true;
                }
            }
        }

        public void Dispose()
        {
            _stateController.OnStateChange -= ChangeState;
        }
    }
}