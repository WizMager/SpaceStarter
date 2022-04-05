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

        private readonly FlewAngleCounter _flewAngle;
        private readonly FlyToCenterGravity _toCenterGravity;
        private readonly EdgeGravityToPlanet _edgeGravityToPlanet;
        private readonly EdgeGravityFromPlanet _edgeGravityFromPlanet;
        private readonly ArcFromPlanet _arcFromPlanet;

        public StateController(PlanetView planetView, PlayerView playerView, AllData data, GravityView gravityView, 
            GravityLittleView gravityLittleView)
        {
            var playerTransform = playerView.transform;
            var planetTransform = planetView.transform;
            
            _flewAngle = new FlewAngleCounter(planetTransform, playerTransform, data.Planet.flyAngle, 
                this);
            _toCenterGravity = new FlyToCenterGravity(playerTransform, data.Planet.rotationInGravitySpeed,
                data.Planet.moveSpeedCenterGravity, planetTransform, this);
            _edgeGravityToPlanet = new EdgeGravityToPlanet(playerTransform, gravityView, this, 
                data.Planet.moveSpeedToPlanet);
            _edgeGravityFromPlanet = new EdgeGravityFromPlanet(data.Planet.rotationTimeToEdgeGravity,
                data.Planet.moveSpeedToEdgeGravity, gravityLittleView, playerTransform, 
                this, planetTransform);
            _arcFromPlanet = new ArcFromPlanet(this, playerTransform, data.Planet.distanceToCenterRadiusArc,
                data.Planet.radiusArc, data.Planet.moveSpeedArcFromPlanet, data.Planet.rotationSpeedArcFromPlanet, 
                data.Planet.rotationSpeedRadius);

            _flewAngle.OnFinish += EndRotateAround;
            _toCenterGravity.OnFinish += EndToCenterGravity;
            _edgeGravityToPlanet.OnFinish += EndEdgeGravityToPlanetToPlanet;
            _edgeGravityFromPlanet.OnFinished += EndGravityFromPlanetFromPlanet;
            _arcFromPlanet.OnFinish += EndArcFlyFromPlanet;
        }

        private void EndArcFlyFromPlanet()
        {
            OnStateChange?.Invoke(States.ArcFlyCameraDown);
            Debug.Log(States.ArcFlyCameraDown);
        }

        private void EndGravityFromPlanetFromPlanet()
        {
            OnStateChange?.Invoke(States.ArcFlyFromPlanet);
            Debug.Log(States.ArcFlyFromPlanet);
        }

        private void EndEdgeGravityToPlanetToPlanet()
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
            _edgeGravityToPlanet.Move(deltaTime);
            _edgeGravityFromPlanet.Move();
            _arcFromPlanet.Move(deltaTime);
        }
        
        public void Clean()
        {
            _flewAngle.OnFinish -= EndRotateAround;
            _toCenterGravity.OnFinish -= EndToCenterGravity;
            _edgeGravityToPlanet.OnFinish -= EndEdgeGravityToPlanetToPlanet;
        }
    }
}