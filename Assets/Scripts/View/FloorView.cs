using System;
using UnityEngine;
using Utils;

namespace View
{
    public class FloorView : MonoBehaviour
    {
        public event Action<string, BonusType> OnShipTouch;
        
        [SerializeField] private BonusType _bonusType;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            
            OnShipTouch?.Invoke(gameObject.name, _bonusType);
            gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }
}