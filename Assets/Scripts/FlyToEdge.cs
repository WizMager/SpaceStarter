using UnityEngine;

public class FlyToEdge
{
    private readonly float _speedRotation;
    private float _rotationAngle;
    private bool _isActive;

    public FlyToEdge(float speedRotationToGravityEdge)
    {
        _speedRotation = speedRotationToGravityEdge;
    }
    
    public bool FlyingToEdge(Transform playerTransform, float deltaTime)
    {
        if (!_isActive) return false;
        
        if (_rotationAngle <= 0)
        {
            playerTransform.Translate(-playerTransform.forward * deltaTime);
        }
        else
        {
            var angleToRotate = _speedRotation * deltaTime;
            playerTransform.Rotate(Vector3.forward * angleToRotate);
            _rotationAngle -= angleToRotate;
        }
        return true;
    }

    public void Activator(bool isActive)
    {
        _isActive = isActive;
        if (_isActive)
        {
            _rotationAngle = GlobalData.PlayerRotationAngleBeforeFlyToEdge;
        }
    }
}