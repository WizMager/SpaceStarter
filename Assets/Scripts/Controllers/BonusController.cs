using System;
using System.Collections.Generic;
using Interface;
using Model;
using Utils;
using View;

namespace Controllers
{
    public class BonusController : IClean, IController
    {
        private readonly PlayerModel _model;
        private readonly int[] _valueBonus;
        private readonly StateController _stateController;
        private readonly List<BuildingView> _buildingViews;
        
        public BonusController(StateController stateController, PlayerModel playerModel, PlayerIndicatorView indicatorView, 
            int[] valueBonus, IEnumerable<BuildingView> buildingViews)
        {
            _model = playerModel;
            indicatorView.SubscribeModel(_model);
            _valueBonus = valueBonus;
            _stateController = stateController;
            _buildingViews = new List<BuildingView>();
            foreach (var buildingView in buildingViews)
            {
                _buildingViews.Add(buildingView);
            }
            _stateController.OnStateChange += ChangeState;
            
            Subscribe();
        }

        private void ChangeState(GameState gameState)
        {
            
        }

        private void Subscribe()
        {
            foreach (var buildingView in _buildingViews)
            {
                buildingView.OnFloorTouch += FloorTouched;
            }
        }

        private void FloorTouched(BonusType type)
        {
            switch (type)
            {
                case BonusType.GoodBonus:
                    _model.TakeBonus(BonusType.GoodBonus, _valueBonus[0]);
                    _model.TakeBonus(BonusType.None, _valueBonus[1]);
                    break;
                case BonusType.None:
                    _model.TakeBonus(BonusType.None, _valueBonus[1]);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        private void UnSubscribe()
        {
            foreach (var buildingView in _buildingViews)
            {
                buildingView.OnFloorTouch -= FloorTouched;
            }
        }

        public void Clean()
        {
            _stateController.OnStateChange -= ChangeState;
            UnSubscribe();
        }
    }
}