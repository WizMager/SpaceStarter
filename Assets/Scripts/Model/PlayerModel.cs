﻿using System;
using Utils;

namespace Model
{
    public class PlayerModel
    {
        public event Action<int> OnChangeHealth;
        public event Action<int> OnChangeBonus;
        public event Action OnZeroHealth;

        private int _playerHealth;
        private int _playerBonus;

        public PlayerModel(int startPlayerHealth, int startPlayerBonus)
        {
            _playerHealth = startPlayerHealth;
            _playerBonus = startPlayerBonus;
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
        
        public void IndicatorChange(BonusType bonusType, int value)
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