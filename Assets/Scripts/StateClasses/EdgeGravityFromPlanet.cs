using System;
using System.Collections;
using Controllers;
using UnityEngine;
using Utils;
using View;

namespace StateClasses
{
    public class EdgeGravityFromPlanet : IDisposable
    {
        public event Action OnFinished;
        private readonly float _moveSpeed;
        private readonly float _gravityHalfSize;
        private readonly ShipView _shipView;
        private readonly Transform _shipTransform;
        private readonly StateController _stateController;
        private readonly Transform _planetTransform;
        
        private Quaternion _finishRotation;

        public EdgeGravityFromPlanet(float moveSpeed, float gravityHalfSize, ShipView shipView, StateController stateController, Transform planetTransform)
        {
            _moveSpeed = moveSpeed;
            _gravityHalfSize = gravityHalfSize;
            _shipView = shipView;
            _shipTransform = shipView.transform;
            _stateController = stateController;
            _planetTransform = planetTransform;
            
            _stateController.OnStateChange += ChangeState;
        }

        private void ChangeState(GameState gameState)
        {
            switch (gameState)
            {
                case GameState.EdgeGravityFromPlanet:
                    var correctPlayerPosition =
                        new Vector3(_shipTransform.position.x, 0, _shipTransform.position.z);
                    _shipTransform.position = correctPlayerPosition;
                    _shipView.StartCoroutine(RotateAndMove());
                    break;
                default:
                    _shipView.StopCoroutine(RotateAndMove());
                    break;
            }
        }

        private IEnumerator RotateAndMove()
        {
            var moveDirection = (_shipTransform.position - _planetTransform.position).normalized;
            var finishRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0, moveDirection.z));
            var startRotation = _shipTransform.rotation;
            var distanceToEdge = _gravityHalfSize - Vector3.Distance(_planetTransform.transform.position, _shipTransform.position);
            for (float i = 0; i < distanceToEdge;)
            {
                var moveStep = Time.deltaTime * _moveSpeed;
                i += moveStep;
                _shipTransform.Translate(moveDirection * moveStep, Space.World);
                _shipTransform.rotation = Quaternion.Lerp(startRotation, finishRotation, i / distanceToEdge);
                yield return null;
            }
            OnFinished?.Invoke();
        }

        public void Dispose()
        {
            _stateController.OnStateChange -= ChangeState;
        }
    }
}