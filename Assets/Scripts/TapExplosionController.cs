using UnityEngine;

public class TapExplosionController : IExecute, IClean
{
    private Camera _camera;
    private float _explosionArea;
    private float _explosionForce;
    private IUserInput<Vector3> _inputTouch;
    private Vector3 _touchPosition;
    private bool _isTouched;

    public TapExplosionController(Camera camera, IUserInput<Vector3> inputTouch, float explosionArea, float explosionForce)
    {
        _camera = camera;
        _explosionArea = explosionArea;
        _explosionForce = explosionForce;
        _inputTouch = inputTouch;
        _inputTouch.OnChange += TouchPositionOnChange;
    }

    private void TouchPositionOnChange(Vector3 touchPosition)
    {
        _touchPosition = touchPosition;
        _isTouched = true;
    }

    private void Shoot(bool isTouched)
    {
        if (!isTouched) return;
        
        var ray = _camera.ScreenPointToRay(_touchPosition); 
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

            Debug.Log(hitsSphereCast.Length);
            if (hitsSphereCast.Length > 1)
            {
                var explosionVector = hitsSphereCast[0].point - hitSphereExplosion.point;
                hitSphereExplosion.rigidbody.AddForce(explosionVector * _explosionForce, ForceMode.Impulse);
            }
            else
            {
                hitsSphereCast[0].rigidbody.AddForce(ray.direction * _explosionForce, ForceMode.Impulse);
            }
        }
        
        _isTouched = false;
    }

    public void Execute(float deltaTime)
    {
        Shoot(_isTouched);
    }

    public void Clean()
    {
        _inputTouch.OnChange -= TouchPositionOnChange;
    }
}