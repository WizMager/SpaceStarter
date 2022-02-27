using System;
using UnityEngine;

public class GameStarter : MonoBehaviour
{
    [SerializeField] private Data _data;
    [SerializeField] private Camera[] _cameras;
    [SerializeField] private GameObject[] _planetsCenter;
    [SerializeField] private Transform _playerTransform;
    private Controllers _controllers;

    private void Start()
    {
        _controllers = new Controllers();
        new GameInitialization(_controllers, _data, _cameras, _planetsCenter, _playerTransform);
        _controllers.Initialization();
    }

    private void Update()
    {
        var deltaTime = Time.deltaTime;
        _controllers.Execute(deltaTime);
    }

    private void OnDestroy()
    {
        _controllers.Clean();
    }
}