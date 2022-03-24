using Interface;
using UnityEngine;
using Utils;

public class TapExplosionController
{
    private readonly IUserInput<Vector3>[] _touch;
    private readonly Camera _camera;
    private readonly float _explosionArea;
    private readonly float _explosionForce;
    private readonly GameObject _particle;

    private bool _isActive;
    private Vector3 _touchDownPosition;

    public TapExplosionController(IUserInput<Vector3>[] touch, Camera camera, float explosionArea, float explosionForce, GameObject particle)
    {
        _touch = touch;
        _camera = camera;
        _explosionArea = explosionArea;
        _explosionForce = explosionForce;
        _particle = particle;

        _touch[(int) TouchInput.InputTouchDown].OnChange += TouchDown;
        _touch[(int) TouchInput.InputTouchUp].OnChange += TouchUp;
    }

    private void Shoot(Vector3 touchPosition)
    {
        var ray = _camera.ScreenPointToRay(touchPosition);
        var raycastHit = new RaycastHit[1];
        Physics.RaycastNonAlloc(ray, raycastHit, _camera.farClipPlane, GlobalData.LayerForAim);
        Object.Instantiate(_particle, raycastHit[0].point, Quaternion.identity);
        var hitsSphereCast = Physics.SphereCastAll(ray.origin, _explosionArea, ray.direction, _camera.farClipPlane, GlobalData.LayerForAim);
        foreach (var hitSphereCast in hitsSphereCast)
        {
            if (hitSphereCast.rigidbody.isKinematic)
            {
                hitSphereCast.rigidbody.isKinematic = false;
            }
            hitSphereCast.rigidbody.AddForce(hitSphereCast.normal * _explosionForce, ForceMode.Impulse);
        }
    }

    private void TouchDown(Vector3 position)
    {
        if (!_isActive) return;

        _touchDownPosition = position;
    }

    private void TouchUp(Vector3 position)
    {
        if (!_isActive) return;

        if (_touchDownPosition != position) return;
        
        Shoot(position);
    }

    public void SetActive()
    {
        _isActive = true;
    }

    public void OnDestroy()
    {
        _touch[(int) TouchInput.InputTouchDown].OnChange -= TouchDown;
        _touch[(int) TouchInput.InputTouchUp].OnChange -= TouchUp;
    }
}