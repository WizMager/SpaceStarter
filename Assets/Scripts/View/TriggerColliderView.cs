using System;
using UnityEngine;

namespace View
{
    public class TriggerColliderView : MonoBehaviour
    {
        public event Action<GameObject> OnObjectTouch;
        
        private void OnTriggerEnter(Collider other)
        {
            OnObjectTouch?.Invoke(other.gameObject);
        }
    }
}