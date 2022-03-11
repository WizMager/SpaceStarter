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

        public bool Aim()
        {
            if (!_isAimEnded) return false;

            _isAimEnded = false;
            return true;
        }
        
        private void TouchDown(Vector3 position)
        {
            _isAimEnded = false;
            _isAim = true;
        }

        private void TouchHold(Vector3 position)
        {
            if (!_isAim) return;
            
            var ray = _camera.ScreenPointToRay(position);
            var raycastHit = new RaycastHit[1];
            if (Physics.RaycastNonAlloc(ray, raycastHit, _camera.farClipPlane, GlobalData.LayerForAim) <= 0) return;
            var castPosition = new Vector3(raycastHit[0].point.x, 0, raycastHit[0].point.z);
            Debug.DrawLine(_playerTransform.localPosition, castPosition);
            _flyDirection = _playerTransform.localPosition - castPosition;
            var offset = new Vector3(0, 180f, 0);
            _playerTransform.LookAt(castPosition);
            _playerTransform.Rotate(offset);
        }

        private void TouchUp(Vector3 position)
        {
            _isAim = false;
            _isAimEnded = true;
        }

        public void OnDestroy()
        {
            _touches[(int) TouchInput.InputTouchDown].OnChange -= TouchDown;
            _touches[(int) TouchInput.InputTouchHold].OnChange -= TouchHold;
            _touches[(int) TouchInput.InputTouchUp].OnChange -= TouchUp;
        }
    }
}