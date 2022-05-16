using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;

namespace View
{
    public class BuildingView : MonoBehaviour
    {
        [SerializeField] private float _forceDestruction = 0.5f;
        public event Action<FloorType> OnFloorTouch;
        
        private List<Rigidbody> _rigidbodies;
        private bool _isFirstTouch = true;

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

       private int _count;

        private void ShipTouched(string floorName, FloorType floorType, Vector3 shipPosition)
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

			    for (int i = 0; i < _rigidbodies.Count; i++)
			    {
                    if (_rigidbodies[i].name != floorName) continue;
                    _count = i + 1;
                    for (int j = i; j < _rigidbodies.Count; j++)
					{
                        _rigidbodies[j].isKinematic = false;
				        _rigidbodies[j].GetComponent<FloorView>().IsActive();
				        var direction = (shipPosition - _rigidbodies[j].position).normalized;
				        var forceDirection = _rigidbodies[j].mass * _forceDestruction;
				        _rigidbodies[j].AddForceAtPosition(-direction * forceDirection, _rigidbodies[j].transform.right,
				         ForceMode.Impulse);
				        _rigidbodies[j].angularVelocity = new Vector3(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f),
			                    UnityEngine.Random.Range(0f, 1f));
					}
			    }
                
                _isFirstTouch = false;
            }

            for (int i = _count; i < _rigidbodies.Count; i++)
            {
                 _rigidbodies[i].isKinematic = false;
                 _rigidbodies[i].GetComponent<FloorView>().IsActive();
                 var direction = (shipPosition - _rigidbodies[i].position).normalized;
                 var forceDirection = _rigidbodies[i].mass * _forceDestruction;
                 _rigidbodies[i].AddForceAtPosition(-direction * forceDirection, _rigidbodies[i].transform.right,
                     ForceMode.Impulse);
                 _rigidbodies[i].angularVelocity = new Vector3(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f),
                         UnityEngine.Random.Range(0f, 1f));
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
    }
}