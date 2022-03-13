using System.Collections;
using UnityEngine;
using Utils;
using View;

public class CameraController : IClean
{
    private readonly Camera _camera;
    private readonly float _cameraStartUpDivision;
    private readonly float _cameraUpSpeed;
    private readonly float _cameraUpOffset;
    private readonly IUserInput<float> _vertical;
    private readonly IUserInput<float> _horizontal;
    private readonly Vector3 _lastPlanetCenter;
    private readonly float _fpRotationSpeed;
    private readonly float _cameraDownPosition;
    private readonly float _cameraDownSpeed;

    private readonly Transform _playerTransform;
    private float _distanceToPlayer;
    private bool _isLastPlanet;

    public CameraController(Camera camera, float cameraStartUpDivision, float cameraUpSpeed, float cameraUpOffset, 
        IUserInput<float>[] axisInput, Vector3 lastPlanetCenter, float fpRotationSpeed, PlayerView playerView,
        float cameraDownPosition, float cameraDownSpeed)
    {
        _camera = camera;
        _cameraStartUpDivision = cameraStartUpDivision;
        _cameraUpSpeed = cameraUpSpeed;
        _cameraUpOffset = cameraUpOffset;
        _vertical = axisInput[(int) AxisInput.InputVertical];
        _horizontal = axisInput[(int) AxisInput.InputHorizontal];
        _lastPlanetCenter = lastPlanetCenter;
        _fpRotationSpeed = fpRotationSpeed;
        _cameraDownPosition = cameraDownPosition;
        _cameraDownSpeed = cameraDownSpeed;

        _playerTransform = playerView.transform;
        _vertical.OnChange += VerticalChanged;
        _horizontal.OnChange += HorizontalChanged;
    }
    

    public void FollowPlayer()
    {
        var offsetPosition = _playerTransform.position;
        var cameraTransform = _camera.transform;
        offsetPosition.y += cameraTransform.position.y;
        cameraTransform.position = offsetPosition;
    }

    public void CameraUp(float deltaTime)
    {
        if (_distanceToPlayer < _cameraUpOffset)
        {
            if (_distanceToPlayer == 0)
            {
                _distanceToPlayer = (_camera.transform.position.y - _playerTransform.position.y) / _cameraStartUpDivision;
            }
            _distanceToPlayer += deltaTime * _cameraUpSpeed;
        }
        var offsetPosition = _playerTransform.position;
        offsetPosition.y += _distanceToPlayer;
        _camera.transform.position = offsetPosition;
    }
    
    public void CameraDown(float deltaTime)
    {
        var offsetY = _camera.transform.position.y;
        var playerTransformPosition = _playerTransform.position;
        var offsetX = playerTransformPosition.x;
        var offsetZ = playerTransformPosition.z;
        if (_cameraDownPosition <= offsetY)
        {
            offsetY -= deltaTime * _cameraDownSpeed;
            var offset = new Vector3(offsetX, offsetY, offsetZ);
            _camera.transform.position = offset;
        }
        else
        {
            var offset = new Vector3(offsetX, offsetY, offsetZ);
            _camera.transform.position = offset;
        }
    }
    
    public void FirstPersonActivation()
    {
        var position = new Vector3(-15.43f, 0.86f, 40.65f);
        var rotation = Quaternion.Euler(new Vector3(0, -90f, 0));
        _camera.transform.SetPositionAndRotation(position, rotation);
        _isLastPlanet = true;
    }

    private void VerticalChanged(float value)
    {
        if (!_isLastPlanet) return;
        _camera.transform.RotateAround(_lastPlanetCenter, Vector3.forward, value * _fpRotationSpeed);
    }

    private void HorizontalChanged(float value)
    {
        if (!_isLastPlanet) return;
        _camera.transform.RotateAround(_lastPlanetCenter, Vector3.up, -value * _fpRotationSpeed);
    }


    public void Clean()
    {
        _vertical.OnChange -= VerticalChanged;
        _horizontal.OnChange -= HorizontalChanged;
    }
}