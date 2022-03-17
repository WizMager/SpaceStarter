using System;
using UnityEngine;

namespace View
{
    public class AsteroidView : MonoBehaviour
    {
        public event Action OnColliderEnter;
        public event Action OnColliderExit;

        private void OnCollisionEnter(Collision collision)
        {
            if (!collision.gameObject.GetComponent<PlayerView>()) return;
            
            OnColliderEnter?.Invoke();
        }

        private void OnCollisionExit(Collision other)
        {
            if (!other.gameObject.GetComponent<PlayerView>()) return;
            
            OnColliderExit?.Invoke();
        }
    }
}