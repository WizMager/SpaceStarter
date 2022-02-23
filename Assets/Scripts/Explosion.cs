using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private GameObject _planetCenter;
    private void Start()
    {
        gameObject.GetComponent<Rigidbody>().AddForce(Vector3.forward * 50f, ForceMode.Impulse);
        if (_planetCenter != null)
        {
            Destroy(_planetCenter);  
        }
    }
}
