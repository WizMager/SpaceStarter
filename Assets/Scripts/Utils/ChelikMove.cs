using System;
using System.Collections;
using UnityEngine;
using View;
using Random = UnityEngine.Random;

namespace Utils
{
    public class ChelikMove : MonoBehaviour
    {
        [SerializeField] private float _cooldownRotate;
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private float _moveSpeed;
        [Range(1, 359)] [SerializeField] private float _maxAngleToOneRotate;
        [SerializeField] private float _intoSpaceTime;
        [SerializeField] private float _speedIntoSpace;
        [SerializeField] private float _rotateIntoSpace;
        [SerializeField] private PlanetNumber _planetNumber;
        [SerializeField] private GameObject _rayCastPoint;
        [SerializeField] private GameObject _body;
        [SerializeField] private GameObject _upPoint;
        
        private Vector3 _centerPlanet;
        private float _lastTimeForRotate;
        private bool _isActive = true;
        private bool _onGround = true;
        private BoxCollider _collider;
        
        private void Start()
        {
            _collider = GetComponent<BoxCollider>();
            var centerLastPlanet = CenterPlanet();
            if (centerLastPlanet != Vector3.zero)
            {
                _centerPlanet = CenterPlanet();
            }
            else
            {
                throw new ArgumentException($"Wrong Vector3 in when FindCenter in {typeof(ChelikMove)}");
            }
        }

        private void Update()
        {
            // if (!_isActive) return;
            //
            // var deltaTime = Time.deltaTime;
            // transform.RotateAround(_centerPlanet, transform.right, deltaTime * _moveSpeed);
            //
            // if (_lastTimeForRotate < _cooldownRotate)
            // {
            //     _lastTimeForRotate += deltaTime;
            // }
            // else
            // {
            //     StartCoroutine(Rotate());
            // }
        }

        private void OnTriggerStay(Collider other)
        {
            if (!_isActive) return;

            var deltaTime = Time.deltaTime;
            transform.RotateAround(_centerPlanet, transform.right, deltaTime * _moveSpeed);
            
            if (_lastTimeForRotate < _cooldownRotate)
            {
                _lastTimeForRotate += deltaTime;
            }
            else
            {
                StartCoroutine(Rotate());
            }
        }

        private IEnumerator Rotate()
        {
            _isActive = false;
            bool forwardEmptySpace;
            var randomAngle = Random.Range(0, _maxAngleToOneRotate);
            var rotationAxis = transform.position - _centerPlanet;
            var leftOrRight = Random.Range(0, 2);
            for (float i = 0; i < randomAngle; i+= Time.deltaTime * _rotationSpeed)
            {
                float minusModifier;
                if (leftOrRight == 0)
                {
                    minusModifier = -1;
                }
                else
                {
                    minusModifier = 1;
                }
                transform.RotateAround(_centerPlanet, rotationAxis, Time.deltaTime * _rotationSpeed * minusModifier);
                yield return null;
            }

            do
            {
                var ray = new Ray(_rayCastPoint.transform.position, _rayCastPoint.transform.forward);
                var rayCastHit = new RaycastHit[1];
                Physics.RaycastNonAlloc(ray, rayCastHit, 0.2f);
                if (rayCastHit[0].collider == null)
                {
                    forwardEmptySpace = true;
                    StartCoroutine(Rotate());
                    yield return null;
                }
                else
                {
                    forwardEmptySpace = false;
                    _lastTimeForRotate = 0;
                    _isActive = true;
                    StopAllCoroutines();
                }
            } while (forwardEmptySpace);
        }

        public void DeactivateChelikMove()
        {
            StopAllCoroutines();
            _isActive = false;
            _collider.enabled = false;
            StartCoroutine(FlyIntoSpace());
        }

        private IEnumerator FlyIntoSpace()
        {
            var flyDirection = _upPoint.transform.position - transform.position;
            for (float i = 0; i < _intoSpaceTime; i+= Time.deltaTime)
            {
                transform.Translate(flyDirection * _speedIntoSpace * Time.deltaTime, Space.World);
                _body.transform.RotateAround(transform.position, transform.right, _rotateIntoSpace * Time.deltaTime);
                yield return null;
            }
            Destroy(gameObject);
        }
        
        private Vector3 CenterPlanet()
        {
            var planetViews = FindObjectsOfType<PlanetView>();
            foreach (var planetView in planetViews)
            {
                if (planetView.number == _planetNumber)
                {
                    return planetView.gameObject.transform.position;
                }
            }
            return Vector3.zero;
        }
    }
}