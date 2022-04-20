using System;
using Interface;
using Model;
using ScriptableData;
using UnityEngine;
using Utils;
using View;

namespace Controllers
{
    public class PlayerMoveController: IExecute, IClean
    {
        public event Action<bool> OnTakeDamage;
        public event Action OnStopTakeDamage;
        
        private readonly RotationAroundPlanet _rotationAroundPlanet;
        private readonly UpAndDownAroundPlanet _upAndDownAroundPlanet;
        private readonly StateController _stateController;
        private readonly PlayerModel _playerModel;
        private readonly ShipView _shipView;
        private readonly Transform _planetTransform;
        private readonly AllData _data;

        private bool _isActive;
        private float _wholeFlyTime;
        
        public PlayerMoveController(StateController stateController, ShipView shipView, AllData data, IUserInput<Vector3>[] touchInput, 
            PlanetView planetView, GravityLittleView gravityView, PlayerModel playerModel)
        {
            _shipView = shipView;
             var playerTransform = shipView.transform;
             _planetTransform = planetView.transform;
             _data = data;

             _rotationAroundPlanet = new RotationAroundPlanet(data.Planet.startSpeedRotationAroundPlanet, playerTransform, _planetTransform);
            _upAndDownAroundPlanet = new UpAndDownAroundPlanet(data.Planet.startEngineForce, data.Planet.startGravity,
                playerTransform, planetView, gravityView, touchInput, data.Planet.maxGravity,
                data.Planet.maxEngineForce, data.Planet.gravityAcceleration, data.Planet.engineAcceleration, 
                data.Player.cooldownTakeDamage, data.Player.thresholdAfterTouchPlanetGravity);
            _stateController = stateController;
            _playerModel = playerModel;

            _stateController.OnStateChange += StateChange;
            _upAndDownAroundPlanet.OnTakeDamage += TakeDamage;
        }

        private void StateChange(GameState gameState)
        {
            switch (gameState)
            {
                case GameState.ToCenterGravity:
                    _shipView.SeparateTurbine(_planetTransform, _data);
                    break;
                case GameState.FlyAroundPlanet:
                    _shipView.StartFlyTurbine();
                    _isActive = true;
                    _upAndDownAroundPlanet.Active(true);
                    break;
                case GameState.EdgeGravityFromPlanet:
                    _isActive = false;
                    _upAndDownAroundPlanet.Active(false);
                    OnStopTakeDamage?.Invoke();
                    _playerModel.SetQuality(_wholeFlyTime, _upAndDownAroundPlanet.GetPlayerTouchTime);
                    _playerModel.SetScoreMultiply();
                    _wholeFlyTime = 0;
                    break;
                case GameState.FlyAway:
                    _isActive = false;
                    _upAndDownAroundPlanet.Active(false);
                    _shipView.ConnectTurbine();
                    break;
                default:
                    _isActive = false;
                    _upAndDownAroundPlanet.Active(false);
                    OnStopTakeDamage?.Invoke();
                    break;
            }
        }

        private void TakeDamage(bool isTake)
        {
            OnTakeDamage?.Invoke(isTake);
        }
        
        public void Execute(float deltaTime)
        {
            if (!_isActive) return;
            _wholeFlyTime += deltaTime;
            _rotationAroundPlanet.Move(deltaTime);
            _upAndDownAroundPlanet.Move(deltaTime);
        }

        public void Clean()
        {
            _stateController.OnStateChange -= StateChange;
        }
    }
}