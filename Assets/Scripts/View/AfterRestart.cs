using System;
using System.Collections.Generic;
using Controllers;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace View
{
    public class AfterRestart : MonoBehaviour
    {
        public bool firstTimeLevelLaunch;
        private Material[] _buildings;
        private Material[] _trees;
        private Material _cheliks;
        private Material _grass;
        private Material _ground;
        private Material _water;
        private Material _atmosphere;
        private List<int> _colorNumbers;

        private StateController _stateController;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            firstTimeLevelLaunch = true;
            _colorNumbers = new List<int>();
        }

        public void TakeStateController(StateController stateController)
        {
            _stateController = stateController;
            _stateController.OnStateChange += ChangeState;
        }

        private void ChangeState(GameState gameState)
        {
            switch (gameState)
            {
                case GameState.Restart:
                    firstTimeLevelLaunch = false;
                    _stateController.OnStateChange -= ChangeState;
                    break;
                case GameState.NextLevel:
                    firstTimeLevelLaunch = true;
                    _stateController.OnStateChange -= ChangeState;
                    break;
            }
        }

        private void ColorNumberRandomization()
        {
            
        }
    }
}