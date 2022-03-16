using System;
using Interface;
using Model;
using Utils;
using View;

namespace Controller
{
    public class BonusController : IClean, IController
    {
        public event Action<BonusType> OnBonusTake;

        private readonly PlayerModel _model;
        private readonly BonusView[] _bonusViews;
        
        public BonusController(PlayerModel playerModel, PlayerHealthView playerHealthView, PlayerBonusView playerBonusView, 
            BonusView[] bonusViews)
        {
            _model = playerModel;
            _model.SubscribeController(this);
            playerHealthView.SubscribeModel(_model);
            playerBonusView.SubscribeModel(_model);
            _bonusViews = bonusViews;
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
            OnBonusTake?.Invoke(bonusType);
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
            _model.Dispose();
            UnSubscribe();
        }
    }
}