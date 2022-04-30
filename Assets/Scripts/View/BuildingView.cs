using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;

namespace View
{
    public class BuildingView : MonoBehaviour
    {
        public event Action<BonusType> OnFloorTouch;
        
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
        
        private void ShipTouched(string floorName, BonusType bonusType, Vector3 touchedFloorPosition)
        {
            if (_isFirstTouch)
            {
                switch (bonusType)
                {
                    case BonusType.GoodBonus:
                        OnFloorTouch?.Invoke(BonusType.GoodBonus);
                        break;
                    case BonusType.None:
                        OnFloorTouch?.Invoke(BonusType.None);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(bonusType), bonusType, null);
                } 
            }
            
            _isFirstTouch = false;
            
            for (int i = 0; i < _rigidbodies.Count; i++)
            {
                if (_rigidbodies[i].name != floorName) continue;
                var startFloorCounter = i;
                for (int j = i; j < _rigidbodies.Count; j++)
                {
                    _rigidbodies[j].isKinematic = false;
                    _rigidbodies[j].GetComponent<FloorView>().IsActive();
                    // var yUpOffset = _rigidbodies[j].GetComponent<BoxCollider>().bounds.size.y;
                    // var touchedPosition = touchedFloorPosition;
                    // touchedPosition.y = yUpOffset * (j - startFloorCounter + 1);
                    //var impulseDirection = (_rigidbodies[j].position - touchedPosition).normalized;
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
    }
}