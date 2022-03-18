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

        private bool _isDirectionCalculated;
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
            var isDirectionCalculated = false;
            _lineRenderer.positionCount = _iterations;

            for (int i = 0; i < _iterations; i++)
            {
                
                    if (!isDirectionCalculated)
                    {
                        var ray = new Ray(_trailPlayer.transform.position, _trailPlayer.transform.forward);
                        var raycastHit = new RaycastHit[1];
                        if (Physics.RaycastNonAlloc(ray, raycastHit) > 0)
                        {
                            if (raycastHit[0].collider.tag == "Asteroid")
                            {
                                var currentDirection = _trailPlayer.transform.forward.normalized;
                                var normal = raycastHit[0].normal;
                                reflectVector = raycastHit[0].point + Vector3.Reflect(currentDirection, normal);
                                distance = Vector3.Distance(_trailPlayer.transform.position, raycastHit[0].point);
                                isDirectionCalculated = true;
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
                       isDirectionCalculated = false;
                    }
                    
                    _lineRenderer.SetPosition(i, _trailPlayer.transform.position);
            }
        }
        
        public void Move(float deltaTime)
        {
            if (!_isDirectionCalculated)
            {
                var ray = new Ray(_playerTransform.position, _playerTransform.forward);
                var raycastHit = new RaycastHit[1];
                if (Physics.RaycastNonAlloc(ray, raycastHit) > 0)
                {
                    if (raycastHit[0].collider.tag == "Asteroid")
                    {
                        var currentDirection = _playerTransform.forward.normalized;
                        var normal = raycastHit[0].normal;
                        _reflectVector = raycastHit[0].point + Vector3.Reflect(currentDirection, normal);
                        _distance = Vector3.Distance(_playerTransform.position, raycastHit[0].point);
                        _isDirectionCalculated = true;
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
            
            if (_distance > 0) return;
            _playerTransform.LookAt(_reflectVector);
            _isDirectionCalculated = false;
        }

        public void ClearLine()
        {
            _lineRenderer.positionCount = 0;
        }
    }
}