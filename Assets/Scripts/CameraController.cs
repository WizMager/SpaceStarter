using UnityEngine;

public class CameraController
{
    private readonly Camera _camera;
    private Vector3 _startVector;
    private float _distanceToPlayer;
    private float _cameraStartUpDivision;
    private float _cameraUpMultiply;
    private Vector3 _cameraRotationOffset;
    private Vector3 _cameraPositionOffset;

    public CameraController(Camera camera, float cameraStartUpDivision, float cameraUpMultiply)
    {
        _camera = camera;
        _cameraStartUpDivision = cameraStartUpDivision;
        _cameraUpMultiply = cameraUpMultiply;
        _cameraRotationOffset = new Vector3(camera.transform.rotation.eulerAngles.x, 0, camera.transform.rotation.eulerAngles.z);
        _cameraPositionOffset = _camera.transform.position;
    }

    public void RotateAroundPlanet(Transform player, Transform currentPlanet)
    {
        //_camera.transform.position = currentPlanet.position + new Vector3(0, _cameraPositionOffset.y, 0);

        _camera.transform.rotation = Quaternion.Euler(_cameraRotationOffset.x, _camera.transform.rotation.eulerAngles.y, _cameraRotationOffset.z);
         var endVector = player.position - currentPlanet.position;
         var rotateAngle = Vector3.Angle(_startVector, endVector);
         _camera.transform.RotateAround(currentPlanet.position, currentPlanet.up, rotateAngle);
         _startVector = endVector;
    }

    public void FollowPlayer(Transform player, float distanceToPlayer, float deltaTime)
    {
        if (_distanceToPlayer < distanceToPlayer)
        {
            if (_distanceToPlayer == 0)
            {
                _distanceToPlayer = (_camera.transform.position.y - player.position.y) / _cameraStartUpDivision;
            }
            _distanceToPlayer += deltaTime * _cameraUpMultiply;
        }
        var offsetPosition = player.position;
        offsetPosition.y += _distanceToPlayer;
        _camera.transform.position = offsetPosition;
    }
    
    public void FollowPlayerWithRotation(Transform player, float distanceToPlayer)
    {
        var offsetPosition = player.transform.position;
        offsetPosition.y += distanceToPlayer;
        var direction = Vector3.RotateTowards(_camera.transform.forward, 
            player.position - _camera.transform.position, 360f, 0f);
        _camera.transform.SetPositionAndRotation(offsetPosition, Quaternion.LookRotation(direction));
    }
}