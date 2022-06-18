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

        private void ShipTouched(int floorNumber, FloorType floorType, Vector3 shipPosition, Quaternion shipRotation)
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

                    float impactFactor = 5f - (Math.Abs(floorNumber - i)) * 2f;

                    _rigidbodies[i].isKinematic = false;
                    _rigidbodies[i].GetComponent<FloorView>().IsActive();
                    var rb = _rigidbodies[i];
                    var direction = (rb.position - shipPosition).normalized;
                    var forceDirection = _rigidbodies[i].mass * _forceDestruction;
                    _rigidbodies[i].AddForce(direction * impactFactor * forceDirection, ForceMode.Impulse);
                    _rigidbodies[i].angularVelocity = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f),
                        UnityEngine.Random.Range(-1f, 1f));
                }
                
                _isFirstTouch = false;

                if (floorType == FloorType.GlassFloor)
                {
                    _rigidbodies[floorNumber].gameObject.SetActive(false);
                }

                var explosion = Instantiate(_floorExplosion);
                explosion.transform.position = _rigidbodies[floorNumber].transform.position;
                explosion.transform.rotation = shipRotation;
                Destroy(explosion, 5f);

            }
            else
            {
                float impactFactor = 2f;
                var rb = _rigidbodies[floorNumber];
                var direction = (rb.position - shipPosition).normalized;
                var forceDirection = _rigidbodies[floorNumber].mass * _forceDestruction;
                _rigidbodies[floorNumber].AddForce(direction * impactFactor * forceDirection, ForceMode.Impulse);
                _rigidbodies[floorNumber].angularVelocity = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f),
                            UnityEngine.Random.Range(-1f, 1f));
            }


        }

        private IEnumerable<Rigidbody> SortRigidbody(IEnumerable<Rigidbody> rigidbodies)
        {
            return rigidbodies.OrderBy(o => Vector3.Distance(GlobalData.PlanetCenter, o.position)).ToList();
        }

        private void OnDestroy()
        {
            // TODO add unsubscribe code
            foreach (var rb in _rigidbodies)
            {

                //rb.GetComponentInParent<FloorView>().OnShipTouch += ShipTouched;
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
                _rigidbodies[i].isKinematic = false;

                _rigidbodies[i].AddForce(transform.up * UnityEngine.Random.Range(0f, 2f), ForceMode.Impulse);
                _rigidbodies[i].angularVelocity = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f),
                    UnityEngine.Random.Range(-1f, 1f));
            }
        }

    }
}