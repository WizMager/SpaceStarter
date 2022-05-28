using System;
using UnityEngine;
using Utils;

namespace View
{
    public class FloorView : MonoBehaviour
    {
        public event Action<int, FloorType, Vector3, Quaternion> OnShipTouch;

        [SerializeField] private FloorType _floorType;
        [SerializeField] private float _gravityForce;

        private BoxCollider _triggerCollider;
        private bool _isActive;
        private Rigidbody _body;
        private Vector3 _direction;
        private int _floorNumber = -1;

        public void IsActive()
        {
            _isActive = true;
		}

        public void SetFloorNumber(int floorNumber)
        {
            _floorNumber = floorNumber;
        }

        private void Start()
        {
            _body = GetComponent<Rigidbody>();
            var colliders = GetComponentsInChildren<BoxCollider>();
            foreach (var boxCollider in colliders)
            {
                if (!boxCollider.CompareTag("TriggerCollider")) continue;
                _triggerCollider = boxCollider;
                break;
            }

            _triggerCollider.GetComponent<TriggerColliderView>().OnObjectTouch += TriggerEntered;
        }

        private void TriggerEntered(GameObject gameObjectReceive)
        {
            if (gameObjectReceive.CompareTag("Planet"))
            {
                _body.isKinematic = true;
                _isActive = false;
            }

            if (gameObjectReceive.CompareTag("Player"))
            {
                var shipPosition = gameObjectReceive.transform.position;
                var shipRotation = gameObjectReceive.transform.rotation;
                OnShipTouch?.Invoke(_floorNumber, _floorType, shipPosition, shipRotation);
                var colliders = gameObject.GetComponentsInChildren<BoxCollider>();
                foreach (var boxCollider in colliders)
                {
                    if (!boxCollider.CompareTag("TriggerCollider")) continue;
                    boxCollider.enabled = false;
                    break;
                }
            }
            
        }

        private void FixedUpdate()
        {
            if (!_isActive) return;
			_direction = (GlobalData.PlanetCenter - transform.position).normalized;
			_body.AddForce(_direction * _gravityForce, ForceMode.Acceleration);     
		}
	}
}