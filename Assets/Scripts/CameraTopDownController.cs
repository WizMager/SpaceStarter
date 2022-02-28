using UnityEngine;

public class CameraTopDownController: IExecute
{
    private Camera _camera;
    private Transform _currentPlanet;
    private readonly Transform _player;
    private Vector3 _startVector;
    private Vector3 _endVector;

    public CameraTopDownController(Camera cameraTopDown, Transform planet, Transform playerTransform)
    {
        _camera = cameraTopDown;
        _currentPlanet = planet;
        _player = playerTransform;
        _startVector = _player.position - _currentPlanet.position;
        _endVector = _startVector;
    }

    public void Execute(float deltaTime)
    {
        _startVector = _player.position - _currentPlanet.position;
        var rotateAngle = Vector3.Angle(_endVector, _startVector);
        _camera.transform.RotateAround(_currentPlanet.position, Vector3.up, rotateAngle);
        _endVector = _player.position - _currentPlanet.position;
    }
}