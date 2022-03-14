using System;
using UnityEngine;
using View;

namespace DefaultNamespace
{
    public class LastPlanet : IDisposable
    {
        private readonly Transform _playerTransform;
        private readonly float _moveSpeed;
        private readonly GravityView _gravityView;

        private bool _inGravity;

        public LastPlanet(PlayerView playerView, float moveSpeedLastPlanet, GravityView gravityView)
        {
            _playerTransform = playerView.transform;
            _moveSpeed = moveSpeedLastPlanet;
            _gravityView = gravityView;

            _gravityView.OnPlayerGravityEnter += GravityEntered;
        }

        public bool FlyLastPlanet(float deltaTime)
        {
            if (_inGravity) return true;
            
            PlayerTranslate(deltaTime);
            return false;
        }

        private void PlayerTranslate(float deltaTime)
        {
            _playerTransform.Translate(_playerTransform.forward * deltaTime * _moveSpeed, Space.World);  
        }
        
        private void GravityEntered()
        {
            _inGravity = true;
        }


        public void Dispose()
        {
            _gravityView.OnPlayerGravityEnter -= GravityEntered;
        }
    }
}