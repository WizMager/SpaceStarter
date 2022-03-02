using UnityEngine;

public class PlayerMoveNextPlanet
{
      private bool _isFlying;
      private bool _isTappedDown;
      private bool _isTappedUp;
      private Vector3 _moveDirection;
      private readonly Transform _player;

      public PlayerMoveNextPlanet(Transform player)
      {
            _player = player;
      }

      private void RotatePlayer()
      {
            var rotationAngle = Vector3.Angle(_player.forward, _moveDirection);
            _player.Rotate(_player.up, rotationAngle);
      }
      
      public void Moving(float deltaTime)
      {
            if (_isFlying)
            {
                  _player.Translate(_moveDirection * deltaTime);  
            }
      }

      public void PlayerTapPointSet(Vector3 tapPoint)
      {
            if (_isTappedUp) return;
            _isFlying = true;
            _isTappedUp = true;
            var lastY = _player.position.y;
            _moveDirection = (tapPoint - _player.position).normalized;
            _moveDirection.y = 0;
            RotatePlayer();
      }

      public void FirstTapDeactivation()
      {
            _isTappedUp = false;
      }
}