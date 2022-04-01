using System;
using UnityEngine;

namespace View
{
    public class HouseView : MonoBehaviour
    {
        public event Action OnHouseColliderEnter;
        private bool _onTheGround = true;

        private void Update()
        {
            if (_onTheGround)
            {
                var groundRay = new Ray(transform.position, -transform.forward);
                var raycastHit = new RaycastHit[1];
                if (Physics.RaycastNonAlloc(groundRay, raycastHit, .3f) < 1)
                {
                    _onTheGround = false;
                    var rb = GetComponent<Rigidbody>();
                    rb.AddForce(transform.forward + new Vector3(UnityEngine.Random.Range(0f, 2f), UnityEngine.Random.Range(0f, 2f), UnityEngine.Random.Range(0f, 2f)), ForceMode.Impulse);
                }
            }

        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Chelik"))
            {
                OnHouseColliderEnter?.Invoke();
            }
        }
    }
}