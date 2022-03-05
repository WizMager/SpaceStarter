using System;
using UnityEngine;

namespace View
{
    public class PlanetView : MonoBehaviour
    {
        public event Action OnPlayerPlanetEnter;
        public event Action OnPlayerPlanetExit;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.GetComponentInParent<PlayerView>()) return;
            
            OnPlayerPlanetEnter?.Invoke();
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.GetComponentInParent<PlayerView>()) return;
            
            OnPlayerPlanetExit?.Invoke();
        }
    }
}