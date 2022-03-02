using UnityEngine;

public class CameraTopDown
{
    private readonly Camera _camera;
    private Vector3 _startVector;
    private Vector3 _endVector;
    private float _distanceToPlayer;
    private float _cameraUpDivision;

    public CameraTopDown(Camera camera, float cameraUpDivision)
    {
        _camera = camera;
        _cameraUpDivision = cameraUpDivision;
    }

    public void RotateAroundPlanet(Transform player, Transform currentPlanet)
    {
        _startVector = player.position - currentPlanet.position;
        var rotateAngle = Vector3.Angle(_endVector, _startVector);
        _camera.transform.RotateAround(currentPlanet.position, Vector3.up, rotateAngle);
        _endVector = player.position - currentPlanet.position;
    }

    public void FollowPlayer(Transform player, float distanceToPlayer, float deltaTime)
    {
        if (_distanceToPlayer < distanceToPlayer)
        {
            if (_distanceToPlayer == 0)
            {
                _distanceToPlayer = (Mathf.Abs(player.position.y) - Mathf.Abs(_camera.transform.position.y)) / _cameraUpDivision;
            }
            _distanceToPlayer += deltaTime;
        }
        var offsetPosition = player.transform.position;
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