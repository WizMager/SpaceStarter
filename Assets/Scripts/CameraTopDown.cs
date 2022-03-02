using UnityEngine;

public class CameraTopDown
{
    private readonly Camera _camera;
    private Vector3 _startVector;
    private Vector3 _endVector;

    public CameraTopDown(Camera camera)
    {
        _camera = camera;

    }

    public void RotateAroundPlanet(Transform player, Transform currentPlanet)
    {
        _startVector = player.position - currentPlanet.position;
        var rotateAngle = Vector3.Angle(_endVector, _startVector);
        _camera.transform.RotateAround(currentPlanet.position, Vector3.up, rotateAngle);
        _endVector = player.position - currentPlanet.position;
    }

    public void FollowPlayer(Transform player, float distanceToPlayer)
    {
        var offsetPosition = player.transform.position;
        offsetPosition.y += distanceToPlayer;
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