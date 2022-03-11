using System.Collections;
using UnityEngine;
using View;

namespace DefaultNamespace
{
    public class FlyToEdgeGravity
    {
            private readonly float _rotationSpeed;
            private readonly float _moveSpeed;
            private GravityView _gravityView;
            private readonly Transform _playerTransform;
        
            private bool _isInGravity;
            private bool _isRotated;
            private Quaternion _lookRotation;

            public FlyToEdgeGravity(float rotationSpeed, float moveSpeed, GravityView gravityView, Transform playerTransform)
            {
                _rotationSpeed = rotationSpeed;
                _moveSpeed = moveSpeed;
                _gravityView = gravityView;
                _playerTransform = playerTransform;

                _gravityView.OnPlayerGravityExit += GravityExited;
            }

            public void SetDirection(Vector3 lookDirection)
            {
                _isInGravity = true;
                _isRotated = false;
                _lookRotation = Quaternion.LookRotation(new Vector3(lookDirection.x, 0, lookDirection.z));
                _gravityView.StartCoroutine(Rotate());
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
                _gravityView.StopCoroutine(Rotate());
            }

            private void GravityExited()
            {
                _isInGravity = false;
            }

            public bool IsFinished()
            {
                if (!_isInGravity)
                {
                    return true;
                }
                
                if (_isRotated)
                {
                    _playerTransform.Translate(_playerTransform.forward * Time.deltaTime * _moveSpeed, Space.World);
                }
                
                return false;
            }

            public void ChangePlanet(GravityView currentGravityView)
            {
                OnDestroy();
                _gravityView = currentGravityView;
                _gravityView.OnPlayerGravityExit += GravityExited;
            }
            
            public void OnDestroy()
            {
                _gravityView.OnPlayerGravityExit -= GravityExited;
            }
    }
}