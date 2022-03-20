using System;
using UnityEngine;
using Object = UnityEngine.Object;

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
                    var childrenTransforms = _trailPlayer.GetComponentsInChildren<Transform>();
                    Transform rightRayPoint = null;
                    Transform leftRayPoint = null;
                    Transform centerRayPoint = null;
                    foreach (var transform in childrenTransforms)
                    {
                        switch (transform.tag)
                        {
                            case "RightRayPoint":
                                rightRayPoint = transform;
                                break;
                            case "LeftRayPoint":
                                leftRayPoint = transform;
                                break;
                            case "CenterRayPoint":
                                centerRayPoint = transform;
                                break;
                        }
                    }

                    if (rightRayPoint == null && leftRayPoint == null && centerRayPoint == null)
                    {
                        throw new NullReferenceException("right left or center ray point is null");
                    }

                    var rayCenter = new Ray(centerRayPoint.position, centerRayPoint.forward);
                    var rayRight = new Ray(rightRayPoint.position, rightRayPoint.forward);
                    var rayLeft = new Ray(leftRayPoint.position, leftRayPoint.forward);
                    var raycastHitCenter = new RaycastHit[1];
                    var raycastHitRight = new RaycastHit[1];
                    var raycastHitLeft = new RaycastHit[1];
                    if (Physics.RaycastNonAlloc(rayRight, raycastHitRight) > 0 &&
                        Physics.RaycastNonAlloc(rayLeft, raycastHitLeft) > 0 &&
                        Physics.RaycastNonAlloc(rayCenter, raycastHitCenter) > 0)
                    {
                        var rightDistance = raycastHitRight[0].distance;
                        var centerDistance = raycastHitCenter[0].distance;
                        var leftDistance = raycastHitLeft[0].distance;

                        RaycastHit[] raycastHitForCalculate;
                        Ray rayForCalculate;
                        float distanceForCalculate;

                        if (centerDistance < leftDistance && centerDistance < rightDistance)
                        {
                            raycastHitForCalculate = raycastHitCenter;
                            rayForCalculate = rayCenter;
                            distanceForCalculate = centerDistance;
                        }
                        else if (rightDistance < leftDistance)
                        {
                            raycastHitForCalculate = raycastHitRight;
                            rayForCalculate = rayRight;
                            distanceForCalculate = rightDistance;
                        }
                        else
                        {
                            raycastHitForCalculate = raycastHitLeft;
                            rayForCalculate = rayLeft;
                            distanceForCalculate = leftDistance;
                        }

                        switch (raycastHitForCalculate[0].collider.tag)
                        {
                            case "Asteroid":
                                var currentDirection = rayForCalculate.direction;
                                var normal = raycastHitForCalculate[0].normal;
                                reflectVector = raycastHitForCalculate[0].point +
                                                Vector3.Reflect(currentDirection, normal);
                                distance = distanceForCalculate;
                                isCalculated = true;
                                break;
                            default:
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
                var childrenTransforms = _playerTransform.gameObject.GetComponentsInChildren<Transform>();
                Transform rightRayPoint = null;
                Transform leftRayPoint = null;
                Transform centerRayPoint = null;
                foreach (var transform in childrenTransforms)
                {
                    switch (transform.tag)
                    {
                        case "RightRayPoint":
                            rightRayPoint = transform;
                            break;
                        case "LeftRayPoint":
                            leftRayPoint = transform;
                            break;
                        case "CenterRayPoint":
                            centerRayPoint = transform;
                            break;
                    }
                }

                if (rightRayPoint == null && leftRayPoint == null && centerRayPoint == null)
                {
                    throw new NullReferenceException("right left or center ray point is null");
                }

                var rayCenter = new Ray(centerRayPoint.position, centerRayPoint.forward);
                var rayRight = new Ray(rightRayPoint.position, rightRayPoint.forward);
                var rayLeft = new Ray(leftRayPoint.position, leftRayPoint.forward);
                var raycastHitCenter = new RaycastHit[1];
                var raycastHitRight = new RaycastHit[1];
                var raycastHitLeft = new RaycastHit[1];
                if (Physics.RaycastNonAlloc(rayRight, raycastHitRight) > 0 &&
                    Physics.RaycastNonAlloc(rayLeft, raycastHitLeft) > 0 &&
                    Physics.RaycastNonAlloc(rayCenter, raycastHitCenter) > 0)
                {
                    var rightDistance = raycastHitRight[0].distance;
                    var centerDistance = raycastHitCenter[0].distance;
                    var leftDistance = raycastHitLeft[0].distance;

                    RaycastHit[] raycastHitForCalculate;
                    Ray rayForCalculate;
                    float distanceForCalculate;

                    if (centerDistance < leftDistance && centerDistance < rightDistance)
                    {
                        raycastHitForCalculate = raycastHitCenter;
                        rayForCalculate = rayCenter;
                        distanceForCalculate = centerDistance;
                    }
                    else if (rightDistance < leftDistance)
                    {
                        raycastHitForCalculate = raycastHitRight;
                        rayForCalculate = rayRight;
                        distanceForCalculate = rightDistance;
                    }
                    else
                    {
                        raycastHitForCalculate = raycastHitLeft;
                        rayForCalculate = rayLeft;
                        distanceForCalculate = leftDistance;
                    }

                    switch (raycastHitForCalculate[0].collider.tag)
                    {
                        case "Asteroid":
                            var currentDirection = rayForCalculate.direction;
                            var normal = raycastHitForCalculate[0].normal;
                            _reflectVector = raycastHitForCalculate[0].point +
                                             Vector3.Reflect(currentDirection, normal);
                            _distance = distanceForCalculate;
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

        public void ClearLine()
        {
            _lineRenderer.positionCount = 0;
        }
    }
}