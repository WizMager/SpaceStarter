using System;
using UnityEngine;

namespace View
{
    public class DeadZoneView : MonoBehaviour
    {
        public event Action OnDeadZoneEnter;

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<PlayerView>())
            {
                OnDeadZoneEnter?.Invoke();
            }
        }
    }
}