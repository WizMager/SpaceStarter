using Interface;
using ScriptableData;
using UnityEngine;
using Utils;
using View;

namespace Controllers
{
    public class PlayerMoveController: IExecute, IClean
    {
        private readonly RotationAroundPlanet _rotationAroundPlanet;
        private readonly UpAndDownAroundPlanet _upAndDownAroundPlanet;
        private readonly StateController _stateController;

        private bool _isActive;
        
        public PlayerMoveController(PlayerView playerView, AllData data, IUserInput<Vector3>[] touchInput, 
            PlanetView planetView, GravityLittleView gravityView, StateController stateController)
        {
             var playerTransform = playerView.transform;
             var planetTransform = planetView.transform;
             
             _rotationAroundPlanet = new RotationAroundPlanet(data.Planet.startSpeedRotationAroundPlanet, playerTransform, planetTransform);
            _upAndDownAroundPlanet = new UpAndDownAroundPlanet(data.Planet.startEngineForce, data.Planet.startGravity,
                playerTransform, planetView, gravityView, touchInput, data.Planet.maxGravity,
                data.Planet.maxEngineForce, data.Planet.gravityAcceleration, data.Planet.engineAcceleration);
            _stateController = stateController;

            _stateController.OnStateChange += StateChange;
        }

        private void StateChange(GameState gameState)
        {
            if (gameState == GameState.FlyAroundPlanet)
            {
                _isActive = true;
                _upAndDownAroundPlanet.Active(true);
            }
            else
            {
                _isActive = false;
                _upAndDownAroundPlanet.Active(false); 
            }
        }

        public void Execute(float deltaTime)
        {
            if (!_isActive) return;
            _rotationAroundPlanet.Move(deltaTime);
            _upAndDownAroundPlanet.Move(deltaTime);
        }

        public void Clean()
        {
            _stateController.OnStateChange -= StateChange;
        }
    }
}