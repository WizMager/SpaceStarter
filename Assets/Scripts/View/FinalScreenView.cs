using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class FinalScreenView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _finalScore;
        [SerializeField] private TextMeshProUGUI _tryCount;
        [SerializeField] private TextMeshProUGUI _quality;
        [SerializeField] private Button _restart;
        [SerializeField] private Button _nextLevel;

        public Button[] SetValue(int[] values)
        {
            _finalScore.text = values[0].ToString();
            _tryCount.text = values[1].ToString();
            _quality.text = values[2].ToString();
            return new []{_restart, _nextLevel};
        }

        private void OnDisable()
        {
            _restart.onClick.RemoveAllListeners();
            _nextLevel.onClick.RemoveAllListeners();
        }
    }
}