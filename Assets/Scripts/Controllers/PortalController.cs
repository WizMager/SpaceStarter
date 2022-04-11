using System;
using Interface;
using ScriptableData;
using UnityEngine;
using Utils;
using View;
using Object = UnityEngine.Object;

namespace Controllers
{
    public class PortalController : IExecute
    {
        private readonly StateController _stateController;
        private GameState _gameState;
        
        private readonly Transform _player;
        private readonly Transform _planet;
        private readonly GameObject _portalPrefab;
        private Transform _portal;
        private PortalView _portalView;
        private readonly AllData _data;
        private bool _isOpen;
        private bool _isLaunched;

        public PortalController(Transform playerTransform, Transform planetTransform, AllData data,
            StateController stateController)
        {
            _stateController = stateController;
            _portalPrefab = data.Portal.portalPrefab;
            _player = playerTransform;
            _planet = planetTransform;
            _data = data;
            _isOpen = false;
            _isLaunched = false;
            
            // //var portal = GameObject.Find("Portal");
            // var portal = GameObject.FindWithTag("Portal");
            // var portalConeStart = GameObject.Find("ConeStart");
            //
            // //portal.SetActive(false);
            // portalConeStart.SetActive(false);
            
            _stateController.OnStateChange += ChangeState;
        }

        private void ChangeState(GameState gameState)
        {
            _gameState = gameState;
        }

        private Vector3 GetPortalPosition()
        {
            return Vector3.zero;
        }
        
        private void CreatePortal()
        {
            if (!_portal)
            {
                _portal = Object.Instantiate(_portalPrefab, GetPortalPosition(), _player.rotation).transform;
                _portalView = _portal.GetComponent<PortalView>();
            }
        }
        
        private void OpenPortal()
        {
            if (!_isOpen)
            {
                var distance = _data.Planet.distanceFlyAway;
                Vector3 dirNorm = (_player.position - _planet.position).normalized;
                Vector3 target = _player.position + dirNorm * distance;

                _portalView.SetPosition(target, _player.rotation);
                _portalView.OpenPortal();
                _isOpen = true;
            }
        }

        private void LaunchPortal()
        {
            if (!_isLaunched)
            {
                _portalView.LaunchTeleport();
                _isLaunched = true;
            }
        }
        
        public void Execute(float deltaTime)
        {
            switch (_gameState)
            {
                case GameState.EdgeGravityToPlanet:
                    break;
                case GameState.ToCenterGravity:
                    break;
                case GameState.FlyAroundPlanet:
                    break;
                case GameState.EdgeGravityFromPlanet:
                    break;
                case GameState.ArcFlyFromPlanet:
                    break;
                case GameState.ArcFlyRadius:
                    break;
                case GameState.ArcFlyCameraDown:
                    break;
                case GameState.ArcFlyFirstPerson:
                    break;
                case GameState.ShootPlanet:
                    CreatePortal();
                    break;
                case GameState.FlyAway:
                    OpenPortal();
                    break;
                case GameState.EndFlyAway:
                    LaunchPortal();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}