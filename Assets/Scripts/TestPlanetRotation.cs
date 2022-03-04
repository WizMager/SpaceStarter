using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlanetRotation : MonoBehaviour
{
    [SerializeField] private float _speedRotation;

    void Update()
    {
        transform.RotateAround(transform.position, transform.up, -_speedRotation * Time.deltaTime);
    }
}
