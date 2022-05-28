using System;
using UnityEngine;
using Utils;

namespace View
{
    public class BonusView : MonoBehaviour
    {
        public FloorType floorType;
        public event Action<GameObject> OnBonusPickUp;
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.GetComponent<ShipView>()) return;
            
            OnBonusPickUp?.Invoke(gameObject);
            Destroy(gameObject);
        }
    }
}