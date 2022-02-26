using UnityEngine;

public class TapExplosionController : IExecute, IClean
{
    private Camera _camera;
    private float _explosionArea;
    private float _explosionForce;
    private IUserInput<Vector3> _inputTouch;
    private Vector3 _touchPosition;

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
    }

    private void Shoot(bool isTouched)
    {
        if (!isTouched) return;
        var ray = _camera.ScreenPointToRay(_touchPosition); 
        var hitRaycast = new RaycastHit[1];
        if (Physics.RaycastNonAlloc(ray, hitRaycast) > 0)
        {
            var hitsSphereCast = Physics.SphereCastAll(hitRaycast[0].point, _explosionArea, hitRaycast[0].normal);
            foreach (var hitSphereCast in hitsSphereCast)
            {
                if (!hitSphereCast.rigidbody.isKinematic) continue;
                hitSphereCast.rigidbody.isKinematic = false;
            }
        }
    }
    
    public void Execute(float deltaTime)
    {
        
    }

    public void Clean()
    {
        
    }
}