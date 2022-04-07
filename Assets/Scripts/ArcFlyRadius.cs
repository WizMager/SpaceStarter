using System;
using Controllers;
using UnityEngine;
using Utils;

public class ArcFlyRadius : IDisposable
{
     public event Action OnFinish;

     private readonly StateController _stateController;
     private readonly Transform _playerTransform;
     private readonly GameObject _gravityView;
     private readonly GameObject _gravityLittleView;
     private readonly SphereCollider _planetCollider;
     private readonly float _rotationSpeed;
     private readonly ArcFromPlanet _arcFromPlanet;

     private bool _isActive;
     private Vector3 _centerCircle;
     private float _rotatedDistance;

     public ArcFlyRadius(StateController stateController, Transform playerTransform, float rotationSpeed, 
          GameObject gravityView, GameObject gravityLittleView, SphereCollider planetCollider, ArcFromPlanet arcFromPlanet)
     {
          _stateController = stateController;
          _playerTransform = playerTransform;
          _rotationSpeed = rotationSpeed;
          _gravityView = gravityView;
          _gravityLittleView = gravityLittleView;
          _planetCollider = planetCollider;
          _arcFromPlanet = arcFromPlanet;

          _stateController.OnStateChange += ChangeState;
          _arcFromPlanet.CenterRadiusPosition += SetCenterRadiusPosition;
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
               _gravityView.SetActive(false);
               _gravityLittleView.SetActive(false);
               _planetCollider.enabled = false;
               OnFinish?.Invoke();
          }
     }

     public void Dispose()
     {
          _stateController.OnStateChange -= ChangeState;
     }
}