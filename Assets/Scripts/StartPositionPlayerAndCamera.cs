using UnityEngine;
using View;

public class StartPositionPlayerAndCamera
{
    private readonly Transform _player;
    private readonly Transform _planet;
    private readonly Transform _gravity;
    private readonly Transform _camera;
    private readonly float _startDistanceFromPlanet;
    private readonly float _startCameraHeight;
    private readonly float _restartCameraHeight;
    private readonly GravityLittleView _gravityLittleView;

    public StartPositionPlayerAndCamera(Transform playerTransform, Transform planetTransform, Transform gravityTransform,
        Transform cameraTransform, float startDistanceFromPlanet, float startCameraHeight, float restartCameraHeight, GravityLittleView gravityLittleView)
    {
        _player = playerTransform;
        _planet = planetTransform;
        _gravity = gravityTransform;
        _camera = cameraTransform;
        _startDistanceFromPlanet = startDistanceFromPlanet;
        _startCameraHeight = startCameraHeight;
        _restartCameraHeight = restartCameraHeight;
        _gravityLittleView = gravityLittleView;
    }

    public void SetPlayerAndCamera(bool isFirstTry)
    {
        if (isFirstTry)
        {
            SetDefault();
        }
        else
        {
            SetRestart();
        }
    }
    
    private void SetRestart()
    {
        _player.gameObject.SetActive(true);
        _planet.GetComponent<SphereCollider>().enabled = true;
        _gravity.gameObject.SetActive(true);
        _gravityLittleView.gameObject.SetActive(true);
        var planetRay = new Ray(_planet.position, _planet.forward);
        var gravityRadius = _gravity.gameObject.GetComponent<MeshCollider>().bounds.size.x / 2;
        var pathToCenter = gravityRadius - (gravityRadius - _planet.gameObject.GetComponent<SphereCollider>().radius) / 2;
        var startPlayerPosition = planetRay.GetPoint(pathToCenter);
        var rotation = Quaternion.FromToRotation(_planet.forward, _planet.right) * Quaternion.Euler(0, 0, 90f);

        _player.position = startPlayerPosition;
        _player.LookAt(_planet.position);
        _player.rotation = rotation;

        var cameraPosition = startPlayerPosition;
        cameraPosition.y = _restartCameraHeight;
        var startRotation = Quaternion.Euler(new Vector3(90, 180, 180));
        _camera.SetPositionAndRotation(cameraPosition, startRotation);
    }

    private void SetDefault()
    {
        var planetRay = new Ray(_planet.position, _planet.forward);
        _player.position = planetRay.GetPoint(_startDistanceFromPlanet);
        _player.LookAt(_planet.position);
        
        var cameraRay = new Ray(_player.position, _player.forward);
        var planetRadius = _planet.GetComponent<SphereCollider>().radius;
        var distanceHalfGravity = (_gravity.GetComponent<MeshCollider>().bounds.size.x / 2 - planetRadius) / 2;
        var distanceToCenterGravity = _startDistanceFromPlanet - planetRadius - distanceHalfGravity;
        var defaultCameraPosition = cameraRay.GetPoint(distanceToCenterGravity);
        defaultCameraPosition.y = _startCameraHeight;
        //TODO: this is just for test and never will be change
        defaultCameraPosition.z += 1.2f;
        var defaultCameraRotation = Quaternion.Euler(new Vector3(90, 180, 180));
        _camera.SetPositionAndRotation(defaultCameraPosition, defaultCameraRotation);
    }
}