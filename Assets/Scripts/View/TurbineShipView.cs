using ScriptableData;
using UnityEngine;

namespace View
{
    public class TurbineShipView : MonoBehaviour
    {
        private float _speedRotation;
        private Transform _planet;
        private float _flyAngle;
        private bool _isFlyAroundPlanet;
        private float _currentAngle;

        private void Update()
        {
            if (!_isFlyAroundPlanet) return;
            if (_currentAngle >= _flyAngle)
            {
                _isFlyAroundPlanet = false;
                return;
            }
            var stepAngle = Time.deltaTime * _speedRotation;
            transform.RotateAround(_planet.position, _planet.up, stepAngle);
            _currentAngle += stepAngle;
        }

        public void SwitchFlyAroundPlanet(bool isActive)
        {
            _isFlyAroundPlanet = isActive;
        }

        public void SetValueFields(Transform planet, AllData data)
        {
            _planet = planet;
            _flyAngle = data.Planet.flyAngle;
            _speedRotation = data.Planet.startSpeedRotationAroundPlanet;
        }
        
        // public void StartMove(float rotationTime)
        // {
        //     _rotation = Quaternion.LookRotation(-transform.right);
        //     _rotationTime = rotationTime;
        //     StartCoroutine(FlyToConnectPoint());
        // }
        //
        // private IEnumerator FlyToConnectPoint()
        // {
        //     var currentRotation = transform.rotation;
        //     for (float i = 0; i < _rotationTime; )
        //     {
        //         var deltaTime = Time.deltaTime;
        //         i += deltaTime;
        //         transform.rotation = Quaternion.Lerp(currentRotation, _rotation, i / _rotationTime);
        //         yield return null;
        //     }
        //     
        // }
    }
}