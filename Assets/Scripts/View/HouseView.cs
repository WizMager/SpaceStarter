using System;
using UnityEngine;

namespace View
{
    public class HouseView : MonoBehaviour
    {
        private Rigidbody _rb;
        private bool _onTheGround = true;

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if (!_onTheGround) return;
            var groundRay = new Ray(transform.position, -transform.up);
            var raycastHit = new RaycastHit[1];
            
            if (Physics.RaycastNonAlloc(groundRay, raycastHit, .3f) >= 1) return;
            _onTheGround = false;
            _rb.AddForce(transform.up * UnityEngine.Random.Range(0f, 2f), ForceMode.Impulse);
            _rb.angularVelocity = new Vector3(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f),
                UnityEngine.Random.Range(0f, 1f));
        }
        
    }
}