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
        var hitRaycast = new RaycastHit[1];
        if (Physics.RaycastNonAlloc(ray, hitRaycast, _camera.farClipPlane, GlobalData.LayerForAim) > 0)
        {
            var hitsSphereCast = Physics.SphereCastAll(hitRaycast[0].point, _explosionArea, ray.direction);
            Debug.Log(hitsSphereCast.Length);
            var hitSphereExplosion = hitsSphereCast[0];
            foreach (var hitSphereCast in hitsSphereCast)
            {
                if (hitSphereCast.rigidbody.isKinematic)
                {
                    hitSphereCast.rigidbody.isKinematic = false;
                }
                
                if (hitSphereCast.distance > hitSphereExplosion.distance)
                {
                    hitSphereExplosion = hitSphereCast;
                }
            }
            
            if (hitsSphereCast.Length > 1)
            {
                hitSphereExplosion.rigidbody.AddForce(hitsSphereCast[0].normal * _explosionForce, ForceMode.Impulse);
                Object.Instantiate(_particle, hitRaycast[0].point, Quaternion.identity);
            }
            else
            {
                hitsSphereCast[0].rigidbody.AddForce(ray.direction * _explosionForce, ForceMode.Impulse);
                Object.Instantiate(_particle, hitRaycast[0].point, Quaternion.identity);
            }
        }
        Debug.Log(hitRaycast[0].transform.gameObject.name);
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