using Controller;
using Data;
using UnityEngine;

public class GameStarter : MonoBehaviour
{
    [SerializeField] private ScriptableData _data;
    private Controllers _controllers;

    private void Start()
    {
        _controllers = new Controllers();
        new GameInitialization(_controllers, _data);
        _controllers.Initialization();
    }

    private void Update()
    {
        var deltaTime = Time.deltaTime;
        _controllers.Execute(deltaTime);
    }

    private void FixedUpdate()
    {
        var fixedDeltaTime = Time.fixedDeltaTime;
        _controllers.FixedExecute(fixedDeltaTime);
    }

    private void OnDestroy()
    {
        _controllers.Clean();
    }
}