using System;
using UnityEngine;
using Utils;

namespace View
{
    public class BonusView : MonoBehaviour
    {
        public BonusType bonusType;
        public event Action<GameObject> OnBonusPickUp;
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.GetComponent<ShipView>()) return;
            
            OnBonusPickUp?.Invoke(gameObject);
            Destroy(gameObject);
        }
    }
}