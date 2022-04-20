using ScriptableData;
using UnityEngine;

namespace View
{
    public class ShipView : MonoBehaviour
    {
        private TurbineShipView _turbinePart;
        private Vector3 _turbineConnectedPosition;

        private void Start()
        {
            _turbinePart = GetComponentInChildren<TurbineShipView>();
            _turbineConnectedPosition = _turbinePart.gameObject.transform.position;
        }

        public void SeparateTurbine(Transform planet, AllData data)
        {
            _turbinePart.gameObject.transform.SetParent(null);
            _turbinePart.SetValueFields(planet, data);
        }

        public void StartFlyTurbine()
        {
            _turbinePart.SwitchFlyAroundPlanet(true);
        }

        public void ConnectTurbine()
        {
            //_backPart.gameObject.transform.SetParent(transform, false);
            _turbinePart.gameObject.transform.position = _turbineConnectedPosition;
        }
    }
}