using System;
using Interface;
using ScriptableData;
using UnityEngine;
using Utils;
using View;

namespace Controllers
{
    public class StateController : IExecute, IClean
    {
        public event Action<States> OnStateChange;

        private readonly AroundPlanetAngleCounter _flewAngle;
        private readonly FlyToCenterGravity _toCenterGravity;
        private readonly FlyToGravity _flyToGravity;

        public StateController(PlanetView planetView, PlayerView playerView, AllData data, GravityLittleView gravityLittle)
        {
            var playerTransform = playerView.transform;
            var planetTransform = planetView.transform;
            
            _flewAngle = new AroundPlanetAngleCounter(planetTransform, playerTransform, data.Planet.flyAngle, 
                this);
            _toCenterGravity = new FlyToCenterGravity(playerTransform, data.Planet.rotationInGravitySpeed,
                data.Planet.moveSpeedCenterGravity, planetTransform, this);
            _flyToGravity = new FlyToGravity(playerTransform, gravityLittle, this);

            _flewAngle.OnFinish += EndRotateAround;
            _toCenterGravity.OnFinish += EndToCenterGravity;
            _flyToGravity.OnFinish += EndFlyToGravityToPlanet;
        }

        private void EndFlyToGravityToPlanet()
        {
            OnStateChange?.Invoke(States.ToCenterGravity);
            Debug.Log(States.ToCenterGravity);
        }

        private void EndToCenterGravity()
        {
            OnStateChange?.Invoke(States.FlyAroundPlanet);
            Debug.Log(States.FlyAroundPlanet);
        }

        private void EndRotateAround()
        {
            OnStateChange?.Invoke(States.EdgeGravityFromPlanet);
            Debug.Log(States.EdgeGravityFromPlanet);
        }

        public void Execute(float deltaTime)
        {
            _flewAngle.FlewAngle();
            _toCenterGravity.FlyToCenter(deltaTime);
            _flyToGravity.Move(deltaTime);
        }
        
        public void Clean()
        {
            _flewAngle.OnFinish -= EndRotateAround;
            _toCenterGravity.OnFinish -= EndToCenterGravity;
            _flyToGravity.OnFinish -= EndFlyToGravityToPlanet;
        }
    }
}