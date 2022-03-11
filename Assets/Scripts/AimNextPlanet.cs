using UnityEngine;
using UnityEngine.UIElements;
using Utils;
using View;

namespace DefaultNamespace
{
    public class AimNextPlanet
    {
        private readonly IUserInput<Vector3>[] _touches;
        private readonly Transform _playerTransform;
        private readonly Camera _camera;

        private Vector3 _flyDirection;
        private bool _isAimEnded;
        private bool _isAim;

        private Vector3 _startMousePosition;
        private Quaternion _rotate;

        public AimNextPlanet(IUserInput<Vector3>[] touches, Transform playerTransform, Camera camera)
        {
            _touches = touches;
            _playerTransform = playerTransform;
            _camera = camera;


            _touches[(int) TouchInput.InputTouchDown].OnChange += TouchDown;
            _touches[(int) TouchInput.InputTouchHold].OnChange += TouchHold;
            _touches[(int) TouchInput.InputTouchUp].OnChange += TouchUp;
        }

        public Vector3 Aim(float deltaTime)
        {
            if (!_isAimEnded) return Vector3.zero;
            
            _playerTransform.rotation.SetLookRotation(_flyDirection);
            return _flyDirection;
        }
        
        private void TouchDown(Vector3 position)
        {
            // var clearPosition = new Vector3(position.x, 0, position.z);
            // _flyDirection = _playerTransform.position + clearPosition;
            _isAimEnded = false;
            _isAim = true;
        }

        private void TouchHold(Vector3 position)
        {
            if (!_isAim) return;
            
            var ray = _camera.ScreenPointToRay(position);
            var raycastHit = new RaycastHit[1];
            if (Physics.RaycastNonAlloc(ray, raycastHit, _camera.farClipPlane, GlobalData.LayerForAim) > 0)
            {
                Debug.Log(raycastHit[0].point);
                //_flyDirection = clearPosition - _playerTransform.localPosition;
            }
        }

        private void TouchUp(Vector3 position)
        {
            var clearPosition = new Vector3(position.x, 0, position.z);
            _flyDirection = _playerTransform.position + clearPosition;
            _isAim = false;
            //_isAimEnded = true;
        }

        public void OnDestroy()
        {
            _touches[(int) TouchInput.InputTouchDown].OnChange -= TouchDown;
            _touches[(int) TouchInput.InputTouchHold].OnChange -= TouchHold;
            _touches[(int) TouchInput.InputTouchUp].OnChange -= TouchUp;
        }
    }
}