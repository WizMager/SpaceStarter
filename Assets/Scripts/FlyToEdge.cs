using UnityEngine;

public class FlyToEdge
{
    private readonly float _speedRotation;
    private readonly float _moveSpeed;
    
    private float _rotationAngle;
    private bool _isActive;
    private Vector3 _startDirection;
    private Vector3 _endDirection;

    public FlyToEdge(float speedRotationToGravityEdge, float speedMoveToEdgeGravity)
    {
        _speedRotation = speedRotationToGravityEdge;
        _moveSpeed = speedMoveToEdgeGravity;
    }
    
    public bool FlyingToEdge(Transform playerTransform, float deltaTime)
    {
        if (!_isActive) return false;
        
        if (_rotationAngle <= 0)
        {
            playerTransform.Translate(-playerTransform.forward * deltaTime * _moveSpeed);
        }
        else
        {
            var playerRotation = playerTransform.rotation.eulerAngles;
            playerRotation.x = 0;
            var angleToRotate = _speedRotation * deltaTime;
            Debug.DrawLine(playerRotation, _endDirection, Color.green);
            playerTransform.rotation = Quaternion.Lerp( playerTransform.rotation,Quaternion.LookRotation(_endDirection, playerTransform.up), angleToRotate);
            //playerTransform.rotation = Quaternion.Lerp(Quaternion.Euler(playerRotation), Quaternion.Euler(_endDirection), angleToRotate);
            _rotationAngle -= angleToRotate;
            Debug.Log(_rotationAngle);
        }
        
        return true;
    }

    public void Activate(Vector3 startDirection, Vector3 endDirection)
    {
        _isActive = true;
        _rotationAngle = GlobalData.PlayerRotationAngleBeforeFlyToEdge;
        _startDirection = startDirection;
        _endDirection = endDirection;
        _endDirection = new Vector3(endDirection.z, 0, endDirection.x);
        //_endDirection.y = 0;
    }

    public void Deactivate()
    {
        _isActive = false;
    }
}