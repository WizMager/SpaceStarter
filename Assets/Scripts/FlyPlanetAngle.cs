using UnityEngine;

namespace DefaultNamespace
{
    public class FlyPlanetAngle
    {
        private Transform _planet;
        private readonly Transform _player;

        private Vector3 _start;
        private Vector3 _end;
        private float _fullAngle;
        private float _currentAngle;
        
        public FlyPlanetAngle(Transform currentPlanet, Transform player, Transform nextPlanet)
        {
            _planet = currentPlanet;
            _player = player;
            _start = player.position - currentPlanet.position;
            _end = _start;
            _fullAngle = Vector3.Angle(_start, nextPlanet.position - currentPlanet.position) + 360f;
        }
        
        public Vector3 FlewAngle()
        {
            _start = _planet.position - _player.position;
            if (_currentAngle >= _fullAngle)
            {
                var lookDirection = (_player.position - _planet.position).normalized;
                return lookDirection;
            }
            else
            {
                _currentAngle += Vector3.Angle(_start, _end);
                _end = _start;
                return Vector3.zero;
            }
        }

        public void ChangePlanet(Transform currentPlanet, Transform nextPlanet)
        {
            _planet = currentPlanet;
            _start = _player.position - currentPlanet.position;
            _end = _start;
            _fullAngle = Vector3.Angle(_start, nextPlanet.position - currentPlanet.position) + 360f;
        }
    }
}