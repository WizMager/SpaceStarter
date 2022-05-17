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
                rb.GetComponent<FloorView>().OnShipTouch += ShipTouched;
            }
        }

        public void Reset()
        {
            _isFirstTouch = true;
        }

       //private int _count;

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
                    //_count = i + 1;
                    for (int j = i; j < _rigidbodies.Count; j++)
					{

                        _rigidbodies[j].isKinematic = false;
				        _rigidbodies[j].GetComponent<FloorView>().IsActive();

                        //var direction = (shipPosition - _rigidbodies[j].position).normalized;
                        var rb = _rigidbodies[j];
                        var direction = (rb.position - shipPosition).normalized;
                        //Debug.Log("_rigidbodies[j].position " + _rigidbodies[j].position);
                        //Debug.Log("direction " + direction);
                        //var directionFromPlanet = (_rigidbodies[i].position - GlobalData.PlanetCenter).normalized;
                        var forceDirection = _rigidbodies[j].mass * _forceDestruction;
                        //_rigidbodies[j].AddForceAtPosition(-direction * forceDirection, _rigidbodies[j].transform.right,
                        // ForceMode.Impulse);
                        //_rigidbodies[j].angularVelocity = new Vector3(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f),
                        //           UnityEngine.Random.Range(0f, 1f));

                        _rigidbodies[j].AddForce(direction * impactFactor * forceDirection, ForceMode.Impulse);

                        Debug.DrawLine(_rigidbodies[j].position, direction * 100f, Color.red, 1000f);
                        //_rigidbodies[j].AddForce(directionFromPlanet * forceDirection * 5f, ForceMode.Impulse);
                        //_rigidbodies[j].angularVelocity = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f),
                        //         UnityEngine.Random.Range(-1f, 1f));
                        impactFactor = impactFactor / 4;
                        //_rigidbodies[j].gameObject.SetActive(false);

                        //var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                        //sphere.transform.position = _rigidbodies[j].position;


                    }
                }
                
                _isFirstTouch = false;

                //var explosion = UnityEngine.Object.Instantiate(_floorExplosion, _rigidbodies[iFloorNumber].transform.gameObject.transform, false);
                var explosion = UnityEngine.Object.Instantiate(_floorExplosion);
                explosion.transform.position = _rigidbodies[iFloorNumber].transform.position;
                explosion.transform.rotation = shipRotation;
                _rigidbodies[iFloorNumber].gameObject.SetActive(false);
                GameObject.Destroy(explosion, 5f);

            }

            //for (int i = _count; i < _rigidbodies.Count; i++)
            //{
            //     _rigidbodies[i].isKinematic = false;
            //     _rigidbodies[i].GetComponent<FloorView>().IsActive();
            //    // var direction = (shipPosition - _rigidbodies[i].position).normalized;
            //    var direction = (_rigidbodies[i].position - shipPosition).normalized;
            //    var forceDirection = _rigidbodies[i].mass * _forceDestruction;
            //    //_rigidbodies[i].AddForceAtPosition(-direction * forceDirection, _rigidbodies[i].transform.right,
            //    //    ForceMode.Impulse);
            //    //_rigidbodies[i].AddForce(direction * forceDirection * 2f, ForceMode.Impulse);
            //    //_rigidbodies[i].angularVelocity = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f),
            //    //         UnityEngine.Random.Range(-1f, 1f));
            //}
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