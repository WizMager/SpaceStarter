using UnityEngine;

public class FlyToNextPlanet
{
      private Vector3 _direction;
      private float _moveSpeed;
      private float _speedRotation;

      private bool _isActive;
      private bool _isRotation = true;
      private bool _isFirstMove;
      private float _angleToRotation;

      public FlyToNextPlanet(float moveSpeed, float speedRotation)
      {
          _moveSpeed = moveSpeed;
          _speedRotation = speedRotation;
      }

      public void Move(Transform player, float deltaTime)
      {
          if (_isActive)
          {
              if (_isFirstMove)
              {
                  _angleToRotation = Vector3.Angle(player.forward, _direction);
                  Debug.Log(_angleToRotation);
                  _isFirstMove = false;
              }
              
              if (_angleToRotation >= 0)
              {
                  var angleToRotate = deltaTime * _speedRotation;
                  player.Rotate(-Vector3.forward, angleToRotate);
                  _angleToRotation -= angleToRotate;
              }
              else
              {
                  player.Translate(-player.forward * deltaTime * _moveSpeed);
              }
          }
      }

      public void SetActive(bool isActive)
      {
          _isActive = isActive;
      }

      public void SetDirection(Vector3 direction)
      {
          _direction = direction;
          _isFirstMove = true;
      }
}