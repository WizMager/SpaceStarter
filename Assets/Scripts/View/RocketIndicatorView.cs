using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class RocketIndicatorView : MonoBehaviour
    {
        private List<Image> _rockets;
        private int _rocketCount;

        private void Start()
        {
            _rockets = new List<Image>();
            var rockets = GetComponentsInChildren<Image>();
            foreach (var rocket in rockets)
            {
                _rockets.Add(rocket);
            }

            _rocketCount = _rockets.Count;
        }
    }
}