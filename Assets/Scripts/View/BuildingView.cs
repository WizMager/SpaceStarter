using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace View
{
    public class BuildingView : MonoBehaviour
    {
        [SerializeField] private Vector3 _centerPlanet = Vector3.zero;
        private List<Rigidbody> _rigidbodies;

        
        private void Start()
        {
            _rigidbodies = new List<Rigidbody>(SortRigidbody(transform.GetComponentsInChildren<Rigidbody>()));

            foreach (var rb in _rigidbodies)
            {
                rb.GetComponent<FloorView>().OnShipTouch += ShipTouched;
            }
            // Test right sorting.
            // foreach (var rb in _rigidbodies)
            // {
            //     Debug.Log(Vector3.Distance(_centerPlanet, rb.position) + "   -   " + rb.name);
            // }
        }

        private void ShipTouched(string floorName)
        {
            for (int i = 0; i < _rigidbodies.Count; i++)
            {
                if (_rigidbodies[i].name != floorName) continue;
                
                for (int j = i; j < _rigidbodies.Count; j++)
                {
                    _rigidbodies[j].isKinematic = false;
                    _rigidbodies[j].AddForce(1f * _rigidbodies[j].transform.forward, ForceMode.Impulse);
                    _rigidbodies[j].angularVelocity = Vector3.up;
                }
                return;
            }
        }

        private List<Rigidbody> SortRigidbody(IEnumerable<Rigidbody> rigidbodies)
        {
            return rigidbodies.OrderBy(o => Vector3.Distance(_centerPlanet, o.position)).ToList();
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