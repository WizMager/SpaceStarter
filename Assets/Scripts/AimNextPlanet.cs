using UnityEngine;
using Utils;
using View;

namespace DefaultNamespace
{
    public class AimNextPlanet
    {
        private readonly IUserInput<Vector3>[] _touches;
        private readonly Transform _playerTransform;
        private readonly GravityView _gravityView;
        private LineRenderer _lineRenderer;

        private Vector3 _flyDirection;
        private bool _isAimEnded;

        public AimNextPlanet(IUserInput<Vector3>[] touches, Transform playerTransform)
        {
            _touches = touches;
            _playerTransform = playerTransform;


            _touches[(int) TouchInput.InputTouchDown].OnChange += TouchDown;
            _touches[(int) TouchInput.InputTouchHold].OnChange += TouchHold;
            _touches[(int) TouchInput.InputTouchUp].OnChange += TouchUp;
            //_gravityView.OnPlayerGravityEnter += GravityEntered;
        }

        public Vector3 Aim()
        {
            return _isAimEnded ? _flyDirection : Vector3.zero;
        }
        
        private void TouchDown(Vector3 position)
        {
            _isAimEnded = false;
            var clearPosition = new Vector3(position.x, 0, position.z);
            _flyDirection = _playerTransform.position - clearPosition;
        }

        private void TouchHold(Vector3 position)
        {
            var clearPosition = new Vector3(position.x, 0, position.z);
            _flyDirection = _playerTransform.position - clearPosition; 
            _playerTransform.rotation.SetFromToRotation(_playerTransform.forward, _flyDirection);
            // var rotateAngle = Vector3.Angle(_playerTransform.forward, _flyDirection);
            // _playerTransform.Rotate(_playerTransform.up, rotateAngle);
        }

        private void TouchUp(Vector3 position)
        {
            _isAimEnded = true;
            var clearPosition = new Vector3(position.x, 0, position.z);
            _flyDirection = _playerTransform.position - clearPosition;
        }

        private void GravityEntered()
        {
            
        }

        //TODO: show full trajectory with array of lines which impact with colliders
        private void ShowTrajectory()
        {
            
        }
        
        public void OnDestroy()
        {
            _touches[(int) TouchInput.InputTouchDown].OnChange -= TouchDown;
            _touches[(int) TouchInput.InputTouchHold].OnChange -= TouchHold;
            _touches[(int) TouchInput.InputTouchUp].OnChange -= TouchUp;
        }
    }
}