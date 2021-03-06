using System;
using System.Collections.Generic;
using Interface;
using Model;
using ScriptableData;
using StateClasses;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using View;

namespace Controllers
{
    public class StateController : IExecute, IClean
    {
        public event Action<GameState> OnStateChange;

        private readonly PlayerModel _playerModel;
        private readonly DeadScreenView _deadView;
        private readonly FirstPersonView _firstPersonView;
        private readonly Button[] _restartButtons;
        private readonly FinalScreenView _finalScreenView;
        private readonly List<GameObject> _rocketIndicators;
        private readonly GameObject _ship;

        private readonly StartPositionPlayerAndCamera _startPosition;
        private readonly FlewAngleCounter _flewAngle;
        private readonly FlyToCenterGravity _toCenterGravity;
        private readonly EdgeGravityToPlanet _edgeGravityToPlanet;
        private readonly EdgeGravityFromPlanet _edgeGravityFromPlanet;
        private readonly ArcFromPlanet _arcFromPlanet;
        private readonly ArcFlyRadius _arcFlyRadius;
        private readonly ArcCameraDown _arcCameraDown;
        private readonly ArcFlyFirstPerson _arcFlyFirstPerson;
        private readonly NextStateAfterEndShoot _nextStateAfterEndShoot;
        private readonly FlyAway _flyAway;
        private readonly EndFlyAway _endFlyAway;
        private readonly RestartAfterWaiting _restartAfterWaiting;

        public StateController(PlanetView planetView, ShipView shipView, AllData data, GravityView gravityView, 
            GravityLittleView gravityLittleView, Camera camera, PlayerModel playerModel, DeadScreenView deadView, 
            FirstPersonView firstPersonView, RestartButtonView[] restartButtons, FinalScreenView finalScreenView, 
            RocketIndicatorView[] rocketIndicatorViews)
        {
            _ship = shipView.gameObject;
            _playerModel = playerModel;
            _deadView = deadView;
            _firstPersonView = firstPersonView;
            _restartButtons = new Button[restartButtons.Length];
            for (int i = 0; i < restartButtons.Length; i++)
            {
                _restartButtons[i] = restartButtons[i].GetComponent<Button>();
            }
            _finalScreenView = finalScreenView;
            _rocketIndicators = new List<GameObject>(rocketIndicatorViews.Length);
            foreach (var rocketIndicatorView in rocketIndicatorViews)
            {
                _rocketIndicators.Add(rocketIndicatorView.gameObject);
            }

            var playerTransform = shipView.transform;
            var planetTransform = planetView.transform;
            var gravityHalfSize = gravityView.GetComponent<MeshRenderer>().bounds.size.x / 2;

            _startPosition = new StartPositionPlayerAndCamera(playerTransform, planetTransform, gravityView.transform,
                camera.transform, data.Planet.distanceFromCenterPlanetToSpawn, data.Camera.startCameraHeight,
                data.Camera.cameraDownPosition, gravityLittleView);
            _flewAngle = new FlewAngleCounter(planetTransform, playerTransform, data.Planet.flyAngle, 
                this);
            _toCenterGravity = new FlyToCenterGravity(shipView, data.Planet.moveSpeedCenterGravity, planetTransform, this);
            _edgeGravityToPlanet = new EdgeGravityToPlanet(playerTransform, gravityView, this, 
                data.Planet.moveSpeedToPlanet);
            _edgeGravityFromPlanet = new EdgeGravityFromPlanet(data.Planet.moveSpeedToEdgeGravity, gravityHalfSize, 
                shipView, this, planetTransform);
            _arcFromPlanet = new ArcFromPlanet(this, playerTransform, data.Planet.distanceToCenterRadiusArc,
                data.Planet.radiusArc, data.Planet.moveSpeedArcFromPlanet, data.Planet.rotationSpeedArcFromPlanet, 
                gravityView.gameObject, gravityLittleView.gameObject, planetTransform.GetComponent<SphereCollider>());
            _arcFlyRadius = new ArcFlyRadius(this, playerTransform, data.Planet.rotationSpeedRadius, _arcFromPlanet);
            _arcCameraDown = new ArcCameraDown(this, playerTransform, planetView,
                data.Planet.stopDistanceFromPlanetSurface, data.Planet.percentOfCameraDownPath, 
                data.Planet.moveSpeedArcCameraDown, data.Planet.rotationSpeedArcFromPlanet);
            _arcFlyFirstPerson = new ArcFlyFirstPerson(this, playerTransform, planetView,
                data.Planet.stopDistanceFromPlanetSurface,
                data.Planet.percentOfCameraDownPath, data.Planet.moveSpeedArcFirstPerson);
            _nextStateAfterEndShoot = new NextStateAfterEndShoot(this, data.Planet.waitBeforeFlyAway);
            _flyAway = new FlyAway(this, playerTransform, planetTransform, data.Planet.distanceFlyAway,
                data.Planet.moveSpeedFlyAway, data.Planet.rotationSpeedFlyAway, gravityView.gameObject);
            _endFlyAway = new EndFlyAway(this, playerTransform);
            _restartAfterWaiting = new RestartAfterWaiting(this, data.RestartData.waitAfterRestart, planetView);

            _flewAngle.OnFinish += EndRotateAround;
            _toCenterGravity.OnFinish += EndToCenterGravity;
            _edgeGravityToPlanet.OnFinish += EndEdgeGravityToPlanetToPlanet;
            _edgeGravityFromPlanet.OnFinished += EndGravityFromPlanetFromPlanet;
            _arcFromPlanet.OnFinish += EndArcFlyFromPlanet;
            _arcFlyRadius.OnFinish += EndFlyRadius;
            _arcCameraDown.OnFinish += EndArcCameraDown;
            _arcFlyFirstPerson.OnFinish += EndArcFlyFirstPerson;
            _nextStateAfterEndShoot.OnFinish += NextStateAfterEndShoot;
            _flyAway.OnFinish += EndFlyAway;
            _endFlyAway.OnFinish += EndCycle;
            _playerModel.OnZeroHealth += RocketCrushed;
            _playerModel.OnZeroRocketLeft += EndShoot;
            _restartAfterWaiting.OnFinish += RestartAfterWaiting;
            foreach (var restartButton in _restartButtons)
            {
                restartButton.onClick.AddListener(Restart);
                restartButton.onClick.AddListener(NextLevel);
            }

            _deadView.gameObject.SetActive(false);
            _startPosition.Set();
            SwitchUIWhenInteract(false);
        }

