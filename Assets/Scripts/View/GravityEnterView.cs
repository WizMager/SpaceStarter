using System;
using UnityEngine;
using Utils;

namespace View
{
    public class GravityEnterView : MonoBehaviour
    {
        public event Action OnPlayerGravityEnter;
        public event Action OnPlayerGravityExit;
        
        public PlanetNumber number;
        
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