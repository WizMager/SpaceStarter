using System;
using Controllers;
using Utils;

namespace StateClasses
{
    public class NextStateAfterEndShoot : IDisposable
    {
        public event Action OnFinish;

        private readonly StateController _stateController;
        private readonly float _waitBeforeFly;
        
        private bool _isActive;
        private float _currentTimeLeft;
        
        public NextStateAfterEndShoot(StateController stateController, float waitBeforeFlyAway)
        {
            _stateController = stateController;
            _waitBeforeFly = waitBeforeFlyAway;

            _stateController.OnStateChange += ChangeState;
        }

        private void ChangeState(GameState gameState)
        {
            _isActive = gameState == GameState.NextStateAfterEndShoot;
        }

        public void Move(float deltaTime)
        {
            if (!_isActive) return;
            if (_currentTimeLeft < _waitBeforeFly)
            {
                _currentTimeLeft += deltaTime;
            }
            else
            {
                _currentTimeLeft = 0;
                OnFinish?.Invoke();
            }
        }

        public void Dispose()
        {
            _stateController.OnStateChange -= ChangeState;
        }
    }
}