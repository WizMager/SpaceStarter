using Interface;
using ScriptableData;
using UnityEngine;
using Utils;

namespace Controllers
{
    public class AtmosphereController : IExecute
    {
        private readonly Vector3 _planetCenter;
        private readonly Transform _planet;
        private readonly Transform _player;
        private readonly Transform _atmosphere;
        private StateController _stateController;
        private readonly MaterialsData _materialsData;

        private bool _isRotate;
        private Vector3 _atmosphereStartPosition;
        private Quaternion _atmosphereStartRotation;
        private Vector3 _startVectorAround;
        private Vector3 _endVectorAround;

        public AtmosphereController(StateController stateController, Transform planet, Transform player, Transform atmosphere, MaterialsData materialsData)
        {
            _stateController = stateController;
            _planet = planet;
            _planetCenter = planet.position;
            _player = player;
            _atmosphere = atmosphere;
            _atmosphereStartPosition = atmosphere.position;
            _atmosphereStartRotation = atmosphere.rotation;
            _materialsData = materialsData;
            PaintAtmosphere(_materialsData, _atmosphere);

            _stateController.OnStateChange += ChangeState;
        }

        private void ChangeState(GameState gameState)
        {
            switch (gameState)
            {
                case GameState.FlyAroundPlanet:
                    FlyAroundPlanet(7f);
                    _isRotate = true;
                    break;
                case GameState.EdgeGravityFromPlanet:
                    FlyAroundPlanet(-7f);
                    _isRotate = false;
                    break;
                case GameState.ArcFlyRadius:
                    _atmosphere.gameObject.SetActive(false);
                    _isRotate = false;
                    break;
                case GameState.Restart:
                    _atmosphere.gameObject.SetActive(true);
                    _atmosphere.SetPositionAndRotation(_atmosphereStartPosition, _atmosphereStartRotation);
                    _isRotate = false;
                    break;
                case GameState.NextLevel:
                    _atmosphere.gameObject.SetActive(true);
                    _atmosphere.SetPositionAndRotation(_atmosphereStartPosition, _atmosphereStartRotation);
                    _isRotate = false;
                    PaintAtmosphere(_materialsData, _atmosphere);
                    break;
            }
        }

        private void FlyAroundPlanet()
        {
            _startVectorAround = _player.position - _planet.position;
            var angle = Vector3.Angle(_startVectorAround, _endVectorAround);
            _atmosphere.RotateAround(_planetCenter, _planet.up, angle);
            _endVectorAround = _startVectorAround;
        }
        
        private void FlyAroundPlanet(float angle)
        {
            _startVectorAround = _player.position - _planet.position;
            _atmosphere.RotateAround(_planetCenter, _planet.up, angle);
            _endVectorAround = _startVectorAround;
        }

        public void Execute(float deltaTime)
        {
            if (!_isRotate) return;
            FlyAroundPlanet();
        }

        private void PaintAtmosphere(MaterialsData materialsData, Transform atmosphere)
        {
            var randomColor = Random.Range(0, materialsData.atmospherePlanet.Length);
            atmosphere.GetComponent<MeshRenderer>().material = materialsData.atmospherePlanet[randomColor];
        }
    }
}