using System;
using Model;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class PlayerBonusView : MonoBehaviour
    {
        private Text _bonusText;
        private PlayerModel _model;
        
        private void Start()
        {
            _bonusText = GetComponent<Text>();
        }
        
        public void SubscribeModel(PlayerModel playerModel)
        {
            _model = playerModel;
            _model.OnChangeBonus += SetTextBonus;
        }

        private void SetTextBonus(int value)
        {
            _bonusText.text = $"Bonus: {value}";
        }

        private void OnDestroy()
        {
            _model.OnChangeBonus -= SetTextBonus; 
        }
    }
}