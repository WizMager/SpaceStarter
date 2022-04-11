using System;
using System.Collections.Generic;
using Interface;
using Model;
using UnityEngine;
using Utils;
using View;

namespace Controllers
{
    public class BonusController : IClean, IController
    {
        private readonly PlayerModel _model;
        private readonly List<BonusView> _bonusViewList;
        private readonly BonusView[] _bonusViews;
        private readonly int[] _valueBonus;
        private readonly StateController _stateController;
        
        public BonusController(PlayerModel playerModel, PlayerIndicatorView indicatorView, 
            BonusView[] bonusViews, int[] valueBonus, StateController stateController)
        {
            _model = playerModel;
            indicatorView.SubscribeModel(_model);
            _bonusViewList = new List<BonusView>();
            _bonusViews = bonusViews;
            _valueBonus = valueBonus;
            _stateController = stateController;
            foreach (var bonusView in bonusViews)
            {
                _bonusViewList.Add(bonusView);
            }

            _stateController.OnStateChange += ChangeState;
            
            Subscribe();
        }

        private void ChangeState(GameState gameState)
        {
            if (gameState != GameState.ArcFlyRadius) return;
            foreach (var bonusView in _bonusViewList)
            {
                bonusView.gameObject.SetActive(false);
            }
        }

        private void Subscribe()
        {
            foreach (var bonusView in _bonusViews)
            {
                if (bonusView != null)
                {
                    bonusView.OnBonusPickUp += BonusPickedUp; 
                }
            }
        }

        private void BonusPickedUp(GameObject bonus)
        {
            var bonusView = bonus.GetComponent<BonusView>();
            switch(bonusView.bonusType)
            {
                case BonusType.GoodBonus:
                    _model.TakeBonus(BonusType.GoodBonus, _valueBonus[0]);
                    break;
                case BonusType.BadBonus:
                    _model.TakeBonus(BonusType.BadBonus, _valueBonus[1]);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(bonusView.bonusType), bonusView.bonusType, null);
            }

            _bonusViewList.Remove(bonusView);
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
            _stateController.OnStateChange -= ChangeState;
            UnSubscribe();
        }
    }
}