using System;
using Controllers;
using UnityEngine;
using Utils;
using View;

namespace StateClasses
{
    public class EdgeGravityToPlanet : IDisposable
    {
        public event Action OnFinish;
    
        private readonly Transform _playerTransform;
        private readonly GravityView _gravityView;
        private readonly StateController _stateController;
        private readonly float _moveSpeed;

        private bool _isActive = true;
     
        public EdgeGravityToPlanet(Transform playerTransform, GravityView gravityView, StateController stateController, float moveSpeed)
        {
            _playerTransform = playerTransform;
            _gravityView = gravityView;
            _stateController = stateController;
            _moveSpeed = moveSpeed;

            _gravityView.OnPlayerGravityEnter += OnGravityEnter;
            _stateController.OnStateChange += ChangeState;
        }

        private void ChangeState(GameState gameState)
        {
            _isActive = gameState == GameState.EdgeGravityToPlanet;
        }

        private void OnGravityEnter()
        {
            if (!_isActive) return;
            OnFinish?.Invoke();
        }

        public void Move(float deltaTime)
        {
            if (!_isActive) return;
            _playerTransform.Translate(_playerTransform.forward * deltaTime * _moveSpeed, Space.World); 
        }

        public void Dispose()
        {
            _gravityView.OnPlayerGravityEnter -= OnGravityEnter;
            _stateController.OnStateChange -= ChangeState;
        }
    }
}