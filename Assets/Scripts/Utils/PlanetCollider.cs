using System;
using UnityEngine;

public class PlanetCollider : MonoBehaviour
{
    public event Action OnPlayerPlanetEnter;
    public event Action OnPlayerPlanetExit;

    private void OnTriggerEnter(Collider other)
    {
        OnPlayerPlanetEnter?.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        OnPlayerPlanetExit?.Invoke();
    }
}
