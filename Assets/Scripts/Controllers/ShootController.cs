using System.Collections.Generic;
using Interface;
using Model;
using UnityEngine;
using Utils;
using View;
using MissilePoolUsing = Pool.MissilePool;

namespace Controllers
{
    public class ShootController : IController, IClean
    {
        private readonly IUserInput<Vector3>[] _touch;
        private readonly Camera _camera;
        private readonly Transform _planet;
        private readonly Transform _missileStartPosition;
        private readonly GameObject _missile;
        private readonly StateController _stateController;
        private readonly PlayerModel _playerModel;
        private readonly MissilePoolUsing _missilePool;
        private List<MissileView> _activeMissile;

        private bool _isActive;
        private Vector3 _touchDownPosition;

        public ShootController(IUserInput<Vector3>[] touch, Camera camera, ScriptableData.AllData data, Transform missileStartPosition, 
            Transform planet, StateController stateController, PlayerModel playerModel)
        {
            _touch = touch;
            _camera = camera;
            _planet = planet;
            _missile = data.Missile.missilePrefab;
            _missileStartPosition = missileStartPosition;
            _stateController = stateController;
            _playerModel = playerModel;

            _missilePool = new MissilePoolUsing(_missile, _missileStartPosition, 5);
            _activeMissile = new List<MissileView>();

            _touch[(int) TouchInputState.InputTouchDown].OnChange += TouchDown;
            _touch[(int) TouchInputState.InputTouchUp].OnChange += TouchUp;
            _stateController.OnStateChange += ChangeState;
        }

        private void ChangeState(GameState state)
        {
            switch (state)
            {
                case GameState.ShootPlanet:
                    _isActive = true;
                    break;
                case GameState.Restart:
                    foreach (var missileView in _activeMissile)
                    {
                        _missilePool.Push(missileView);
                    }
                    _isActive = false;
                    break;
                default:
                    _isActive = false;
                    break;
            }
        }

        private void Shoot(Vector3 touchPosition)
        {
            var ray = _camera.ScreenPointToRay(touchPosition);
            var raycastHit = new RaycastHit[1];
            Physics.RaycastNonAlloc(ray, raycastHit, _camera.farClipPlane, GlobalData.LayerForExplosion);
            var missileView = _missilePool.Pop();
            _activeMissile.Add(missileView);
            missileView.transform.SetPositionAndRotation(_missileStartPosition.position, _missileStartPosition.rotation);
            missileView.OnFlyEnd += MissileFlyEnded;
            missileView.SetTarget(raycastHit[0].point, _planet);
            _playerModel.ShootRocket();
        }

        private void MissileFlyEnded(MissileView obj)
        {
            _activeMissile.Remove(obj);
            obj.OnFlyEnd -= MissileFlyEnded;
            _missilePool.Push(obj);
        }

        private void TouchDown(Vector3 position)
        {
            if (!_isActive) return;

            _touchDownPosition = position;
        }

        private void TouchUp(Vector3 position)
        {
            if (!_isActive) return;

            if (_touchDownPosition != position) return;
        
            Shoot(position);
        }
    
        public void Clean()
        {
            _touch[(int) TouchInputState.InputTouchDown].OnChange -= TouchDown;
            _touch[(int) TouchInputState.InputTouchUp].OnChange -= TouchUp;
            _stateController.OnStateChange -= ChangeState;
        }
    }
}