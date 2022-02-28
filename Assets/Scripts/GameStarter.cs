using UnityEngine;

public class GameStarter : MonoBehaviour
{
    [SerializeField] private Data _data;
    [SerializeField] private Camera[] _cameras;
    [SerializeField] private GameObject[] _planetsCenter;
    [SerializeField] private GameObject _player;
    private Controllers _controllers;

    private void Start()
    {
        _controllers = new Controllers();
        new GameInitialization(_controllers, _data, _cameras, _planetsCenter, _player);
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

    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.yellow;
    //     Gizmos.DrawRay(_player.transform.position, _player.transform.up);
    //     Gizmos.color = Color.green;
    //     Gizmos.DrawRay(_player.transform.position, _player.transform.right);
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawRay(_player.transform.position, _player.transform.up - _player.transform.right);
    // }
}