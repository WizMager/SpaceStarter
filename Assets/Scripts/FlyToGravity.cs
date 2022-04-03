using System;
using Controllers;
using UnityEngine;
using Utils;
using View;

public class FlyToGravity
{
    public event Action OnFinish;
    
     private readonly Transform _playerTransform;
     private GravityLittleView _gravityView;
     private readonly StateController _stateController;

     private bool _isActive = true;
     
     public FlyToGravity(Transform playerTransform, GravityLittleView gravityView, StateController stateController)
     {
          _playerTransform = playerTransform;
          _gravityView = gravityView;
          _stateController = stateController;

          _gravityView.OnPlayerGravityEnter += OnGravityEnter;
          _stateController.OnStateChange += ChangeState;
     }

     private void ChangeState(States state)
     {
         _isActive = state == States.EdgeGravityToPlanet;
     }

     private void OnGravityEnter()
     {
         if (!_isActive) return;
         OnFinish?.Invoke();
     }

     public void Move(float deltaTime)
     {
         if (!_isActive) return;
         _playerTransform.Translate(_playerTransform.forward * deltaTime * 10f, Space.World); 
     }
}