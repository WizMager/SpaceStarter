using System;
using Controllers;
using UnityEngine;
using Utils;

namespace StateClasses
{
    public class EndFlyAway : IDisposable
    {
        public event Action OnFinish;
    
        private readonly StateController _stateController;
        private readonly Transform _playerTransform;

        private bool _isActive;

        public EndFlyAway(StateController stateController, Transform playerTransform)
        {
            _stateController = stateController;
            _playerTransform = playerTransform;

            _stateController.OnStateChange += ChangeState;
        }

        private void ChangeState(GameState state)
        {
            if (state == GameState.EndFlyAway)
            {
                _isActive = true;
            }
            else
            {
                _isActive = false;
            }
        }
        
        public void DoAction(float deltaTime)
        {
            if (!_isActive) return;
            _playerTransform.gameObject.SetActive(false);
        }

        public void Dispose()
        {
            _stateController.OnStateChange -= ChangeState;
        }
    }
}