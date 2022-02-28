using System;
using UnityEngine;

public class PlanetCollider : MonoBehaviour
{
    public event Action OnPlayerEnter;
    public event Action OnPlayerExit;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player")
        {
            OnPlayerEnter?.Invoke();
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.name == "Player")
        {
            OnPlayerExit?.Invoke();
        }
    }
}
