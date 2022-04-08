using System;
using Controllers;
using UnityEngine;
using Utils;

namespace StateClasses
{
     public class ArcFlyRadius : IDisposable
     {
          public event Action OnFinish;

          private readonly StateController _stateController;
          private readonly Transform _playerTransform;
          private readonly float _rotationSpeed;
          private readonly ArcFromPlanet _arcFromPlanet;

          private bool _isActive;
          private Vector3 _centerCircle;
          private float _rotatedDistance;

          public ArcFlyRadius(StateController stateController, Transform playerTransform, float rotationSpeed, ArcFromPlanet arcFromPlanet)
          {
               _stateController = stateController;
               _playerTransform = playerTransform;
               _rotationSpeed = rotationSpeed;
               
               _arcFromPlanet = arcFromPlanet;

               _stateController.OnStateChange += ChangeState;
               _arcFromPlanet.OnCenterRadiusPosition += SetCenterRadiusPosition;
          }

          private void SetCenterRadiusPosition(Vector3 position)
          {
               _centerCircle = position;
          }

          private void ChangeState(GameState gameState)
          {
               if (gameState == GameState.ArcFlyRadius)
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
               _rotatedDistance = 0;
          }

          public void Move(float deltaTime)
          {
               if (!_isActive) return;
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

          public void Dispose()
          {
               _stateController.OnStateChange -= ChangeState;
               _arcFromPlanet.OnCenterRadiusPosition -= SetCenterRadiusPosition;
          }
     }
}