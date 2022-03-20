using UnityEngine;

namespace Utils
{
    public class ChelikGravity : MonoBehaviour
    {
        [SerializeField] private float _gravityForce;
        [SerializeField] private Transform _planetCenter;
        [SerializeField] private float _chelikMoveForce;

        private Rigidbody _body;
        private Vector3 _gravityVector;
        private void Start()
        {
            _body = gameObject.GetComponent<Rigidbody>();
        }
    
        private void Update()
        {
            if (_planetCenter == null) return;
            //_body.MovePosition(ForwardDirection(transform.forward) * Time.deltaTime * _chelikMoveForce);
            //_body.AddForce(ForwardDirection(transform.forward) * Time.deltaTime * _chelikMoveForce);
            transform.Translate(ForwardDirection(transform.forward) * Time.deltaTime * _chelikMoveForce);
        }

        private void FixedUpdate()
        {
            if (_planetCenter == null) return;
            _gravityVector = transform.position - _planetCenter.position;
            transform.rotation = Quaternion.FromToRotation(transform.up, _gravityVector) * transform.rotation;
            _body.AddForce(_gravityVector * -_gravityForce * Time.deltaTime);
        }

        private Vector3 ForwardDirection(Vector3 forward)
        {
            return forward - Vector3.Dot(forward, -_gravityVector) * -_gravityVector;
        }

        // private void OnDrawGizmos()
        // {
        //     Gizmos.color = Color.red;
        //     Gizmos.DrawRay(transform.position, ForwardDirection(transform.forward));
        //     Gizmos.color = Color.green;
        //     Gizmos.DrawLine(transform.position, _planetCenter.position);
        // }
    }
}
