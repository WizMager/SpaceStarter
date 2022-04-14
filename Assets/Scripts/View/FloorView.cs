using System;
using UnityEngine;

namespace View
{
    public class FloorView : MonoBehaviour
    {
        public event Action<string> OnShipTouch;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            
            OnShipTouch?.Invoke(gameObject.name);
            gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }
}