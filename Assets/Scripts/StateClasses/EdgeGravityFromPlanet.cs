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
        private readonly GravityView _gravityColliderView;
        private readonly Transform _playerTransform;
        private readonly StateController _stateController;
        private readonly Transform _planetTransform;
        
        private bool _isInGravity;
        private Quaternion _finishRotation;
        private bool _isActive;
        private Vector3 _moveDirection;
        private bool _isRotated;

        public EdgeGravityFromPlanet(float rotationTime, float moveSpeed, GravityView gravityColliderView, 
            Transform playerTransform, StateController stateController, Transform planetTransform)
        {
            _rotationTime = rotationTime;
            _moveSpeed = moveSpeed;
            _gravityColliderView = gravityColliderView;
            _playerTransform = playerTransform;
            _stateController = stateController;
            _planetTransform = planetTransform;

            _gravityColliderView.OnShipGravityExit += GravityColliderExited;
            _stateController.OnStateChange += ChangeState;
        }

        private void ChangeState(GameState gameState)
        {
            switch (gameState)
            {
                case GameState.EdgeGravityFromPlanet:
                    _isActive = true;
                    _isInGravity = true;
                    _isRotated = false;
                    var correctPlayerPosition =
                        new Vector3(_playerTransform.position.x, 0, _playerTransform.position.z);
                    _playerTransform.position = correctPlayerPosition;
                    _moveDirection = _playerTransform.position - _planetTransform.position;
                    _finishRotation = Quaternion.LookRotation(new Vector3(_moveDirection.x, 0, _moveDirection.z));
                    _gravityColliderView.StartCoroutine(Rotate());
                    break;
                default:
                    _isActive = false;
                    break;
            }
        }

        private IEnumerator Rotate()
        {
            var startRotation = _playerTransform.rotation;
            for (float i = 0; i < _rotationTime; i += Time.deltaTime)
            {
                _playerTransform.rotation = Quaternion.Lerp(startRotation, _finishRotation, i / _rotationTime);
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
                if (_isRotated)
                {
                    OnFinished?.Invoke();
                }
            }
            else
            {
                _playerTransform.Translate(_moveDirection * Time.deltaTime * _moveSpeed, Space.World);
            }
        }

        public void Dispose()
        {
            _gravityColliderView.OnShipGravityExit -= GravityColliderExited;
            _stateController.OnStateChange -= ChangeState;
        }
    }
}