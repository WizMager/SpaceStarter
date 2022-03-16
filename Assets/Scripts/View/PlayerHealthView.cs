using Model;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class PlayerHealthView : MonoBehaviour
    {
        private Text _healthText;
        private PlayerModel _model;

        private void Start()
        {
            _healthText = GetComponent<Text>();
        }

        public void SubscribeModel(PlayerModel model)
        {
            _model = model;
            _model.OnChangeHealth += SetText;
        }

        private void SetText(int value)
        {
            _healthText.text = $"Health: {value}";
        }

        private void OnDestroy()
        {
            _model.OnChangeHealth -= SetText;
        }
    }
}