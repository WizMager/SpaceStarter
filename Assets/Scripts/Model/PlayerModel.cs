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

        public PlayerModel(int startPlayerHealth, int startPlayerRocket)
        {
            _playerHealth = startPlayerHealth;
            _playerRocket = startPlayerRocket;
            _playerScore = 0;
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
    }
}