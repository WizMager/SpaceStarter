using System;
using Interface;
using Model;
using Utils;
using View;

namespace Controllers
{
    public class BonusController : IClean, IController
    {
        private readonly PlayerModel _model;
        private readonly BonusView[] _bonusViews;
        private readonly int[] _valueBonus;
        
        public BonusController(PlayerModel playerModel, PlayerIndicatorView indicatorView, 
            BonusView[] bonusViews, int[] valueBonus)
        {
            _model = playerModel;
            indicatorView.SubscribeModel(_model);
            _bonusViews = bonusViews;
            _valueBonus = valueBonus;
            Subscribe();
        }

        private void Subscribe()
        {
            foreach (var bonusView in _bonusViews)
            {
                bonusView.OnBonusPickUp += BonusPickedUp;
            }
        }

        private void BonusPickedUp(BonusType bonusType)
        {
            switch(bonusType)
            {
                case BonusType.GoodBonus:
                    _model.IndicatorChange(BonusType.GoodBonus, _valueBonus[0]);
                    break;
                case BonusType.BadBonus:
                    _model.IndicatorChange(BonusType.BadBonus, _valueBonus[1]);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(bonusType), bonusType, null);
            }
        }

        private void UnSubscribe()
        {
            foreach (var bonusView in _bonusViews)
            {
                bonusView.OnBonusPickUp -= BonusPickedUp;
            }
        }

        public void Clean()
        {
            UnSubscribe();
        }
    }
}