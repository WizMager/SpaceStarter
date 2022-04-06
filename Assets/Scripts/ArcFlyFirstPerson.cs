using System;
using Controllers;
using UnityEngine;
using Utils;
using View;

public class ArcFlyFirstPerson : IDisposable
{
      public event Action OnFinish;
    
      private readonly StateController _stateController;
      private readonly Transform _playerTransform;
      private readonly PlanetView _planetView;
      private readonly float _stopDistanceFromPlanet;
      private readonly int _percentPath;
      private readonly float _moveSpeed;
      
      private bool _isActive;
      private float _pathToFly;
      private float _flewPath;
      
      public ArcFlyFirstPerson(StateController stateController, Transform playerTransform, PlanetView planetView, 
            float stopDistanceFromPlanet, int percentPathCameraDown, float moveSpeed)
      {
          _stateController = stateController;
          _playerTransform = playerTransform;
          _planetView = planetView;
          _stopDistanceFromPlanet = stopDistanceFromPlanet;
          _percentPath = 100 - percentPathCameraDown;
          _moveSpeed = moveSpeed;
          
          _stateController.OnStateChange += ChangeState;
      }
      
      private void ChangeState(GameState gameState)
      {
          if (gameState == GameState.ArcFlyFirstPerson)
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
          var pathToCenterPlanet = Vector3.Distance(_playerTransform.position, _planetView.transform.position);
          var colliderRadius = _planetView.GetComponent<SphereCollider>().radius;
          _pathToFly = (pathToCenterPlanet - colliderRadius - _stopDistanceFromPlanet) / 100 * _percentPath;
          _flewPath = 0;
      }

      public void Move(float deltaTime)
      {
          if (!_isActive) return;
          if (_flewPath < _pathToFly)
          { 
              var distance = _moveSpeed * deltaTime;
              _playerTransform.Translate(_playerTransform.forward * distance, Space.World);
              _flewPath += distance;
          }
          else
          {
              OnFinish?.Invoke();
          }
      }
      
      public void Dispose()
      {
          _stateController.OnStateChange -= ChangeState;
      }
}