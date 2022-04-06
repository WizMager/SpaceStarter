using System;
using Controllers;
using UnityEngine;
using Utils;

public class ArcFromPlanet : IDisposable
{
     public event Action OnFinish;
     
     private readonly StateController _stateController;
     private readonly Transform _playerTransform;
     private readonly float _distanceToCenterCircle;
     private readonly float _radius;
     private readonly float _moveSpeed;
     private readonly float _rotationSpeedArc;
     private readonly float _rotationSpeed;

     private bool _isActive;
     private Vector3 _centerCircle;
     private float _angleRotateToSurface;
     private float _distanceToSurfaceCircle;
     private bool _isRotated;
     private float _flewDistance;
     private float _rotatedDistance;
     private float _currentAngleRotated;

     public ArcFromPlanet(StateController stateController,Transform playerTransform, float distanceToCenterCircle, 
          float radius, float moveSpeed, float rotationSpeedArc, float rotationSpeed)
     {
          _stateController = stateController;
          _playerTransform = playerTransform;
          _distanceToCenterCircle = distanceToCenterCircle;
          _radius = radius;
          _moveSpeed = moveSpeed;
          _rotationSpeedArc = rotationSpeedArc;
          _rotationSpeed = rotationSpeed;

          _stateController.OnStateChange += ChangeState;
     }

     private void ChangeState(States state)
     {
          if (state == States.ArcFlyFromPlanet)
          {
               _isActive = true;
               SetupAndCalculate();
          }
          else
          {
               _isActive = false;
          }
     }

     private void SetupAndCalculate()
     {
          var ray = new Ray(_playerTransform.position, _playerTransform.forward);
          _centerCircle = ray.GetPoint(_distanceToCenterCircle);
          _angleRotateToSurface = Mathf.Tan(_radius / _distanceToCenterCircle) * Mathf.Rad2Deg;
          _distanceToSurfaceCircle = Mathf.Sqrt(_radius * _radius + _distanceToCenterCircle * _distanceToCenterCircle);
          _flewDistance = 0;
          _rotatedDistance = 0;
          _currentAngleRotated = 0;
          _isRotated = false;
     }

     public void Move(float deltaTime)
     {
          if (!_isActive) return;
          if (_isRotated)
          {
               if (_flewDistance < _distanceToSurfaceCircle)
               {
                    var distance = deltaTime * _moveSpeed;
                    _playerTransform.Translate(_playerTransform.forward * distance, Space.World);
                    _flewDistance += distance;
               }
               else
               {
                    if (_rotatedDistance < 180f)
                    {
                         if (_rotatedDistance == 0)
                         {
                              _playerTransform.LookAt(_centerCircle);  
                              _playerTransform.Rotate(_playerTransform.up, -90f);
                         }
                         var rotation = deltaTime * _rotationSpeed;
                         _playerTransform.RotateAround(_centerCircle, Vector3.up, rotation);
                         _rotatedDistance += rotation;
                    }
                    else
                    {
                         OnFinish?.Invoke();
                    }
               }  
          }
          else
          {
               if (_currentAngleRotated < _angleRotateToSurface)
               {
                    var angle = _rotationSpeedArc * deltaTime;
                    _playerTransform.Rotate(_playerTransform.up, -angle);
                    _currentAngleRotated += angle;
               }
               else
               {
                    _isRotated = true;
               }
          }
     }

     public void Dispose()
     {
          _stateController.OnStateChange -= ChangeState; 
     }
}