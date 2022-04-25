using System;
using System.Collections;
using Controllers;
using UnityEngine;
using Utils;
using View;

namespace StateClasses
{
    public class FlyToCenterGravity : IDisposable
    {
        public event Action OnFinish;

        private readonly float _rotationSpeedGravity;
        private readonly float _moveSpeedGravity;
        private readonly Transform _shipTransform;
        private readonly Transform _planet;
        private readonly StateController _stateController;
        private readonly ShipView _shipView;
    
        private Vector3 _direction;
        private float _pathCenter;
        private Quaternion _finishRotation;
        private readonly SphereCollider _planetCollider;

        public FlyToCenterGravity(ShipView shipView, float rotationSpeedGravity, float moveSpeedGravity, 
            Transform planet, StateController stateController)
        {
            _rotationSpeedGravity = rotationSpeedGravity;
            _moveSpeedGravity = moveSpeedGravity;
            _shipTransform = shipView.transform;
            _planet = planet;
            _stateController = stateController;
            _planetCollider = _planet.GetComponent<SphereCollider>();
            _shipView = shipView;

            _stateController.OnStateChange += StateChange;
        }

        private void StateChange(GameState gameState)
        {
            switch (gameState)
            {
                case GameState.ToCenterGravity:
                    var playerPosition = _shipTransform.position;
                    var planetPosition = _planet.position;
                    _direction = (planetPosition - playerPosition).normalized;
                    _pathCenter = (Vector3.Distance(playerPosition, planetPosition) - _planetCollider.radius) / 2;
                    _finishRotation = Quaternion.Euler(0, 90, 90);
                    _shipView.StartCoroutine(RotateAndMove());
                    break;
                default:
                    _shipView.StopCoroutine(RotateAndMove());
                    break;
            }
        }

        private IEnumerator RotateAndMove()
        {
            for (float i = 0; i < _pathCenter; )
            {
                var deltaTime = Time.deltaTime;
                var moveStep = deltaTime * _moveSpeedGravity;
                _shipTransform.Translate(_direction * moveStep, Space.World);
                i += moveStep;
                
                var stepAngle = deltaTime * _rotationSpeedGravity;
                var currentRotation = _shipTransform.rotation;
                _shipTransform.rotation = Quaternion.Lerp(currentRotation, _finishRotation, stepAngle);

                yield return null;
            }
            OnFinish?.Invoke();
        }

        public void Dispose()
        {
            _stateController.OnStateChange -= StateChange;
        }
    }
}