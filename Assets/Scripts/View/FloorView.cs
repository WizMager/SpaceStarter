using System;
using System.Collections;
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
        [SerializeField] private Vector3 _centerPlanet = Vector3.zero;
        [SerializeField] private float _timerBeforIsKinimatik;
        [SerializeField] private float _gravidyForce;

        //!!!
        private bool _isActive;
        private Rigidbody _body;
        private Vector3 _direction;
        
        public void IsKinimatikActivated()//!!!
        {
            StartCoroutine(IsKinimatikTimer());
        }

        private IEnumerator IsKinimatikTimer()//!!!
        {
            for (float i = 0; i < _timerBeforIsKinimatik;)
			{
                var time = Time.deltaTime;
                i += time;
                yield return null;
			}

            _isActive = false;
            _body.isKinematic = true;
            StopCoroutine(IsKinimatikTimer());
		}

        public void IsActive( bool isActive)//!!!
        {
            _isActive = isActive;
		}


        private void Start()
        {
            switch (_bonusType)
            {
                case BonusType.GoodBonus:
                    gameObject.GetComponent<MeshRenderer>().material = _goodFloor;
                    break;
                case BonusType.None:
                    gameObject.GetComponent<MeshRenderer>().material = _badFloor;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _body = GetComponent<Rigidbody>();//!!!
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Planet"))
            {
                _body.isKinematic = true;
                _isActive = false;
            }

            if (!other.gameObject.CompareTag("Player")) return;
            
            OnShipTouch?.Invoke(gameObject.name, _bonusType);
            //gameObject.GetComponent<BoxCollider>().enabled = false; //!!!
        }

        private void FixedUpdate()//!!!
        {
            if (!_isActive) return;
            _direction = (_centerPlanet - transform.position).normalized; 
            _body.AddForce(_direction * _gravidyForce, ForceMode.Acceleration);       
        }
    }
}