using System;
using UnityEngine;

public class TapExplosion : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private float _areaExplosion;
    [SerializeField] private float _explosionForce;

    private RaycastHit[] raycastHits;
    

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            
            raycastHits = new RaycastHit[3];
            var hits = Physics.RaycastNonAlloc(ray, raycastHits);
            if (hits <= 0) return;
            var hitsSphere = Physics.SphereCastAll(raycastHits[0].point, _areaExplosion, raycastHits[0].normal);
            foreach (var hitSphere in hitsSphere)
            {
                if (!hitSphere.rigidbody.isKinematic) continue;
                hitSphere.rigidbody.isKinematic = false;
                var explosionVector = raycastHits[raycastHits.Length - 1].transform.position -
                                      raycastHits[0].transform.position;
                raycastHits[raycastHits.Length - 1].rigidbody.AddForce(explosionVector * _explosionForce, ForceMode.Impulse);
            }
        }
    }
}