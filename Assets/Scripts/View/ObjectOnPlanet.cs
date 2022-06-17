using Controllers;
using UnityEngine;
using Utils;

namespace View
{

    public class ObjectOnPlanet : MonoBehaviour
    {
        [SerializeField] private float _rayLength = 1f;
        private Rigidbody _rigidbody;
        private StateController _stateController;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            CheckForImpulse();
        }

        public void GetStateController(StateController stateController)
        {
            _stateController = stateController;
            _stateController.OnStateChange += ChangeState;
        }

        private void ChangeState(GameState gameState)
        {
            if (gameState == GameState.Restart)
            {
                _rigidbody.velocity = Vector3.zero;
                _rigidbody.angularVelocity = Vector3.zero;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Building"))
            {
                //_stateController.OnStateChange -= ChangeState;
                Destroy(gameObject);
            }
        }

        public void AddBlastForce()
        {
            _rigidbody.AddForce(transform.up * Random.Range(0f, 2f), ForceMode.Impulse);
            _rigidbody.angularVelocity = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f),
                Random.Range(-1f, 1f));
        }
        private void CheckForImpulse() 
        { 
            if (Physics.Raycast(transform.position, -transform.up, _rayLength)) return;
            AddBlastForce();
        }
    }
}