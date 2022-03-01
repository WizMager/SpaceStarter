using UnityEngine;
using UnityEngine.UIElements;

public class TapExplosionControllerExperimental : IExecute, IClean
{
   private Camera _camera;
    private float _explosionArea;
    private float _explosionBoxForce;
    private IUserInput<Vector3> _inputTouch;
    private Vector3 _touchPosition;
    private bool _isTouched;
    private GameObject _particle;
    private GameObject _explosionBoxPrefab;
    private GameObject _lastPlanet;

    public TapExplosionControllerExperimental(Camera camera, IUserInput<Vector3> inputTouch, float explosionArea, 
        float explosionBoxForce, GameObject particle, GameObject explosionBoxPrefab, GameObject lastPlanet)
    {
        _camera = camera;
        _explosionArea = explosionArea;
        _explosionBoxForce = explosionBoxForce;
        _inputTouch = inputTouch;
        _inputTouch.OnChange += TouchPositionOnChange;
        _particle = particle;
        _explosionBoxPrefab = explosionBoxPrefab;
        _lastPlanet = lastPlanet;
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
            var hitsSphereCast = Physics.SphereCastAll(hitRaycast[0].point, _explosionArea, 
                ray.direction, _explosionArea);
            
            var pos = hitsSphereCast[0].transform.position;
            //var position = new Vector3(pos.x + 3.05f, pos.y + 1.05f, pos.z - 1);
            Object.Destroy(hitsSphereCast[0].transform.gameObject);
            var go = Object.Instantiate(_explosionBoxPrefab, pos, Quaternion.identity);
            //go.transform.position += new Vector3(3.05f, 1.05f, -1);
            // foreach (var hitSphereCast in hitsSphereCast)
            // {
            //     var pos = hitSphereCast.transform.position;
            //     var position = new Vector3(pos.x - 3.05f, pos.y - 1.05f, pos.z + 1);
            //     Object.Destroy(hitSphereCast.transform.gameObject);
            //     Object.Instantiate(_explosionBoxPrefab, position, Quaternion.identity);
            // }
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