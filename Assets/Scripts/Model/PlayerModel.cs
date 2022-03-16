using System;
using Controller;
using Utils;

namespace Model
{
    public class PlayerModel : IDisposable
    {
        public event Action<int> OnChangeHealth;
        public event Action<int> OnChangeBonus;
        public event Action OnZeroHealth;

        private int _playerHealth;
        private int _playerBonus;
        private readonly int[] _valueBonus;
        private BonusController _bonusController;

        public PlayerModel(int startPlayerHealth, int startPlayerBonus, int[] valueBonus)
        {
            _playerHealth = startPlayerHealth;
            _playerBonus = startPlayerBonus;
            _valueBonus = valueBonus;
        }

        public void SubscribeController(BonusController bonusController)
        {
            _bonusController = bonusController;
            _bonusController.OnBonusTake += BonusTaked;
        }

        private void BonusTaked(BonusType bonusType)
        {
            switch(bonusType)
            {
                case BonusType.GoodBonus:
                    _playerBonus += _valueBonus[(int) BonusType.GoodBonus];
                    OnChangeBonus?.Invoke(_playerBonus);
                    break;
                case BonusType.BadBonus:
                    _playerHealth -= _valueBonus[(int) BonusType.BadBonus];
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
        
        public void Dispose()
        {
            _bonusController.OnBonusTake -= BonusTaked;
        }
    }
}