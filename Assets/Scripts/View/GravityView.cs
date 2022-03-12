using System;
using UnityEngine;
using Utils;

namespace View
{
    public class GravityView : MonoBehaviour
    {
        public event Action<Vector3> OnPlayerFirstGravityEnter;
        public event Action OnPlayerGravityEnter;
        public event Action OnPlayerGravityExit;

        public ObjectNumber number;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.GetComponent<PlayerView>()) return;
            
            var contact = other.gameObject.transform.position;
            OnPlayerFirstGravityEnter?.Invoke(contact);
            OnPlayerGravityEnter?.Invoke();
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.GetComponent<PlayerView>()) return;
            
            OnPlayerGravityExit?.Invoke();
        }
    }
}