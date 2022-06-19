using ScriptableData;
using UnityEngine;

namespace View
{
    public class ShipView : MonoBehaviour
    {
        [SerializeField] private GameObject _gravity;
        [SerializeField] private ParticleSystem _shieldPS;
        private AllData _data;
        private TurbineShipView _turbineView;
        private Transform _turbine;
        private Vector3 _turbineConnectedPosition;
        private Quaternion _turbineConnectedRotation;
        private Vector3 _turbineEdgeGravityPosition;
        private Quaternion _turbineEdgeGravityRotation;
        private float _shieldTimeOn = 0f;

        public void Initialization(AllData data)
        {
            _data = data;
            _turbineView = FindObjectOfType<TurbineShipView>();
            _turbine = _turbineView.gameObject.transform;
            _turbineConnectedPosition = _turbine.localPosition;
            _turbineConnectedRotation = _turbine.localRotation;
        }
        
        public (Vector3 position, Quaternion rotation) SetupAndCalculate()
        {
            _turbineEdgeGravityRotation = _turbine.rotation;
            var ray = new Ray(transform.position, transform.forward);
            var pathToEdgeGravity = Vector3.Distance(transform.position, _gravity.transform.position) -
                                    _gravity.GetComponent<MeshCollider>().bounds.size.x / 2;
            _turbineEdgeGravityPosition = ray.GetPoint(pathToEdgeGravity);
            return (_turbineEdgeGravityPosition, _turbineEdgeGravityRotation);
        }
        
        public void Setup(Vector3 position, Quaternion rotation)
        {
            SeparateTurbine();
            _turbineEdgeGravityPosition = position;
            _turbineEdgeGravityRotation = rotation;
            _turbine.SetPositionAndRotation(position, rotation);
        }
        
        public void SeparateTurbine()
        {
            _turbine.SetParent(null);
            _turbineView.SetValueFields(_data);
        }

        public void StartFlyTurbine()
        {
            _turbineView.Reset();
        }

        public void ConnectTurbine()
        {
            _turbine.SetParent(transform);
            _turbine.localPosition = _turbineConnectedPosition;
            _turbine.localRotation = _turbineConnectedRotation;
        }

        public void RestartTurbine()
        {
            SeparateTurbine();
            _turbine.rotation = _turbineEdgeGravityRotation;
            _turbine.position = _turbineEdgeGravityPosition;
            _turbineView.SwitchFlyAroundPlanet(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (Time.time - _shieldTimeOn > .051f)
            {
                _shieldTimeOn = Time.time;
                _shieldPS.Play();
            }
        }

    }
}