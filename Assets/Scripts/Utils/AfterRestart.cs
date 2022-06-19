using System.Collections.Generic;
using Controllers;
using ScriptableData;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Utils
{
    public class AfterRestart : MonoBehaviour
    {
        [SerializeField] private int _maxNumberForRandom;
        [SerializeField] private AllData _allData;
        private bool _firstTimeLevelLaunch = true;
        private int _colorNumbers;
        private StateController _stateController;
        private Dictionary<int, Dictionary<int, List<Material>>> _preparedMaterials;
        private MaterialsTake _materialsTake;

        public Dictionary<int, List<Transform>> SpawnedTransforms { get; set; }

        public bool FirstTimeLevelLaunch => _firstTimeLevelLaunch;
        public Dictionary<int, Dictionary<int, List<Material>>> PrepareMaterials => _preparedMaterials;
        
        private void Awake()
        {
            var otherAfterRestart = FindObjectsOfType<AfterRestart>();
            if (otherAfterRestart.Length > 1)
            {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            _materialsTake = new MaterialsTake(_allData.Materials);
        }

        public void TakeStateController(StateController stateController)
        {
            _materialsTake = new MaterialsTake(_allData.Materials);
            _stateController = stateController;
            _stateController.OnStateChange += ChangeState;
            if (_firstTimeLevelLaunch)
            {
                ColorNumberRandomization();
            }
            _preparedMaterials = _materialsTake.TakeRandomMaterials(_colorNumbers);
        }

        private void ChangeState(GameState gameState)
        {
            switch (gameState)
            {
                case GameState.Restart:
                    _firstTimeLevelLaunch = false;
                    _stateController.OnStateChange -= ChangeState;
                    break;
                case GameState.NextLevel:
                    _firstTimeLevelLaunch = true;
                    _stateController.OnStateChange -= ChangeState;
                    break;
            }
        }

        private void ColorNumberRandomization()
        {
            _colorNumbers = Random.Range(0, _maxNumberForRandom);
        }
    }
}