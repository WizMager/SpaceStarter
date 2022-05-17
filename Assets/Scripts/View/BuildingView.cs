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
        private bool _onTheGround = true;
        private GameObject _floorExplosion;

        private void Start()
        {
            _floorExplosion = Resources.Load<GameObject>("Particles/FloorExplosion");
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
                    _count = i + 1;
                    for (int j = i; j < _rigidbodies.Count; j++)
					{
                        _rigidbodies[j].isKinematic = false;
				        _rigidbodies[j].GetComponent<FloorView>().IsActive();
				        //var direction = (shipPosition - _rigidbodies[j].position).normalized;
                        var direction = (_rigidbodies[i].position - shipPosition).normalized;
                        var directionFromPlanet = (_rigidbodies[i].position - GlobalData.PlanetCenter).normalized;
                        var forceDirection = _rigidbodies[j].mass * _forceDestruction;
                        //_rigidbodies[j].AddForceAtPosition(-direction * forceDirection, _rigidbodies[j].transform.right,
                        // ForceMode.Impulse);
                        //_rigidbodies[j].angularVelocity = new Vector3(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f),
                        //           UnityEngine.Random.Range(0f, 1f));
                        _rigidbodies[j].AddForce(direction * forceDirection * j / _rigidbodies.Count, ForceMode.Impulse);
                        _rigidbodies[j].AddForce(directionFromPlanet * forceDirection * 5f, ForceMode.Impulse);
                        _rigidbodies[j].angularVelocity = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f),
                                 UnityEngine.Random.Range(-1f, 1f));
                    }
			    }
                
                //_isFirstTouch = false;
            }

            for (int i = _count; i < _rigidbodies.Count; i++)
            {
                 _rigidbodies[i].isKinematic = false;
                 _rigidbodies[i].GetComponent<FloorView>().IsActive();
                // var direction = (shipPosition - _rigidbodies[i].position).normalized;
                var direction = (_rigidbodies[i].position - shipPosition).normalized;
                var forceDirection = _rigidbodies[i].mass * _forceDestruction;
                //_rigidbodies[i].AddForceAtPosition(-direction * forceDirection, _rigidbodies[i].transform.right,
                //    ForceMode.Impulse);
                //_rigidbodies[i].AddForce(direction * forceDirection * 2f, ForceMode.Impulse);
                //_rigidbodies[i].angularVelocity = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f),
                //         UnityEngine.Random.Range(-1f, 1f));
            }

            //var explosion = UnityEngine.Object.Instantiate(_floorExplosion, _rigidbodies[iFloorNumber].transform.gameObject.transform, false);
            var explosion = UnityEngine.Object.Instantiate(_floorExplosion);
            explosion.transform.position = _rigidbodies[iFloorNumber].transform.position;
            //explosion.transform.rotation = shipPosition.rotation;
            _rigidbodies[iFloorNumber].gameObject.active = false;
            GameObject.Destroy(explosion, 50f);
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