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
        private int _startFloor;

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
                    _startFloor = i;
                    var shipPositionForFloor = shipPosition;
                    for (int j = i; j < _rigidbodies.Count; j++)
                    {
                        var floorMultiply = j - _startFloor;
                        var floorSizeY = _rigidbodies[j].GetComponent<BoxCollider>().bounds.size.y;
                        shipPositionForFloor.z = floorMultiply * floorSizeY;
                        var impulseDirection = _rigidbodies[j].position - shipPositionForFloor;
                        Debug.DrawRay(shipPositionForFloor, _rigidbodies[j].position, Color.green, 50f);
                        _rigidbodies[j].isKinematic = false;
                        _rigidbodies[j].GetComponent<FloorView>().IsActive();
                        _rigidbodies[j].AddForce(impulseDirection * 1f, ForceMode.Impulse);
                        //_rigidbodies[j].angularVelocity = Vector3.up;
                        i++;
                    }
                }
                
                _isFirstTouch = false;
            }
            
            
            
            // for (int i = 0; i < _rigidbodies.Count; i++)
            // {
            //     if (_rigidbodies[i].name != floorName) continue;
            //     for (int j = i; j < _rigidbodies.Count; j++)
            //     {
            //         _rigidbodies[j].isKinematic = false;
            //         _rigidbodies[j].GetComponent<FloorView>().IsActive();
            //         _rigidbodies[j].AddForce(_rigidbodies[j].transform.right * 20f, ForceMode.Force);
            //         _rigidbodies[j].angularVelocity = Vector3.up;
            //         i++;
            //     }
            // }
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