using System;
using Controllers;
using UnityEngine;
using Utils;

public class LookToPlanet : IDisposable
{
       public event Action OnFinish;
       private readonly Transform _playerTransform;
       private readonly Transform _planetTransform;
       private readonly float _rotationSpeed;
       private readonly StateController _stateController;
       private float _angle;
       private float _currentAngle;
       private bool _isActive;

       public LookToPlanet(Transform playerTransform, Transform planetTransform, float rotationSpeed, StateController stateController)
       {
              _playerTransform = playerTransform;
              _planetTransform = planetTransform;
              _rotationSpeed = rotationSpeed;
              _stateController = stateController;

              _stateController.OnStateChange += StateChange;
       }

       private void StateChange(States state)
       {
              if (state == States.LookToPlanet)
              {
                     _isActive = true;
                     _angle = Vector3.Angle(_playerTransform.forward, _planetTransform.position - _playerTransform.position);
              }
              else
              {
                     _isActive = false;
              }
       }

       public void Rotate(float deltaTime)
       {
              if (!_isActive) return;
              if (_currentAngle < _angle)
              {
                     var angleRotate = deltaTime * _rotationSpeed;
                     _playerTransform.transform.Rotate(_playerTransform.transform.up, angleRotate);
                     _currentAngle += angleRotate;
              }
              else
              {
                     _currentAngle = 0;
                     OnFinish?.Invoke();
                     _playerTransform.gameObject.SetActive(false);
              }
       }

       public void Dispose()
       {
              _stateController.OnStateChange -= StateChange;   
       }
}