using UnityEngine;

namespace DefaultNamespace
{
    public class TrajectoryCalculate
    {
        private readonly Transform _playerTransform;
        private readonly float _moveSpeed;
        private readonly int _iterations;
        private readonly float _oneStepTime;
        private readonly GameObject _trailPlayer;
        private readonly LineRenderer _lineRenderer;

        private bool _isCalculated;
        private Vector3 _reflectVector;
        private float _distance = 10f;

        public TrajectoryCalculate(Transform playerTransform, float moveSpeed, int iterations, float oneStepTime)
        {
            _playerTransform = playerTransform;
            _moveSpeed = moveSpeed;
            _iterations = iterations;
            _oneStepTime = oneStepTime;
            
            _trailPlayer = Object.Instantiate(Resources.Load<GameObject>("TrailPlayer"));
            _lineRenderer = _trailPlayer.GetComponent<LineRenderer>();
        }

        public void Calculate(Transform playerTransform)
        {
            _trailPlayer.transform.position = playerTransform.position;
            _trailPlayer.transform.rotation = playerTransform.rotation;
            var reflectVector = Vector3.zero;
            var distance = 10f;
            var isCalculated = false;
            _lineRenderer.positionCount = _iterations;

            for (int i = 0; i < _iterations; i++)
            {
                if (!isCalculated)
                {
                    var ray = new Ray(_trailPlayer.transform.position, _trailPlayer.transform.forward);
                    var rcHit = new RaycastHit[1];
                    Physics.RaycastNonAlloc(ray, rcHit);
                    var raycastHit = new RaycastHit[1];
                    if (Physics.BoxCastNonAlloc( _trailPlayer.transform.position, new Vector3(0.25f, 0.25f, 0.5f), 
                            _trailPlayer.transform.forward, raycastHit, _trailPlayer.transform.rotation, rcHit[0].distance + 0.1f) > 0)
                    {
                        switch (raycastHit[0].collider.tag)
                        {
                            case "Asteroid":
                                var currentDirection = _trailPlayer.transform.forward.normalized;
                                var normal = rcHit[0].normal;
                                reflectVector = rcHit[0].point + Vector3.Reflect(currentDirection, normal);
                                distance = Vector3.Distance(_trailPlayer.transform.position, rcHit[0].point);
                                isCalculated = true;
                                break;
                            default:
                                //Debug.Log($"Raycast to something not in list {raycastHit[0].collider.tag}");
                                _distance = 10f; 
                                break;
                        }
                    }
                    else
                    {
                        distance = 10f;
                    }
                }
                _trailPlayer.transform.Translate(_trailPlayer.transform.forward * _oneStepTime, Space.World);
                distance -= _oneStepTime;
                if (distance <= 0)
                {
                    _trailPlayer.transform.LookAt(reflectVector);
                    isCalculated = false;
                }
                _lineRenderer.SetPosition(i, _trailPlayer.transform.position);
            }
        }
        
        public void Move(float deltaTime)
        {
            if (!_isCalculated)
            {
                var ray = new Ray(_playerTransform.position, _playerTransform.forward);
                var rcHit = new RaycastHit[1];
                Physics.RaycastNonAlloc(ray, rcHit);
                var raycastHit = new RaycastHit[1];
                if (Physics.BoxCastNonAlloc( _playerTransform.transform.position, new Vector3(0.25f, 0.25f, 0.5f), 
                        _playerTransform.transform.forward, raycastHit, _playerTransform.transform.rotation, rcHit[0].distance + 0.1f) > 0)
                {
                    switch (raycastHit[0].collider.tag)
                    {
                        case "Asteroid":
                            var currentDirection = _playerTransform.forward.normalized;
                            var normal = rcHit[0].normal;
                            _reflectVector = rcHit[0].point + Vector3.Reflect(currentDirection, normal);
                            _distance = Vector3.Distance(_playerTransform.position, rcHit[0].point);
                            _isCalculated = true;
                            break;
                        default:
                            //Debug.Log($"Raycast to something not in list {raycastHit[0].collider.tag}");
                            _distance = 10f; 
                            break;
                    }
                }
                else
                {
                    _distance = 10f; 
                }
            }
            var moveDistance = deltaTime * _moveSpeed;
            _playerTransform.Translate(_playerTransform.forward * moveDistance, Space.World);
            _distance -= moveDistance;
            
            if (_distance <= 0)
            {
                _playerTransform.LookAt(_reflectVector);
                _isCalculated = false;
            }
        }

        public void ClearLine()
        {
            _lineRenderer.positionCount = 0;
        }
    }
}