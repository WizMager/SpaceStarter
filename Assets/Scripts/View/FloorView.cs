using System;
using UnityEngine;
using Utils;

namespace View
{
    public class FloorView : MonoBehaviour
    {
        public event Action<string, FloorType, Vector3, Quaternion> OnShipTouch;

        [SerializeField] private FloorType _floorType;
        [SerializeField] private float _gravityForce;
        
        private bool _isActive;
        private Rigidbody _body;
        private Vector3 _direction;

        public void IsActive()
        {
            _isActive = true;
		}

        private void Start()
        {
            _body = GetComponent<Rigidbody>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Planet"))
            {
                _body.isKinematic = true;
                _isActive = false;
            }

            if (!other.gameObject.CompareTag("Player")) return;
            var shipPosition = other.transform.position;
            var shipRotation = other.transform.rotation;
            //Debug.Log("gameObject " + gameObject.transform.position);
            //Debug.DrawLine(shipPosition, gameObject.transform.position - shipPosition, Color.red, 1000f);
            OnShipTouch?.Invoke(gameObject.name, _floorType, shipPosition, shipRotation);
            gameObject.GetComponent<BoxCollider>().enabled = false;
        }

        private void FixedUpdate()
        {
            if (!_isActive) return;
			_direction = (GlobalData.PlanetCenter - transform.position).normalized;
			_body.AddForce(_direction * _gravityForce, ForceMode.Acceleration);     
		}
	}
}