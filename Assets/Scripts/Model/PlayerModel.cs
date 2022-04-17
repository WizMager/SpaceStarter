using System;
using UnityEngine;
using Utils;

namespace Model
{
    public class PlayerModel
    {
        public event Action<int> OnChangeScore;
        public event Action<int> OnChangeRocket;
        public event Action OnZeroHealth;
        public event Action OnZeroRocketLeft;

        private int _playerHealth;
        private int _playerRocket;
        private int _playerScore;
        private int _playerQuality;
        private int _playerTryCount;
        private int _scoreMultiply;

        public PlayerModel(int startPlayerHealth, int startPlayerRocket)
        {
            _playerHealth = startPlayerHealth;
            _playerRocket = startPlayerRocket;
            _playerScore = 0;
            _playerTryCount = 1;
            _scoreMultiply = 1;
        }

        public void ShootRocket()
        {
            _playerRocket--;
            OnChangeRocket?.Invoke(_playerRocket);
            if (_playerRocket <= 0)
            {
                OnZeroRocketLeft?.Invoke();
            }
        }
        
        public void TakeDamage(int damage)
        {
            _playerHealth -= damage;
            if (_playerHealth >= 0)
            {
                Debug.Log($"Current health: {_playerHealth}");
            }
            else
            {
                Debug.Log($"Current health: {_playerHealth}");
                OnZeroHealth?.Invoke();
            }
        }
        
        public void TakeBonus(BonusType bonusType, int value)
        {
            switch (bonusType)
            {
                case BonusType.GoodBonus:
                    _playerRocket += value;
                    OnChangeRocket?.Invoke(_playerRocket);
                    break;
                case BonusType.None:
                    _playerScore += value;
                    OnChangeScore?.Invoke(_playerScore);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(bonusType), bonusType, null);
            }
        }

        public void SetQuality(float wholeTime, float touchTime)
        {
            _playerQuality = 100 - Mathf.RoundToInt(touchTime * 100 / wholeTime);
        }

        public void SetScoreMultiply()
        {
            _scoreMultiply = _playerRocket;
        }
        
        public int[] GetValueToFinalScreen()
        {
            _playerQuality = _playerHealth;
            return new[] {_playerScore * _scoreMultiply, _playerTryCount, _playerQuality};
        }
    }
}