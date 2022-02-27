using UnityEngine;

public class CameraTopDownController: IExecute, IClean
{
    private Camera _camera;
    private Transform _currentPlanet;
    private float _speedRotation;

    public CameraTopDownController(Camera cameraTopDown, Transform planet, float speedRotation)
    {
        _camera = cameraTopDown;
        _currentPlanet = planet;
        _speedRotation = speedRotation;
    }

    public void Execute(float deltaTime)
    {
        _camera.transform.RotateAround(_currentPlanet.position, Vector3.up, _speedRotation * deltaTime);
    }

    public void Clean()
    {
        
    }
}