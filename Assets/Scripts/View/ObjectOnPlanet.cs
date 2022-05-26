using UnityEngine;

namespace View
{

    public class ObjectOnPlanet : MonoBehaviour
    {
        private Rigidbody _rb;
        private bool _onTheGround = true;
        [SerializeField] private float _rayLength = 1f;

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            CheckForImpulse();
        }

        public void AddBlastForce()
        {
            //var groundRay = new Ray(transform.position, -transform.up);
            //var raycastHit = new RaycastHit[1];

            //if (Physics.RaycastNonAlloc(groundRay, raycastHit, 1f) >= 1)
            //{
            //    return;
            //}
            _rb.AddForce(transform.up * Random.Range(0f, 2f), ForceMode.Impulse);
            _rb.angularVelocity = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f),
                Random.Range(-1f, 1f));
        }
        private void CheckForImpulse() 
        { 
            if (Physics.Raycast(transform.position, -transform.up, _rayLength)) return;
            AddBlastForce();
        }
    }
}