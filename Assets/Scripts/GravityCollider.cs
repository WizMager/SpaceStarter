using System;
using UnityEngine;

public class GravityCollider : MonoBehaviour
{
    public event Action<Vector3> OnPlayerFirstGravityEnter;
    public event Action OnPlayerGravityEnter;
    public event Action OnPlayerGravityExit;

    private void OnCollisionEnter(Collision collision)
    {
        var contact = collision.GetContact(0);
        OnPlayerFirstGravityEnter?.Invoke(contact.point);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            OnPlayerGravityEnter?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            OnPlayerGravityExit?.Invoke();
        }
    }
}
