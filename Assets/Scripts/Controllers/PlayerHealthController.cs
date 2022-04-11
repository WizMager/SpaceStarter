using Interface;
using Model;

namespace Controllers
{
    public class PlayerHealthController : IExecute, IClean
    {
        private readonly PlayerModel _playerModel;
        private readonly PlayerMoveController _playerMoveController;
        private readonly float _multiplyDamageTake;
        private readonly float _startDamageTake;
        private readonly float _endDamageTake;

        private bool _isTakeDamage;
        private float _currentDamageTake;
        private float _currentDamageAccumulated;

        public PlayerHealthController(PlayerModel playerModel, PlayerMoveController playerMoveController, float multiplyDamageTake, 
            float startDamageTake, float endDamageTake)
        {
            _playerModel = playerModel;
            _playerMoveController = playerMoveController;
            _multiplyDamageTake = multiplyDamageTake;
            _startDamageTake = startDamageTake;
            _endDamageTake = endDamageTake;

            _currentDamageTake = startDamageTake;

            _playerMoveController.OnTakeDamage += TakeDamage;
            _playerMoveController.OnStopTakeDamage += StopTakeDamage;
        }

        private void StopTakeDamage()
        {
            _isTakeDamage = false;
        }

        private void TakeDamage(bool isTake)
        {
            _isTakeDamage = isTake;
        }

        public void Execute(float deltaTime)
        {
            if (_isTakeDamage)
            {
                _currentDamageAccumulated += _currentDamageTake;
                if (_currentDamageAccumulated >= 1)
                {
                    _playerModel.TakeDamage(1);
                    _currentDamageAccumulated -= 1f;
                }

                if (_currentDamageTake < _endDamageTake)
                {
                    _currentDamageTake += deltaTime * _multiplyDamageTake; 
                }
            }
            else
            {
                _currentDamageTake = _startDamageTake;
                _currentDamageAccumulated = 0;
            }
        }

        public void Clean()
        {
            _playerMoveController.OnTakeDamage -= TakeDamage;
        }
    }
}