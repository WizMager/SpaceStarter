using UnityEngine;

public class StartPositionPlayerAndCamera
{
    private readonly Transform _player;
    private readonly Transform _planet;
    private readonly Transform _gravity;
    private readonly Transform _camera;
    private readonly float _startDistanceFromPlanet;
    private readonly float _startCameraHeight;

    public StartPositionPlayerAndCamera(Transform playerTransform, Transform planetTransform, Transform gravityTransform,
        Transform cameraTransform, float startDistanceFromPlanet, float startCameraHeight)
    {
        _player = playerTransform;
        _planet = planetTransform;
        _gravity = gravityTransform;
        _camera = cameraTransform;
        _startDistanceFromPlanet = startDistanceFromPlanet;
        _startCameraHeight = startCameraHeight;
    }

    public void Set()
    {
        var planetRay = new Ray(_planet.position, _planet.forward);
        var startPlayerPosition = planetRay.GetPoint(_startDistanceFromPlanet);
        _player.position = startPlayerPosition;
        _player.LookAt(_planet.position);
        
        var cameraRay = new Ray(_player.position, _player.forward);
        var planetRadius = _planet.GetComponent<SphereCollider>().radius;
        var distanceHalfGravity = (_gravity.GetComponent<MeshCollider>().bounds.size.x / 2 - planetRadius) / 2;
        var distanceToCenterGravity = _startDistanceFromPlanet - planetRadius - distanceHalfGravity;
        var startPosition = cameraRay.GetPoint(distanceToCenterGravity);
        startPosition.y = _startCameraHeight;
        var startRotation = Quaternion.Euler(new Vector3(90, 180, 180));
        _camera.SetPositionAndRotation(startPosition, startRotation);
    }
}