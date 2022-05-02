using System;
using UnityEngine;
using Utils;

namespace Model
{
    public class PlayerModel
    {
        public event Action<int> OnChangeScore;
        public event Action<int> OnChangeRocket;
        public event Action OnZeroHealth;
        public event Action OnZeroRocketLeft;

        private readonly int _basePlayerHealth;
        private int _playerHealth;
        private readonly int _basePlayerRocket;
        private int _touchedHouses;
        private int _playerRocket;
        private int _playerScore;
        private int _playerQuality;
        private int _playerTryCount;
        private int _scoreMultiply;

        public PlayerModel(int startPlayerHealth, int startPlayerRocket)
        {
            _basePlayerHealth = startPlayerHealth;
            _playerHealth = startPlayerHealth;
            _basePlayerRocket = startPlayerRocket;
            _playerRocket = startPlayerRocket;
            _touchedHouses = 0;
            _playerScore = 0;
            _playerTryCount = 1;
            _scoreMultiply = 1;
        }

        public void ShootRocket()
        {
            _playerRocket--;
            OnChangeRocket?.Invoke(_playerRocket);
            if (_playerRocket <= 0)
            {
                OnZeroRocketLeft?.Invoke();
            }
        }
        
        public void TakeDamage(int damage)
        {
            _playerHealth -= damage;
            if (_playerHealth >= 0)
            {
                Debug.Log($"Current health: {_playerHealth}");
            }
            else
            {
                Debug.Log($"Current health: {_playerHealth}");
                OnZeroHealth?.Invoke();
            }
        }
        
        public void TouchHouse(FloorType floorType)
        {
            switch (floorType)
            {
                case FloorType.GlassFloor:
                    _playerRocket += 1;
                    OnChangeRocket?.Invoke(_playerRocket);
                    break;
                case FloorType.SimpleFloor:
                    _touchedHouses += 1;
                    _playerScore = _touchedHouses * _scoreMultiply;
                    OnChangeScore?.Invoke(_playerScore);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(floorType), floorType, null);
            }
        }

        public void SetQuality(float wholeTime, float touchTime)
        {
            _playerQuality = 100 - Mathf.RoundToInt(touchTime * 100 / wholeTime);
        }

        public void SetScoreMultiply()
        {
            _scoreMultiply = _playerRocket;
        }
        
        public int[] GetValueToFinalScreen()
        {
            _playerQuality = _playerHealth;
            return new[] {_playerScore * _scoreMultiply, _playerTryCount, _playerQuality};
        }

        public void ResetRound()
        {
        _playerHealth = _basePlayerHealth;
        _playerRocket = _basePlayerRocket;
        _touchedHouses = 0;
        _playerScore = 0;
        _playerQuality = 0;
        _playerTryCount = 1;
        _scoreMultiply = 1;
        OnChangeRocket?.Invoke(_playerRocket);
        OnChangeScore?.Invoke(_playerScore);
        }
    }
}