using System;
using UnityEngine;

namespace View
{
    public class GravityLittleView : MonoBehaviour
    {
        public event Action OnPlayerGravityEnter;
        public event Action OnPlayerGravityExit;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.GetComponent<ShipView>()) return;
            
            OnPlayerGravityEnter?.Invoke();
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (!other.GetComponent<ShipView>()) return;
            
            OnPlayerGravityExit?.Invoke();
        }
    }
}