using System;
using UnityEngine;
using Utils;

namespace Model
{
    public class PlayerModel
    {
        public event Action<int> OnChangeHealth;
        public event Action<int> OnChangeBonus;
        public event Action OnZeroHealth;
        public event Action OnZeroBonusLeft;

        private int _playerHealth;
        private int _playerBonus;

        public PlayerModel(int startPlayerHealth, int startPlayerBonus)
        {
            _playerHealth = startPlayerHealth;
            _playerBonus = startPlayerBonus;
        }

        public void ShootRocket()
        {
            _playerBonus--;
            OnChangeBonus?.Invoke(_playerBonus);
            if (_playerBonus <= 0)
            {
                OnZeroBonusLeft?.Invoke();
            }
        }
        
        public void TakeDamage(int damage)
        {
            _playerHealth -= damage;
            if (_playerHealth >= 0)
            {
                OnChangeHealth?.Invoke(_playerHealth);
            }
            else
            {
                OnZeroHealth?.Invoke();
            }
        }
        
        public void TakeBonus(BonusType bonusType, int value)
        {
            switch (bonusType)
            {
                case BonusType.GoodBonus:
                    _playerBonus += value;
                    OnChangeBonus?.Invoke(_playerBonus);
                    break;
                case BonusType.BadBonus:
                    _playerHealth -= value;
                    if (_playerHealth > 0)
                    {
                        OnChangeHealth?.Invoke(_playerHealth);
                    }
                    else
                    {
                        OnChangeHealth?.Invoke(_playerHealth);
                        OnZeroHealth?.Invoke();
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(bonusType), bonusType, null);
            }
        }
    }
}