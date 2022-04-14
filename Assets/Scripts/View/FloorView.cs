using System;
using UnityEngine;
using Utils;

namespace View
{
    public class FloorView : MonoBehaviour
    {
        public event Action<string, BonusType> OnShipTouch;
        
        [SerializeField] private BonusType _bonusType;
        [SerializeField] private Material _goodFloor;
        [SerializeField] private Material _badFloor;

        private void Start()
        {
            switch (_bonusType)
            {
                case BonusType.GoodBonus:
                    gameObject.GetComponent<MeshRenderer>().material = _goodFloor;
                    break;
                case BonusType.BadBonus:
                    gameObject.GetComponent<MeshRenderer>().material = _badFloor;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            
            OnShipTouch?.Invoke(gameObject.name, _bonusType);
            gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }
}