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

            public FlyNextPlanet(float moveSpeed, GravityView gravityView, Transform playerTransform)
            {
                _moveSpeed = moveSpeed;
                _gravityView = gravityView;
                _playerTransform = playerTransform;

                _gravityView.OnPlayerGravityEnter += GravityEntered;
            }

            private void GravityEntered()
            {
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