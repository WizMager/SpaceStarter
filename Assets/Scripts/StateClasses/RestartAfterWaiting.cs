using System;
using System.Collections;
using Controllers;
using UnityEngine;
using Utils;
using View;

namespace StateClasses
{
    public class RestartAfterWaiting : IDisposable
    {
        public event Action OnFinish;
        private readonly StateController _stateController;
        private readonly float _waitAfterRestart;
        private readonly PlanetView _planetView;

        public RestartAfterWaiting(StateController stateController, float waitAfterRestart, PlanetView planetView)
        {
            _stateController = stateController;
            _waitAfterRestart = waitAfterRestart;
            _planetView = planetView;

            _stateController.OnStateChange += ChangeState;
        }

        private void ChangeState(GameState gameState)
        {
            switch (gameState)
            {
                case GameState.Restart:
                    _planetView.StartCoroutine(WaitAfterRestart());
                    break;
                default:
                    _planetView.StopCoroutine(WaitAfterRestart());
                    break;
            }
        }
        
        private IEnumerator WaitAfterRestart()
        {
            for (float i = 0; i < _waitAfterRestart; i+=Time.deltaTime)
            {
                yield return null;
            }
            OnFinish?.Invoke();
        }

        public void Dispose()
        {
            _stateController.OnStateChange -= ChangeState;
        }
    }
}