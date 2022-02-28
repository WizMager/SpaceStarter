using UnityEngine;

public class CameraTopDownController: IExecute, IClean
{
    private Camera _camera;
    private Transform _currentPlanet;
    private float _speedRotation;
    private readonly Transform _player;
    private Vector3 _startVector;
    private Vector3 _endVector;

    public CameraTopDownController(Camera cameraTopDown, Transform planet, float speedRotation, Transform playerTransform)
    {
        _camera = cameraTopDown;
        _currentPlanet = planet;
        _speedRotation = speedRotation;
        _player = playerTransform;
        _startVector = _player.position - _currentPlanet.position;
        _endVector = _startVector;
    }

    public void Execute(float deltaTime)
    {
        _startVector = _player.position - _currentPlanet.position;
        // var distance = _camera.transform.position.y;
        // var cameraPosition = _player.position - _camera.transform.forward;
        // cameraPosition.y = distance;
        // _camera.transform.position = cameraPosition;
        var rotateAngle = Vector3.Angle(_endVector, _startVector);
        //_camera.transform.Rotate(_camera.transform.forward, rotateAngle);
        _camera.transform.RotateAround(_currentPlanet.position, Vector3.up, rotateAngle);
        _endVector = _player.position - _currentPlanet.position;
    }

    public void Clean()
    {
        
    }
}