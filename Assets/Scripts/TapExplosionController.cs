using UnityEngine;
using Utils;

public class TapExplosionController
{
    private IUserInput<Vector3>[] _touch;
    private Camera _camera;
    private float _explosionArea;
    private float _explosionForce;
    private GameObject _particle;

    private bool _isActive;

    public TapExplosionController(IUserInput<Vector3>[] touch, Camera camera, float explosionArea, float explosionForce, GameObject particle)
    {
        _touch = touch;
        _camera = camera;
        _explosionArea = explosionArea;
        _explosionForce = explosionForce;
        _particle = particle;

        _touch[(int) TouchInput.InputTouchDown].OnChange += TouchDown;
    }

    private void Shoot(Vector3 touchPosition)
    {
        var ray = _camera.ScreenPointToRay(touchPosition);
        var hitRaycast = new RaycastHit[1];
        if (Physics.RaycastNonAlloc(ray, hitRaycast) > 0)
        {
            var hitsSphereCast = Physics.SphereCastAll(hitRaycast[0].point, _explosionArea, ray.direction);
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
    }

    private void TouchDown(Vector3 touchPosition)
    {
        if (!_isActive) return;
        
        Shoot(touchPosition);
    }

    public void SetActive()
    {
        _isActive = true;
    }

    public void OnDestroy()
    {
        _touch[(int) TouchInput.InputTouchDown].OnChange -= TouchDown;
    }
}