        private void SwitchUIWhenInteract(bool isActive)
        {
            _firstPersonView.gameObject.SetActive(isActive);
            foreach (var restartButton in _restartButtons)
            {
                if (restartButton.GetComponent<RestartButtonView>().GetButtonType !=
                    RestartButtonType.WhenInteract) continue;
                restartButton.gameObject.SetActive(isActive);
                break;
            }

            foreach (var rocketIndicator in _rocketIndicators)
            {
                rocketIndicator.SetActive(isActive);
            }
        }
        
        private void RocketCrushed()
        {
            _firstPersonView.gameObject.SetActive(false);
            OnStateChange?.Invoke(GameState.RocketCrushed);
            _deadView.gameObject.SetActive(true);
            Debug.Log(GameState.RocketCrushed);
        }

        private void Restart()
        {
            OnStateChange?.Invoke(GameState.Restart);
            Debug.Log(GameState.Restart);
            SwitchUIWhenInteract(true);
            _finalScreenView.gameObject.SetActive(false);
            _deadView.gameObject.SetActive(false);
            _startPosition.SetRestart();
            _playerModel.ResetRound();
        }

        private void NextLevel()
        {
            OnStateChange?.Invoke(GameState.NextLevel);
            Debug.Log(GameState.NextLevel);
            SwitchUIWhenInteract(false);
            _finalScreenView.gameObject.SetActive(false);
            _deadView.gameObject.SetActive(false);
            _ship.SetActive(true);
            _startPosition.SetNextRound();
            _playerModel.ResetRound();
            OnStateChange?.Invoke(GameState.EdgeGravityToPlanet);
        }
        
        private void RestartAfterWaiting()
        {
            OnStateChange?.Invoke(GameState.RestartAfterWaiting);
            Debug.Log(GameState.RestartAfterWaiting);
            
            OnStateChange?.Invoke(GameState.FlyAroundPlanet);
            Debug.Log(GameState.FlyAroundPlanet);
        }

