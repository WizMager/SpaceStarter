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
        public event Action<GameState> OnStateChange;

        private readonly FlewAngleCounter _flewAngle;
        private readonly FlyToCenterGravity _toCenterGravity;
        private readonly EdgeGravityToPlanet _edgeGravityToPlanet;
        private readonly EdgeGravityFromPlanet _edgeGravityFromPlanet;
        private readonly ArcFromPlanet _arcFromPlanet;
        private readonly ArcCameraDown _arcCameraDown;
        private readonly ArcFlyFirstPerson _arcFlyFirstPerson;

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
            _arcCameraDown = new ArcCameraDown(this, playerTransform, planetView,
                data.Planet.stopDistanceFromPlanetSurface, data.Planet.percentOfCameraDownPath, 
                data.Planet.moveSpeedArcCameraDown, data.Planet.rotationSpeedArcFromPlanet);
            _arcFlyFirstPerson = new ArcFlyFirstPerson(this, playerTransform, planetView,
                data.Planet.stopDistanceFromPlanetSurface,
                data.Planet.percentOfCameraDownPath, data.Planet.moveSpeedArcFirstPerson);

            _flewAngle.OnFinish += EndRotateAround;
            _toCenterGravity.OnFinish += EndToCenterGravity;
            _edgeGravityToPlanet.OnFinish += EndEdgeGravityToPlanetToPlanet;
            _edgeGravityFromPlanet.OnFinished += EndGravityFromPlanetFromPlanet;
            _arcFromPlanet.OnFinish += EndArcFlyFromPlanet;
            _arcCameraDown.OnFinish += EndArcCameraDown;
            _arcFlyFirstPerson.OnFinish += EndArcFlyFirstPerson;
        }

        private void EndArcFlyFirstPerson()
        {
            OnStateChange?.Invoke(GameState.ShootPlanet);
            Debug.Log(GameState.ShootPlanet);
        }

        private void EndArcCameraDown()
        {
            OnStateChange?.Invoke(GameState.ArcFlyFirstPerson);
            Debug.Log(GameState.ArcFlyFirstPerson);
        }

        private void EndArcFlyFromPlanet()
        {
            OnStateChange?.Invoke(GameState.ArcFlyCameraDown);
            Debug.Log(GameState.ArcFlyCameraDown);
        }

        private void EndGravityFromPlanetFromPlanet()
        {
            OnStateChange?.Invoke(GameState.ArcFlyFromPlanet);
            Debug.Log(GameState.ArcFlyFromPlanet);
        }

        private void EndEdgeGravityToPlanetToPlanet()
        {
            OnStateChange?.Invoke(GameState.ToCenterGravity);
            Debug.Log(GameState.ToCenterGravity);
        }

        private void EndToCenterGravity()
        {
            OnStateChange?.Invoke(GameState.FlyAroundPlanet);
            Debug.Log(GameState.FlyAroundPlanet);
        }

        private void EndRotateAround()
        {
            OnStateChange?.Invoke(GameState.EdgeGravityFromPlanet);
            Debug.Log(GameState.EdgeGravityFromPlanet);
        }

        public void Execute(float deltaTime)
        {
            _flewAngle.FlewAngle();
            _toCenterGravity.FlyToCenter(deltaTime);
            _edgeGravityToPlanet.Move(deltaTime);
            _edgeGravityFromPlanet.Move();
            _arcFromPlanet.Move(deltaTime);
            _arcCameraDown.Move(deltaTime);
            _arcFlyFirstPerson.Move(deltaTime);
        }
        
        public void Clean()
        {
            _flewAngle.OnFinish -= EndRotateAround;
            _toCenterGravity.OnFinish -= EndToCenterGravity;
            _edgeGravityToPlanet.OnFinish -= EndEdgeGravityToPlanetToPlanet;
        }
    }
}