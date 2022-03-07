using UnityEngine;

public class CameraController : IClean
{
    private readonly Camera _camera;
    private readonly float _cameraStartUpDivision;
    private readonly float _cameraUpSpeed;
    private readonly float _cameraUpOffset;
    private IUserInput<float> _vertical;
    private IUserInput<float> _horizontal;
    private readonly Vector3 _lastPlanetCenter;
    private readonly float _fpRotationSpeed;

    // private Vector3 _startVector;
    // private Vector3 _cameraRotationOffset;
    // private Vector3 _cameraPositionOffset;
    private float _distanceToPlayer;
    private bool _isLastPlanet;

    public CameraController(Camera camera, float cameraStartUpDivision, float cameraUpSpeed, float cameraUpOffset, 
        (IUserInput<float> InputVertical, IUserInput<float> InputHorizontal) axisInput, Vector3 lastPlanetCenter, float fpRotationSpeed)
    {
        _camera = camera;
        _cameraStartUpDivision = cameraStartUpDivision;
        _cameraUpSpeed = cameraUpSpeed;
        _cameraUpOffset = cameraUpOffset;
        _vertical = axisInput.InputVertical;
        _horizontal = axisInput.InputHorizontal;
        _lastPlanetCenter = lastPlanetCenter;
        _fpRotationSpeed = fpRotationSpeed;

        _vertical.OnChange += VerticalChanged;
        _horizontal.OnChange += HorizontalChanged;

        // _cameraRotationOffset = new Vector3(camera.transform.rotation.eulerAngles.x, 0, camera.transform.rotation.eulerAngles.z);
        // _cameraPositionOffset = _camera.transform.position;
    }

    // Work not right when flew over to other planet.
    // public void RotateAroundPlanet(Transform player, Transform currentPlanet)
    // {
    //     _camera.transform.rotation = Quaternion.Euler(_cameraRotationOffset.x, _camera.transform.rotation.eulerAngles.y, _cameraRotationOffset.z);
    //      var endVector = player.position - currentPlanet.position;
    //      var rotateAngle = Vector3.Angle(_startVector, endVector);
    //      _camera.transform.RotateAround(currentPlanet.position, currentPlanet.up, rotateAngle);
    //      _startVector = endVector;
    // }

    public void FollowPlayer(Transform player, float deltaTime)
    {
        if (_distanceToPlayer < _cameraUpOffset)
        {
            if (_distanceToPlayer == 0)
            {
                _distanceToPlayer = (_camera.transform.position.y - player.position.y) / _cameraStartUpDivision;
            }
            _distanceToPlayer += deltaTime * _cameraUpSpeed;
        }
        var offsetPosition = player.position;
        offsetPosition.y += _distanceToPlayer;
        _camera.transform.position = offsetPosition;
    }

    public void FirstPersonActivation()
    {
        var position = new Vector3(-12f, 1f, 40.9f);
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