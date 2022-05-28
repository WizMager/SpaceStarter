using ScriptableData;
using UnityEngine;

namespace View
{
    public class ShipView : MonoBehaviour
    {
        [SerializeField] private GameObject _gravity;
        [SerializeField] private Transform _planet;
        [SerializeField] private ParticleSystem _shieldPS;
        private AllData _data;
        private TurbineShipView _turbineView;
        private Transform _turbine;
        private Vector3 _turbineConnectedPosition;
        private Quaternion _turbineConnectedRotation;
        private Vector3 _turbineEdgeGravityPosition;
        private Quaternion _turbineEdgeGravityRotation;
        private float _shieldTimeOn = 0f;

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
            //Debug.Log("Trigger");
            //if (!_shieldPS.isPlaying)
            //Debug.Log(Time.time - _shieldTimeOn);
            if (Time.time - _shieldTimeOn > .051f)
            {
                _shieldTimeOn = Time.time;
                //Debug.Log(Time.time);
                _shieldPS.Play();
            }
            
            //if (other.CompareTag("Planet"))
            //{
            //    _body.isKinematic = true;
            //    _isActive = false;
            //}

            //if (!other.gameObject.CompareTag("Player")) return;
            //var shipPosition = other.transform.position;
            //var shipRotation = other.transform.rotation;
            //var cameraView = other.GetComponent<CameraView>();
            ////Debug.Log("gameObject " + gameObject.transform.position);
            ////Debug.DrawLine(shipPosition, gameObject.transform.position - shipPosition, Color.red, 1000f);
            //OnShipTouch?.Invoke(gameObject.name, _floorType, shipPosition, shipRotation, cameraView);
            //gameObject.GetComponent<BoxCollider>().enabled = false;
        }

    }
}