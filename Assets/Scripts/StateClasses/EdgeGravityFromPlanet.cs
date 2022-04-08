using System;
using System.Collections;
using Controllers;
using UnityEngine;
using Utils;
using View;

namespace StateClasses
{
    public class EdgeGravityFromPlanet : IDisposable
    {
        public event Action OnFinished;
        private readonly float _rotationTime;
        private readonly float _moveSpeed;
        private readonly GravityLittleView _gravityColliderView;
        private readonly Transform _playerTransform;
        private readonly StateController _stateController;
        private readonly Transform _planetTransform;
        
        private bool _isInGravity;
        private bool _isRotated;
        private Quaternion _lookRotation;
        private bool _isActive;

        public EdgeGravityFromPlanet(float rotationTime, float moveSpeed, GravityLittleView gravityColliderView, 
            Transform playerTransform, StateController stateController, Transform planetTransform)
        {
            _rotationTime = rotationTime;
            _moveSpeed = moveSpeed;
            _gravityColliderView = gravityColliderView;
            _playerTransform = playerTransform;
            _stateController = stateController;
            _planetTransform = planetTransform;

            _gravityColliderView.OnPlayerGravityExit += GravityColliderExited;
            _stateController.OnStateChange += ChangeState;
        }

        private void ChangeState(GameState gameState)
        {
            if (gameState == GameState.EdgeGravityFromPlanet)
            {
                _isActive = true;
                _isInGravity = true;
                _isRotated = false;
                var lookDirection = _playerTransform.position - _planetTransform.position;
                _lookRotation = Quaternion.LookRotation(new Vector3(lookDirection.x, 0, lookDirection.z));
                _gravityColliderView.StartCoroutine(Rotate());
            }
            else
            {
                _isActive = false;
            }
        }

        private IEnumerator Rotate()
        {
            var startRotation = _playerTransform.rotation;
            for (float i = 0; i < _rotationTime; i += Time.deltaTime)
            {
                _playerTransform.rotation = Quaternion.Lerp(startRotation, _lookRotation, i / _rotationTime);
                yield return null;
            }
            _isRotated = true;
            _gravityColliderView.StopCoroutine(Rotate());
        }

        private void GravityColliderExited()
        {
            if (!_isActive) return;
            _isInGravity = false;
        }

        public void Move()
        {
            if (!_isActive) return;
            if (!_isInGravity)
            {
                OnFinished?.Invoke();
            }
            if (_isRotated)
            {
                _playerTransform.Translate(_playerTransform.forward * Time.deltaTime * _moveSpeed, Space.World);
            }
        }

        public void Dispose()
        {
            _gravityColliderView.OnPlayerGravityExit -= GravityColliderExited;
            _stateController.OnStateChange -= ChangeState;
        }
    }
}