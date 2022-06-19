using ScriptableData;
using UnityEngine;

namespace View
{
    public class TurbineShipView : MonoBehaviour
    {
        [SerializeField] private Transform _planet;
        private float _speedRotation;
        private float _flyAngle;
        private bool _isFlyAroundPlanet;
        private float _currentAngle;

        public void SetValueFields(AllData data)
        {
            _flyAngle = data.Planet.flyAngle;
            _speedRotation = data.Planet.startSpeedRotationAroundPlanet;
        }
        
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
        
        public void Reset()
        {
            _currentAngle = 0;
            _isFlyAroundPlanet = true;
        }
    }
}