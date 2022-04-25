using System;
using UnityEngine;

namespace View
{
    public class GravityView : MonoBehaviour
    {
        public event Action OnPlayerGravityEnter;
        public event Action OnShipGravityExit;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            OnPlayerGravityEnter?.Invoke();
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            OnShipGravityExit?.Invoke();
        }
    }
}