using System;
using Model;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class PlayerIndicatorView : MonoBehaviour
    {
        private Text _scoreText;
        private PlayerModel _model;

        private void Start()
        {
            var components = GetComponentsInChildren<Text>();
            foreach (var component in components)
            {
                switch (component.tag)
                {
                    case "ScoreIndicator":
                        _scoreText = component;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("Wrong tag for Text component.");
                }
            }
        }
        
        public void SubscribeModel(PlayerModel playerModel)
        {
            _model = playerModel;
            _model.OnChangeScore += SetScoreText;
        }

        private void SetScoreText(int value)
        {
            _scoreText.text = $"Score: {value}";
        }

        private void OnDestroy()
        {
            if (_model != null)
            {
                _model.OnChangeScore -= SetScoreText;
            }
        }
    }
}