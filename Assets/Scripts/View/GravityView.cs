using System;
using UnityEngine;

namespace View
{
    public class GravityView : MonoBehaviour
    {
        public event Action<Vector3> OnPlayerFirstGravityEnter;
        public event Action OnPlayerGravityEnter;
        public event Action OnPlayerGravityExit;

        private void OnCollisionEnter(Collision collision)
        {
            if (!collision.gameObject.GetComponentInParent<PlayerView>()) return;
            
            var contact = collision.GetContact(0);
            OnPlayerFirstGravityEnter?.Invoke(contact.point);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.GetComponentInParent<PlayerView>()) return;

            OnPlayerGravityEnter?.Invoke();
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.GetComponentInParent<PlayerView>()) return;
            
            OnPlayerGravityExit?.Invoke();
        }
    }
}