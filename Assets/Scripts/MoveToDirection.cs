using System.Collections;
using UnityEngine;
using View;

namespace DefaultNamespace
{
    public class MoveToDirection
    {
            private readonly float _rotationSpeed;
            private readonly float _moveSpeed;
            private GravityView _gravityView;
            private readonly Transform _playerTransform;
        
            private bool _isInGravity;
            private bool _isFinished;
            private Quaternion _lookRotation;
            private Vector3 _lookDirection;
        
            public MoveToDirection(float rotationSpeed, float moveSpeed, GravityView gravityView, Transform playerTransform)
            {
                _rotationSpeed = rotationSpeed;
                _moveSpeed = moveSpeed;
                _gravityView = gravityView;
                _playerTransform = playerTransform;

                _gravityView.OnPlayerGravityExit += GravityExited;
                _gravityView.OnPlayerGravityEnter += GravityEntered;
            }

            public void SetDirection(Vector3 lookDirection)
            {
                _isFinished = false;
                _isInGravity = true;
                _lookDirection = lookDirection;
                _lookRotation = Quaternion.LookRotation(new Vector3(lookDirection.x, 0, lookDirection.z));
                _gravityView.StartCoroutine(Move());
            }
            
            private IEnumerator Move()
            {
                if (_isInGravity)
                {
                    _gravityView.StartCoroutine(Rotate());
                    _playerTransform.Translate(_lookDirection * Time.deltaTime * _moveSpeed, Space.World);
                    yield return null;
                }
                else
                {
                    _gravityView.StopCoroutine(Move());
                    _isFinished = true;
                }
            }
            private IEnumerator Rotate()
            {
                var startRotation = _playerTransform.rotation;
                for (float i = 0; i < _rotationSpeed; i += Time.deltaTime)
                {
                    _playerTransform.rotation = Quaternion.Lerp(startRotation, _lookRotation, i / _rotationSpeed);
                    yield return null;
                }
                
                _gravityView.StopCoroutine(Rotate());
            }

            private void GravityExited()
            {
                _isInGravity = false;
            }

            private void GravityEntered()
            {
                _isInGravity = false;
            }

            public bool IsFinished()
            {
                return _isFinished;
            }

            public void ChangePlanet(GravityView currentGravityView)
            {
                OnDestroy();
                _gravityView = currentGravityView;
                _gravityView.OnPlayerGravityExit += GravityExited;
                _gravityView.OnPlayerGravityEnter += GravityEntered;
            }
            
            public void OnDestroy()
            {
                _gravityView.OnPlayerGravityExit -= GravityExited;
            }
    }
}