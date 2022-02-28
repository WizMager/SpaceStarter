using System;
using UnityEngine;

public class GravityCollider : MonoBehaviour
{
    public event Action<Vector3> OnPlayerGravityEnter;
    public event Action OnPlayerGravityExit;

    private void OnCollisionEnter(Collision collision)
    {
        OnPlayerGravityEnter?.Invoke(collision.contacts[0].point);
    }

    private void OnCollisionExit(Collision other)
    {
        OnPlayerGravityExit?.Invoke();
    }
}
