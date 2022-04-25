using ScriptableData;
using UnityEngine;

namespace View
{
    public class ShipView : MonoBehaviour
    {
        [SerializeField] private GameObject _gravity;
        [SerializeField] private Transform _planet;
        private AllData _data;
        private TurbineShipView _turbineView;
        private Transform _turbine;
        private Vector3 _turbineConnectedPosition;
        private Quaternion _turbineConnectedRotation;
        private Vector3 _turbineEdgeGravityPosition;
        private Quaternion _turbineEdgeGravityRotation;

        private void Start()
        {
            _turbineView = GetComponentInChildren<TurbineShipView>();
            _turbine = _turbineView.gameObject.transform;
            _turbineConnectedPosition = _turbine.localPosition;
            _turbineConnectedRotation = _turbine.localRotation;
        }

        public void SetupAndCalculate(AllData data)
        {
            _data = data;
            _turbineEdgeGravityRotation = _turbine.rotation;
            var ray = new Ray(transform.position, transform.forward);
            var pathToEdgeGravity = Vector3.Distance(transform.position, _gravity.transform.position) -
                                    _gravity.GetComponent<MeshCollider>().bounds.size.x / 2;
            _turbineEdgeGravityPosition = ray.GetPoint(pathToEdgeGravity);
        }
        
        public void SeparateTurbine()
        {
            _turbine.SetParent(null);
            _turbineView.SetValueFields(_planet, _data);
        }

        public void StartFlyTurbine()
        {
            _turbineView.SwitchFlyAroundPlanet(true);
        }

        public void ConnectTurbine()
        {
            _turbine.SetParent(transform);
            _turbine.localPosition = _turbineConnectedPosition;
            _turbine.localRotation = _turbineConnectedRotation;
        }

        public void RestartConnectTurbine()
        {
            SeparateTurbine();
            _turbine.rotation = _turbineEdgeGravityRotation;
            _turbine.position = _turbineEdgeGravityPosition;
            _turbineView.Reset();
        }
    }
}