using UnityEngine;
using View;

namespace DefaultNamespace
{
    public class FlyNextPlanet
    {
            private readonly float _moveSpeed;
            private GravityView _gravityView;
            private readonly Transform _playerTransform;
        
            private bool _isInGravity;
            private bool _isActive;

            public FlyNextPlanet(float moveSpeed, GravityView gravityView, Transform playerTransform)
            {
                _moveSpeed = moveSpeed;
                _gravityView = gravityView;
                _playerTransform = playerTransform;

                _gravityView.OnPlayerGravityEnter += GravityEntered;
            }

            private void GravityEntered()
            {
                if (!_isActive) return;
                
                _isInGravity = true;
            }

            public bool IsFinished()
            {
                if (_isInGravity)
                {
                    _isInGravity = false;
                    return true;
                }
                
                _playerTransform.Translate(_playerTransform.forward * Time.deltaTime * _moveSpeed, Space.World);
                return false;
            }

            public void SetActive(bool isActive)
            {
                _isActive = isActive;
            }
            
            public void ChangePlanet(GravityView currentGravityView)
            {
                OnDestroy();
                _gravityView = currentGravityView;
                _gravityView.OnPlayerGravityEnter += GravityEntered;
            }
            
            public void OnDestroy()
            {
                _gravityView.OnPlayerGravityEnter -= GravityEntered;
            }
    }
}