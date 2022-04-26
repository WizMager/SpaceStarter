using System;
using System.Collections.Generic;
using Model;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace View
{
    public class RocketIndicatorView : MonoBehaviour
    {
        [SerializeField] private RocketPanel _rocketPanel;
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

        private void Start()
        {
            _rockets.Reverse();
            IndicateRocket();
        }

        public void TakeModelRef(PlayerModel playerModel, int startRocket)
        {
            _playerModel = playerModel;
            RocketChanged(startRocket);
            _playerModel.OnChangeRocket += RocketChanged;
        }

        private void RocketChanged(int value)
        {
            switch (_rocketPanel)
            {
                case RocketPanel.FirstDownPanel:
                    _rocketCount = value > 7 ? 7 : value;
                    break;
                case RocketPanel.SecondUpPanel:
                    if (value <= 7)
                    {
                        _rocketCount = 0;
                    }
                    else if (value > 14)
                    {
                        _rocketCount = 7;
                    }
                    else
                    {
                        _rocketCount = value - 7;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            IndicateRocket();
        }

        private void IndicateRocket()
        {
            foreach (var rocket in _rockets)
            {
                rocket.enabled = false;
            }
            
            switch (_rocketPanel)
            {
                case RocketPanel.FirstDownPanel:
                    for (int i = 0; i < _rocketCount; i++)
                    {
                        _rockets[i].enabled = true;
                    }
                    break;
                case RocketPanel.SecondUpPanel:
                    for (int i = 0; i < _rocketCount; i++)
                    {
                        _rockets[i].enabled = true;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"{_rocketPanel}");
            }
        }
        
        private void OnDestroy()
        {
            _playerModel.OnChangeRocket -= RocketChanged;
        }
    }
}