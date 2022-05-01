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
        private readonly PlayerIndicatorView _playerIndicatorView;
        
        public BonusController(StateController stateController, PlayerModel playerModel, PlayerIndicatorView indicatorView, 
            int[] valueBonus, IEnumerable<BuildingView> buildingViews)
        {
            _model = playerModel;
            _playerIndicatorView = indicatorView;
            _playerIndicatorView.SubscribeModel(_model);
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
            switch (gameState)
            {
                case GameState.RestartAfterWaiting:
                    foreach (var buildingView in _buildingViews)
                    {
                        buildingView.Reset();
                    }
                    _playerIndicatorView.gameObject.SetActive(true);
                    break;
                case GameState.RocketCrushed:
                    _playerIndicatorView.gameObject.SetActive(false);
                    break;
            }
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