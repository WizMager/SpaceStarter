using UnityEngine;

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

        public void CalculateTrajectory(Transform playerTransform)
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
                    var hit = new RaycastHit[1];
                    var countRaycast = Physics.RaycastNonAlloc(ray, hit);
                    if (countRaycast > 0)
                    {
                        switch (hit[0].collider.tag)
                        {
                            case "Asteroid":
                                var currentDirection = ray.direction.normalized;
                                var normal = hit[0].normal.normalized;
                                reflectVector = hit[0].point + Vector3.Reflect(currentDirection, normal);
                                distance = hit[0].distance;
                                isCalculated = true;
                                break;
                            default:
                                distance = 10f;
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
                var hit = new RaycastHit[1];
                var countRaycast = Physics.RaycastNonAlloc(ray, hit);

                if (countRaycast > 0)
                {
                    switch (hit[0].collider.tag)
                    {
                        case "Asteroid":
                            var currentDirection = ray.direction;
                            var normal = hit[0].normal;
                            _reflectVector = hit[0].point + Vector3.Reflect(currentDirection, normal);
                            _distance = hit[0].distance;
                            _isCalculated = true;
                            break;
                        default:
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
        
        public void Move(float deltaTime, float speed)
        {
            if (!_isCalculated)
            {
                var ray = new Ray(_playerTransform.position, _playerTransform.forward);
                var hit = new RaycastHit[1];
                var countRaycast = Physics.RaycastNonAlloc(ray, hit);

                if (countRaycast > 0)
                {
                    switch (hit[0].collider.tag)
                    {
                        case "Asteroid":
                            var currentDirection = ray.direction;
                            var normal = hit[0].normal;
                            _reflectVector = hit[0].point + Vector3.Reflect(currentDirection, normal);
                            _distance = hit[0].distance;
                            _isCalculated = true;
                            break;
                        default:
                            _distance = 10f;
                            break;
                    }
                }
                else
                {
                    _distance = 10f;
                }
            }
            
            var moveDistance = deltaTime * speed;
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
            _distance = 10f;
        }
    }