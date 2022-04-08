using System;
using UnityEngine;

namespace View
{
    public class HouseView : MonoBehaviour
    {
        public event Action OnHouseColliderEnter;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Chelik"))
            {
                OnHouseColliderEnter?.Invoke();
            }
        }
    }
}