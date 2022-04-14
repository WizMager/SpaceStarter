using System.Collections.Generic;
using Model;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class RocketIndicatorView : MonoBehaviour
    {
        private List<Image> _rockets;
        private int _rocketCount;
        private PlayerModel _playerModel;

        private void Awake()
        {
            _rockets = new List<Image>();
            var rockets = GetComponentsInChildren<Image>();
            foreach (var rocket in rockets)
            {
                _rockets.Add(rocket);
            }
        }

        public void TakeModelRef(PlayerModel playerModel, int startRocket)
        {
            _playerModel = playerModel;
            _rocketCount = startRocket > 7 ? 7 : startRocket;
            IndicateRocket();
            _playerModel.OnChangeBonus += RocketChanged;
        }

        private void RocketChanged(int value)
        {
            _rocketCount = value > 7 ? 7 : value;
            IndicateRocket();
        }

        private void IndicateRocket()
        {
            foreach (var rocket in _rockets)
            {
                rocket.enabled = false;
            }
            
            for (int i = 0; i < _rocketCount; i++)
            {
                _rockets[i].enabled = true;
            }
        }
        
        private void OnDestroy()
        {
            _playerModel.OnChangeBonus -= RocketChanged;
        }
    }
}