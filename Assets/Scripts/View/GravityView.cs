using System;
using UnityEngine;

namespace View
{
    public class GravityView : MonoBehaviour
    {
        public event Action OnPlayerGravityEnter;
        public event Action OnPlayerGravityExit;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.GetComponent<PlayerView>()) return;
            
            OnPlayerGravityEnter?.Invoke();
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.GetComponent<PlayerView>()) return;
            
            OnPlayerGravityExit?.Invoke();
        }
    }
}