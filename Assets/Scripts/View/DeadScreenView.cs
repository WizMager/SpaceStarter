using System;
using Controller;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class DeadScreenView : MonoBehaviour
    {
        [SerializeField] private Sprite _sprite;
        private Image _image;
        private Text _text;

        private void Start()
        {
            _image = GetComponentInChildren<Image>();
            _text = GetComponentInChildren<Text>();
        }

        public void OnDead()
        {
            _image.sprite = _sprite;
            _image.color = new Color(255, 255, 255, 255);
            _text.text = "Your rocket is crashed!";
        }
    }
}