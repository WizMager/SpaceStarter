using UnityEngine;
using Utils;

namespace View
{
    public class RestartButtonView : MonoBehaviour
    {
        [SerializeField] private RestartButtonType _restartButtonType;

        public RestartButtonType GetButtonType => _restartButtonType;
    }
}