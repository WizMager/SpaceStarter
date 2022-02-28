using UnityEngine;

public class GameStarter : MonoBehaviour
{
    [SerializeField] private Data _data;
    [SerializeField] private Camera[] _cameras;
    [SerializeField] private GameObject[] _planets;
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject[] _gravityFields;
    private Controllers _controllers;

    private void Start()
    {
        _controllers = new Controllers();
        new GameInitialization(_controllers, _data, _cameras, _planets, _player, _gravityFields);
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

    private void OnDrawGizmos()
    {
        // Gizmos.color = Color.yellow;
        // Gizmos.DrawRay(_player.transform.position, _player.transform.up);
        // Gizmos.color = Color.green;
        // Gizmos.DrawRay();
        // Gizmos.color = Color.red;
        // Gizmos.DrawRay();
    }
}