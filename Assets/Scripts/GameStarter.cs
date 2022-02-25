using System;
using UnityEngine;

public class GameStarter : MonoBehaviour
{
    [SerializeField] private Data _data;
    private Controllers _controllers;
    private float _deltaTime;

    private void Start()
    {
        _controllers = new Controllers();
        new GameInitialization(_controllers, _data);
        _controllers.Initialization();
    }

    private void Update()
    {
        _deltaTime = Time.deltaTime;
        _controllers.Execute(_deltaTime);
    }

    private void OnDestroy()
    {
        _controllers.Clean();
    }
}