using System;
using UnityEngine;

namespace View
{
    public class CameraColliderView : MonoBehaviour
    {
        public event Action OnPlayerEnter;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.GetComponent<PlayerView>()) return;
            
            OnPlayerEnter?.Invoke();
        }
    }
}