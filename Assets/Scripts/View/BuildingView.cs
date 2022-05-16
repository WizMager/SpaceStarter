using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;

namespace View
{
    public class BuildingView : MonoBehaviour
    {
        public event Action<FloorType> OnFloorTouch;
        
        private List<Rigidbody> _rigidbodies;
        private bool _isFirstTouch = true;
        private bool _onTheGround = true;

        private void Start()
        {
            _rigidbodies = new List<Rigidbody>(SortRigidbody(transform.GetComponentsInChildren<Rigidbody>()));

            foreach (var rb in _rigidbodies)
            {
                rb.GetComponent<FloorView>().OnShipTouch += ShipTouched;
            }
        }

        public void Reset()
        {
            _isFirstTouch = true;
        }
        
        private void ShipTouched(string floorName, FloorType floorType)
        {
            if (_isFirstTouch)
            {
                switch (floorType)
                {
                    case FloorType.GlassFloor:
                        OnFloorTouch?.Invoke(FloorType.GlassFloor);
                        break;
                    case FloorType.SimpleFloor:
                        OnFloorTouch?.Invoke(FloorType.SimpleFloor);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(floorType), floorType, null);
                } 
            }
            
            _isFirstTouch = false;
            
            for (int i = 0; i < _rigidbodies.Count; i++)
            {
                if (_rigidbodies[i].name != floorName) continue;
                for (int j = i; j < _rigidbodies.Count; j++)
                {
                    _rigidbodies[j].isKinematic = false;
                    _rigidbodies[j].GetComponent<FloorView>().IsActive();
                    _rigidbodies[j].AddForce(_rigidbodies[j].transform.right * 20f, ForceMode.Force);
                    _rigidbodies[j].angularVelocity = Vector3.up;
                }
                return;
            }
        }

        private IEnumerable<Rigidbody> SortRigidbody(IEnumerable<Rigidbody> rigidbodies)
        {
            return rigidbodies.OrderBy(o => Vector3.Distance(GlobalData.PlanetCenter, o.position)).ToList();
        }

        private void OnDestroy()
        {
            foreach (var rb in _rigidbodies)
            {
                rb.GetComponent<FloorView>().OnShipTouch -= ShipTouched;
            }
        }
        private void Update()
        {
            if (!_onTheGround) return;
            var groundRay = new Ray(transform.position, -transform.up);
            var raycastHit = new RaycastHit[1];

            if (Physics.RaycastNonAlloc(groundRay, raycastHit, 1f) >= 1)
            {
                return;
            }

            _onTheGround = false;

            for (int i = 0; i < _rigidbodies.Count; i++)
            {
                if (_rigidbodies[i].isKinematic)
                {
                    continue;
                }
                _rigidbodies[i].isKinematic = false;

                _rigidbodies[i].AddForce(transform.up * UnityEngine.Random.Range(0f, 2f), ForceMode.Impulse);
                _rigidbodies[i].angularVelocity = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f),
                    UnityEngine.Random.Range(-1f, 1f));
                return;
            }
        }

    }
}