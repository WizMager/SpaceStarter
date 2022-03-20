using System;
using Model;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class PlayerIndicatorView : MonoBehaviour
    {
        private Text _bonusText;
        private Text _healthText;
        private PlayerModel _model;

        private void Start()
        {
            var components = GetComponentsInChildren<Text>();
            foreach (var component in components)
            {
                switch (component.tag)
                {
                    case "HealthIndicator":
                        _healthText = component;
                        break;
                    case "BonusIndicator":
                        _bonusText = component;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("Wrong tag for Text component.");
                }
            }
        }
        
        public void SubscribeModel(PlayerModel playerModel)
        {
            _model = playerModel;
            _model.OnChangeBonus += SetBonusText;
            _model.OnChangeHealth += SetHealthText;
        }
        
        private void SetHealthText(int value)
        {
            _healthText.text = $"Health: {value}";
        }
        
        private void SetBonusText(int value)
        {
            _bonusText.text = $"Bonus: {value}";
        }

        private void OnDestroy()
        {
            _model.OnChangeBonus -= SetBonusText;
            _model.OnChangeHealth -= SetHealthText;
        }
    }
}