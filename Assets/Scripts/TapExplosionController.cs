using UnityEngine;

public class TapExplosionController
{
    private Camera _camera;
    private float _explosionArea;
    private float _explosionForce;
    private GameObject _particle;

    public TapExplosionController(Camera camera, float explosionArea, float explosionForce, GameObject particle)
    {
        _camera = camera;
        _explosionArea = explosionArea;
        _explosionForce = explosionForce;
        _particle = particle;
    }

    public void Shoot(Vector3 touchPosition)
    {
        var ray = _camera.ScreenPointToRay(touchPosition);
        var hitRaycast = new RaycastHit[1];
        if (Physics.RaycastNonAlloc(ray, hitRaycast) > 0)
        {
            var hitsSphereCast = Physics.SphereCastAll(hitRaycast[0].point, _explosionArea, ray.direction);
            var hitSphereExplosion = hitsSphereCast[0];
            foreach (var hitSphereCast in hitsSphereCast)
            {
                //TODO: fix problem with null ref when access to rb in array from spherecast
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
}