using UnityEngine;

public class CameraTopDownController: IExecute
{
    private Camera _cameraTopDown;
    private Camera _cameraFirstPerson;
    private Transform[] _planets;
    private readonly Transform _player;
    private Vector3 _startVector;
    private Vector3 _endVector;

    public CameraTopDownController(Camera[] cameras, Transform[] firstStagePlanetsTransfrom, Transform playerTransform)
    {
        _cameraTopDown = cameras[0];
        _cameraFirstPerson = cameras[1];
        _planets = firstStagePlanetsTransfrom;
        _player = playerTransform;
        _startVector = _player.position - _planets[0].position;
        _endVector = _startVector;
    }

    public void Execute(float deltaTime)
    {
        _startVector = _player.position - _planets[0].position;
        var rotateAngle = Vector3.Angle(_endVector, _startVector);
        _cameraTopDown.transform.RotateAround(_planets[0].position, Vector3.up, rotateAngle);
        _endVector = _player.position - _planets[0].position;
    }
}