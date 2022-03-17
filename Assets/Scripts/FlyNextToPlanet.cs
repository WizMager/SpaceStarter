using UnityEngine;
using Utils;
using View;

public class FlyNextToPlanet
{
    private readonly float _moveSpeed;
    private GravityView _gravityView;
    private readonly Transform _playerTransform;
    private readonly AsteroidView[] _asteroidViews;
        
    private bool _isInGravity;
    private bool _isActive;
    private bool _isAngleCalculated;
    private Vector3 _reflectVector;
    private bool _colliderEntered;

    public FlyNextToPlanet(float moveSpeed, GravityView gravityView, Transform playerTransform, AsteroidView[] asteroidViews)
    {
        _moveSpeed = moveSpeed;
        _gravityView = gravityView;
        _playerTransform = playerTransform;
        _asteroidViews = asteroidViews;

        _gravityView.OnPlayerGravityEnter += GravityEntered;
        SubscribeToAsteroids();
    }

    private void SubscribeToAsteroids()
    {
        foreach (var asteroidView in _asteroidViews)
        {
            asteroidView.OnColliderEnter += ColliderEntered;
            asteroidView.OnColliderExit += ColliderExited;
        }
    }
    
    private void GravityEntered()
    {
        if (!_isActive) return;
                
        _isInGravity = true;
    }

    public bool IsFinished()
    {
        if (_isInGravity)
        {
            _isInGravity = false;
            return true;
        }
                
        _playerTransform.Translate(_playerTransform.forward * Time.deltaTime * _moveSpeed, Space.World);
        
        Debug.DrawLine(_playerTransform.position, _reflectVector);
        if (!_isAngleCalculated)
        {
            var ray = new Ray(_playerTransform.position, _playerTransform.forward);
            var raycastHit = new RaycastHit[1];
            if (Physics.RaycastNonAlloc(ray, raycastHit) > 0)
            {
                var currentDirection = _playerTransform.forward.normalized;
                var normal = raycastHit[0].normal;
                _reflectVector = Vector3.Reflect(currentDirection, normal);
                _isAngleCalculated = true;
            }
        }
        return false;
    }
    
    private void ColliderEntered()
    {
        if (_colliderEntered) return;
        
        _playerTransform.LookAt(_reflectVector);
        //_playerTransform.rotation.SetLookRotation(_reflectVector);
        _colliderEntered = true;
    }

    private void ColliderExited()
    {
        _colliderEntered = false;
        _isAngleCalculated = false;
    }
    
    public void SetActive(bool isActive)
    {
        _isActive = isActive;
    }

    public void ChangePlanet(GravityView currentGravityView)
    {
        OnDestroy();
        _gravityView = currentGravityView;
        _gravityView.OnPlayerGravityEnter += GravityEntered;
    }
        
    private void UnSubscribeToAsteroids()
    {
        foreach (var asteroidView in _asteroidViews)
        {
            asteroidView.OnColliderEnter -= ColliderEntered;
            asteroidView.OnColliderExit -= ColliderExited;
        }
    }
    
    public void OnDestroy()
    {
        _gravityView.OnPlayerGravityEnter -= GravityEntered;
        //UnSubscribeToAsteroids();
    }
}