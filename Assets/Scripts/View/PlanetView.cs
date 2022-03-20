using System;
using UnityEngine;
using Utils;

namespace View
{
    public class PlanetView : MonoBehaviour
    {
        public event Action OnPlayerPlanetEnter;
        public event Action OnPlayerPlanetExit;
        public PlanetNumber number;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.GetComponent<PlayerView>()) return;
            
            OnPlayerPlanetEnter?.Invoke();
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.GetComponent<PlayerView>()) return;
            
            OnPlayerPlanetExit?.Invoke();
        }
    }
}