using Controllers;
using UnityEngine;

public class GameStarter : MonoBehaviour
{
    [SerializeField] private ScriptableData.AllData _data;
    private AllControllers _controllers;

    private void Start()
    {
        _controllers = new AllControllers();
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