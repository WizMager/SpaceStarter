using System;
using UnityEngine;

public class TapExplosion : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private float _areaExplosion;
    [SerializeField] private float _explosionForce;
    
    private RaycastHit[] _raycastHits = new RaycastHit[3];
    
    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            var hits = Physics.RaycastNonAlloc(ray, _raycastHits);

            if (hits <= 0)
            {
                return;
            }
            
            var hitsSphere = Physics.SphereCastAll(
                _raycastHits[0].point, 
                _areaExplosion, 
                _raycastHits[0].normal
            );
            
            foreach (var hitSphere in hitsSphere)
            {
                if (!hitSphere.rigidbody.isKinematic)
                {
                    continue;
                }
                
                hitSphere.rigidbody.isKinematic = false;

                var firstHit = _raycastHits[0];
                var lastHit = _raycastHits[_raycastHits.Length - 1];
                var explosionVector = lastHit.transform.position - firstHit.transform.position;
                
                lastHit.rigidbody.AddForce(explosionVector * _explosionForce, ForceMode.Impulse);
            }
        }
    }
}