        private void EndCycle()
        {
            _finalScreenView.gameObject.SetActive(true);
            var buttons = _finalScreenView.SetValue(_playerModel.GetValueToFinalScreen());
            buttons[0].onClick.AddListener(Restart);
            buttons[1].onClick.AddListener(NextLevel);
            Debug.Log("End Cycle");
        }
        
        private void EndFlyAway()
        {
            SwitchUIWhenInteract(false);
            Debug.Log(GameState.EndFlyAway);
            OnStateChange?.Invoke(GameState.EndFlyAway);
        }

        private void NextStateAfterEndShoot()
        {
            SwitchUIWhenInteract(false);
            OnStateChange?.Invoke(GameState.FlyAway);
            Debug.Log(GameState.FlyAway);
        }
        
        private void EndShoot()
        {
            SwitchUIWhenInteract(false);
            OnStateChange?.Invoke(GameState.NextStateAfterEndShoot);
            Debug.Log(GameState.NextStateAfterEndShoot);
        }

        private void EndArcFlyFirstPerson()
        {
            OnStateChange?.Invoke(GameState.ShootPlanet);
            Debug.Log(GameState.ShootPlanet);
        }

        private void EndArcCameraDown()
        {
            SwitchUIWhenInteract(true);
            OnStateChange?.Invoke(GameState.ArcFlyFirstPerson);
            Debug.Log(GameState.ArcFlyFirstPerson);
        }

        private void EndFlyRadius()
        {
            OnStateChange?.Invoke(GameState.ArcFlyCameraDown);
            Debug.Log(GameState.ArcFlyCameraDown);
        }
        
        private void EndArcFlyFromPlanet()
        {
            OnStateChange?.Invoke(GameState.ArcFlyRadius);
            Debug.Log(GameState.ArcFlyRadius);
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
            SwitchUIWhenInteract(true);
            OnStateChange?.Invoke(GameState.FlyAroundPlanet);
            Debug.Log(GameState.FlyAroundPlanet);
        }

        private void EndRotateAround()
        {
            SwitchUIWhenInteract(false);
            OnStateChange?.Invoke(GameState.EdgeGravityFromPlanet);
            Debug.Log(GameState.EdgeGravityFromPlanet);
        }

        public void Execute(float deltaTime)
        {
            _flewAngle.FlewAngle();
            _edgeGravityToPlanet.Move(deltaTime);
            _arcFromPlanet.Move(deltaTime);
            _arcFlyRadius.Move(deltaTime);
            _arcCameraDown.Move(deltaTime);
            _arcFlyFirstPerson.Move(deltaTime);
            _nextStateAfterEndShoot.Move(deltaTime);
            _flyAway.Move(deltaTime);
        }
        
        public void Clean()
        {
            _flewAngle.OnFinish -= EndRotateAround;
            _toCenterGravity.OnFinish -= EndToCenterGravity;
            _edgeGravityToPlanet.OnFinish -= EndEdgeGravityToPlanetToPlanet;
            _edgeGravityFromPlanet.OnFinished -= EndGravityFromPlanetFromPlanet;
            _arcFromPlanet.OnFinish -= EndArcFlyFromPlanet;
            _arcFlyRadius.OnFinish -= EndFlyRadius;
            _arcCameraDown.OnFinish -= EndArcCameraDown;
            _arcFlyFirstPerson.OnFinish -= EndArcFlyFirstPerson;
            _nextStateAfterEndShoot.OnFinish -= NextStateAfterEndShoot;
            _flyAway.OnFinish -= EndFlyAway;
            _endFlyAway.OnFinish -= EndCycle;
            _playerModel.OnZeroHealth -= RocketCrushed;
            _restartAfterWaiting.OnFinish += RestartAfterWaiting;
            
            foreach (var restartButton in _restartButtons)
            {
                restartButton.onClick.RemoveAllListeners();
            }
            _flewAngle.Dispose();
            _toCenterGravity.Dispose();
            _edgeGravityToPlanet.Dispose();
            _edgeGravityFromPlanet.Dispose();
            _arcFromPlanet.Dispose();
            _arcFlyRadius.Dispose();
            _arcCameraDown.Dispose();
            _arcFlyFirstPerson.Dispose();
            _nextStateAfterEndShoot.Dispose();
            _flyAway.Dispose();
            _endFlyAway.Dispose();
            _restartAfterWaiting.Dispose();
        }
    }
}