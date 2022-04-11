﻿using Interface;
using Model;
using UnityEngine;
using Utils;
using View;
using Object = UnityEngine.Object;

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

            _touch[(int) TouchInputState.InputTouchDown].OnChange += TouchDown;
            _touch[(int) TouchInputState.InputTouchUp].OnChange += TouchUp;
            _stateController.OnStateChange += ChangeState;
        }

        private void ChangeState(GameState state)
        {
            _isActive = state == GameState.ShootPlanet;
        }

        private void Shoot(Vector3 touchPosition)
        {
            var ray = _camera.ScreenPointToRay(touchPosition);
            var raycastHit = new RaycastHit[1];
            Physics.RaycastNonAlloc(ray, raycastHit, _camera.farClipPlane, GlobalData.LayerForAim);
            var cameraTransform = _camera.transform; 
            var missile = Object.Instantiate(_missile, _missileStartPosition.position, cameraTransform.rotation);
            var missileView = missile.GetComponent<MissileView>(); 
            missileView.SetTargetPoint(raycastHit[0].point);
            missileView.SetPlanetTransform(_planet);
            _playerModel.ShootRocket();
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