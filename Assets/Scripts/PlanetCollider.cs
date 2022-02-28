using System;
using UnityEngine;

public class PlanetCollider : MonoBehaviour
{
    public event Action OnPlayerPlanetEnter;
    public event Action OnPlayerPlanetExit;
    private void OnCollisionEnter(Collision collision)
    {
        OnPlayerPlanetEnter?.Invoke();
    }

    private void OnCollisionExit(Collision other)
    {
        OnPlayerPlanetExit?.Invoke();
    }
}
