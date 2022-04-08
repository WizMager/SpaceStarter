using System;
using Controllers;
using UnityEngine;
using Utils;

namespace StateClasses
{
    public class FlyAway : IDisposable
    {
        public event Action OnFinish;
    
        private readonly StateController _stateController;
        private readonly Transform _playerTransform;
        private readonly Transform _planetTransform;
        private readonly float _distanceFlyAway;
        private readonly float _moveSpeed;
        private readonly float _rotationSpeed;
        private readonly GameObject _gravityView;

        private bool _isActive;
        private bool _isRotated;
        private float _angleToRotate;
        private float _distanceFlew;
        private float _angleRotated;

        public FlyAway(StateController stateController, Transform playerTransform, Transform planetTransform, float distanceFlyAway, 
            float moveSpeed, float rotationSpeed, GameObject gravityView)
        {
            _stateController = stateController;
            _playerTransform = playerTransform;
            _planetTransform = planetTransform;
            _distanceFlyAway = distanceFlyAway;
            _moveSpeed = moveSpeed;
            _rotationSpeed = rotationSpeed;
            _gravityView = gravityView;

            _stateController.OnStateChange += ChangeState;
        }

        private void ChangeState(GameState state)
        {
            if (state == GameState.FlyAway)
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
            _playerTransform.gameObject.SetActive(true);
            _gravityView.SetActive(true);
            _angleToRotate = Vector3.Angle(_playerTransform.forward, _playerTransform.position - _planetTransform.position);
            _isRotated = false;
            _angleRotated = 0;
            _distanceFlew = 0;
        }

        public void Move(float deltaTime)
        {
            if (!_isActive) return;
            if (_isRotated)
            {
                if (_distanceFlew < _distanceFlyAway)
                {
                    var distance = _moveSpeed * deltaTime;
                    _playerTransform.Translate(_playerTransform.forward * distance, Space.World);
                    _distanceFlew += distance;
                }
                else
                {
                    OnFinish?.Invoke();
                }
            }
            else
            {
                if (_angleRotated < _angleToRotate)
                {
                    var rotation = _rotationSpeed * deltaTime;
                    _playerTransform.Rotate(_playerTransform.up, rotation);
                    _angleRotated += rotation;
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