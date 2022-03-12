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
    private readonly PlayerView _playerView;
    private readonly float _moveSpeedCenter;
    private readonly float _cameraDown;
    
    private readonly Transform _playerTransform;
    private float _distanceToPlayer;
    private bool _isLastPlanet;
    private Vector3 _cameraDownEnd;
    private float _distnceCenter;
    
    public CameraController(Camera camera, float cameraStartUpDivision, float cameraUpSpeed, float cameraUpOffset, 
        IUserInput<float>[] axisInput, Vector3 lastPlanetCenter, float fpRotationSpeed, PlayerView playerView, float moveSpeedCenterGravity, float cameraDownOffset)
    {
        _camera = camera;
        _cameraStartUpDivision = cameraStartUpDivision;
        _cameraUpSpeed = cameraUpSpeed;
        _cameraUpOffset = cameraUpOffset;
        _vertical = axisInput[(int) AxisInput.InputVertical];
        _horizontal = axisInput[(int) AxisInput.InputHorizontal];
        _lastPlanetCenter = lastPlanetCenter;
        _fpRotationSpeed = fpRotationSpeed;
        _playerView = playerView;
        _moveSpeedCenter = moveSpeedCenterGravity;
        _cameraDown = cameraDownOffset;

        _playerTransform = _playerView.transform;
        _vertical.OnChange += VerticalChanged;
        _horizontal.OnChange += HorizontalChanged;
    }
    

    public void FollowPlayer(float deltaTime)
    {
        if (_isLastPlanet) return;
        
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

    public void SetCameraDown(float distance)
    {
        _cameraDownEnd = new Vector3(_camera.transform.position.x, _cameraDown, _camera.transform.position.z);
        _distnceCenter = distance;
        _playerView.StartCoroutine(CameraDown());
    }
    
    private IEnumerator CameraDown()
    {
        //TODO: add calculate time to rotate with cycle for
        for (float i = 0; i < _distnceCenter; i += Time.deltaTime * _moveSpeedCenter)
        {
            var offset = _camera.transform.position;
            _camera.transform.position = Vector3.Lerp(offset, _cameraDownEnd, i / _distnceCenter);
            yield return null;
        }
        
        _playerView.StopCoroutine(CameraDown());
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