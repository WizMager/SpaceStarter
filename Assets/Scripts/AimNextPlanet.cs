using DefaultNamespace;
using Interface;
using UnityEngine;
using Utils;
using View;


public class AimNextPlanet
{
    private readonly IUserInput<Vector3>[] _touches;
    private readonly Transform _playerTransform;
    private readonly BoxCollider _collider;
    private readonly Camera _camera;
    private readonly TrajectoryCalculate _trajectory;

    private bool _isAimEnded;
    private bool _isAim;
    private bool _isActive;
    private Vector3 _previousPosition;

    public AimNextPlanet(IUserInput<Vector3>[] touches, PlayerView playerView, Camera camera, 
        TrajectoryCalculate trajectoryCalculate)
    {
        _touches = touches;
        _playerTransform = playerView.transform;
        _collider = playerView.gameObject.GetComponent<BoxCollider>();
        _camera = camera;
        _trajectory = trajectoryCalculate;
        
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
        if (!_isActive) return;
        
        _trajectory.Calculate(_playerTransform.transform);
        _isAimEnded = false;
        _isAim = true;
    }

    private void TouchHold(Vector3 position)
    {
        if (!_isActive) return;
        if (!_isAim) return;
        
        if (_previousPosition != position)
        {
            _trajectory.Calculate(_playerTransform.transform);
            _previousPosition = position;
        }
        
        var ray = _camera.ScreenPointToRay(position);
        var raycastHit = new RaycastHit[1];
        if (Physics.RaycastNonAlloc(ray, raycastHit, _camera.farClipPlane, GlobalData.LayerForAim) <= 0) return;
        var castPosition = new Vector3(raycastHit[0].point.x, 0, raycastHit[0].point.z);
        var offset = new Vector3(0, 180f, 0);
        _playerTransform.LookAt(castPosition);
        _playerTransform.Rotate(offset);
    }

    private void TouchUp(Vector3 position)
    {
        if (!_isActive) return;
            
        _collider.isTrigger = false;
        _isAim = false;
        _isAimEnded = true;
        _trajectory.ClearLine();
    }

    public void SetActive(bool isActive)
    {
        _isActive = isActive;
        _collider.isTrigger = true;
    }
        
    public void OnDestroy()
    {
        _touches[(int) TouchInput.InputTouchDown].OnChange -= TouchDown;
        _touches[(int) TouchInput.InputTouchHold].OnChange -= TouchHold;
        _touches[(int) TouchInput.InputTouchUp].OnChange -= TouchUp;
    }
}