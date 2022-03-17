using System;
using UnityEngine;
using Utils;

namespace View
{
    public class AsteroidView : MonoBehaviour
    {
        public event Action OnColliderEnter;
        public event Action OnColliderExit;
        public NumberAsteroidBelt numberBelt;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.GetComponent<PlayerView>()) return;
            
            OnColliderEnter?.Invoke();
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.gameObject.GetComponent<PlayerView>()) return;
            
            OnColliderExit?.Invoke();
        }
    }
}