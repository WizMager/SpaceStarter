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
        [SerializeField] private PlanetNumber _planetNumber;
        private Vector3 _centerPlanet;
        private float _lastTimeForRotate;
        private bool _isRotate;
        
        private void Start()
        {
            var centerLastPlanet = CenterPlanet();
            if (centerLastPlanet != Vector3.zero)
            {
                _centerPlanet = CenterPlanet();
            }
            else
            {
                throw new ArgumentException("Wrong Vector3 in when FindCenter in ChelikMove");
            }
            
        }

        private void Update()
        {
            if (_isRotate) return;
            
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
            _isRotate = true;
            var randomAngle = Random.Range(0, _maxAngleToOneRotate);
            var rotationAxis = transform.position - _centerPlanet;
            for (float i = 0; i < randomAngle; i+= Time.deltaTime * _rotationSpeed)
            {
                transform.RotateAround(_centerPlanet, rotationAxis, Time.deltaTime * _rotationSpeed);
                yield return null;
            }

            _lastTimeForRotate = 0;
            _isRotate = false;
            StopCoroutine(Rotate());
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