using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;

namespace View
{
    public class BuildingView : MonoBehaviour
    {
        [SerializeField] private float _forceDestruction = 2f;
        public event Action<FloorType> OnFloorTouch;
        
        private List<Rigidbody> _rigidbodies;
        private bool _isFirstTouch = true;
        private bool _onTheGround = true;
        private GameObject _floorExplosion;

        private void Start()
        {
            _floorExplosion = Resources.Load<GameObject>("Particles/FloorExplosion");
            _rigidbodies = new List<Rigidbody>(SortRigidbody(transform.GetComponentsInChildren<Rigidbody>()));
            foreach (var rb in _rigidbodies)
            {
                rb.GetComponentInParent<FloorView>().OnShipTouch += ShipTouched;
            }
        }

        public void Reset()
        {
            _isFirstTouch = true;
        }

        private void ShipTouched(string floorName, FloorType floorType, Vector3 shipPosition, Quaternion shipRotation)
        {
            int iFloorNumber = 0;
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
                    if (_rigidbodies[i].name == floorName)
                    {
                        iFloorNumber = i;
                    }
                    else
                    {
                        continue;
                    }
                    float impactFactor = _rigidbodies.Count;
                    for (int j = i; j < _rigidbodies.Count; j++)
					{
                        _rigidbodies[j].isKinematic = false;
				        _rigidbodies[j].GetComponentInParent<FloorView>().IsActive();
                        var rb = _rigidbodies[j];
                        var direction = (rb.position - shipPosition).normalized;
                        var forceDirection = _rigidbodies[j].mass * _forceDestruction;
                        _rigidbodies[j].AddForce(direction * impactFactor * forceDirection, ForceMode.Impulse);

                        Debug.DrawLine(_rigidbodies[j].position, direction * 100f, Color.red, 1000f);
                        _rigidbodies[j].angularVelocity = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f),
                                 UnityEngine.Random.Range(-1f, 1f));
                        impactFactor = impactFactor / 4;
                    }
                }
                
                _isFirstTouch = false;
                
                var explosion = Instantiate(_floorExplosion);
                explosion.transform.position = _rigidbodies[iFloorNumber].transform.position;
                explosion.transform.rotation = shipRotation;
                Destroy(explosion, 5f);

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
                rb.GetComponentInParent<FloorView>().OnShipTouch += ShipTouched;
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