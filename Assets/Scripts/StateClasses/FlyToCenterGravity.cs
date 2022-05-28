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
        
        private readonly float _moveSpeedGravity;
        private readonly Transform _shipTransform;
        private readonly Transform _planet;
        private readonly StateController _stateController;
        private readonly ShipView _shipView;

        private readonly float _planetRadius;

        public FlyToCenterGravity(ShipView shipView, float moveSpeedGravity, 
            Transform planet, StateController stateController)
        {
            _moveSpeedGravity = moveSpeedGravity;
            _shipTransform = shipView.transform;
            _planet = planet;
            _stateController = stateController;
            _planetRadius = _planet.GetComponent<SphereCollider>().radius;
            _shipView = shipView;

            _stateController.OnStateChange += StateChange;
        }

        private void StateChange(GameState gameState)
        {
            switch (gameState)
            {
                case GameState.ToCenterGravity:
                    _shipView.StartCoroutine(RotateAndMove());
                    break;
                default:
                    _shipView.StopCoroutine(RotateAndMove());
                    break;
            }
        }

        private IEnumerator RotateAndMove()
        {
            var playerPosition = _shipTransform.position;
            var planetPosition = _planet.position;
            var direction = (planetPosition - playerPosition).normalized;
            var pathCenter = (Vector3.Distance(playerPosition, planetPosition) - _planetRadius) / 2;
            var finishRotation = Quaternion.Euler(0, 90, 90);
            var startRotation = _shipTransform.rotation;
            for (float i = 0; i < pathCenter; )
            {
                var moveStep = Time.deltaTime * _moveSpeedGravity;
                i += moveStep;
                _shipTransform.Translate(direction * moveStep, Space.World);
                _shipTransform.rotation = Quaternion.Lerp(startRotation, finishRotation, i / pathCenter);

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