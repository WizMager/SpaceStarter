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
            if (_isTappedDown)
            {
                  Vector3.RotateTowards(_player.forward, _moveDirection, 180, 0);
            }
            var rotationAngle = Vector3.Angle(_player.forward, _moveDirection);
            _player.Rotate(_player.up, rotationAngle);
      }
      
      public void Moving(float deltaTime)
      {
            if (_isFlying)
            {
                  _player.Translate(_moveDirection.normalized * deltaTime);  
            }
      }

      public void PlayerTapPointSet(Vector3 tapPoint)
      {
            if (_isTappedUp) return;
            _isFlying = true;
            _isTappedUp = true;
            _moveDirection = (tapPoint - _player.position);
            _moveDirection.y = 0;
            RotatePlayer();
      }
}