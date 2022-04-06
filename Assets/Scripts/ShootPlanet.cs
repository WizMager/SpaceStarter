using System;
using Controllers;
using Interface;
using UnityEngine;
using Utils;
using View;
using Object = UnityEngine.Object;

public class ShootPlanet : IDisposable
{
    public event Action OnFinish;
    
    private readonly IUserInput<Vector3>[] _touch;
    private readonly Camera _camera;
    private readonly Transform _missileStartPosition;
    private readonly ScriptableData.AllData _data;
    private readonly GameObject _missile;
    private readonly StateController _stateController;

    private bool _isActive;
    private Vector3 _touchDownPosition;
    private int _testShootCounter = 3;

    public ShootPlanet(IUserInput<Vector3>[] touch, Camera camera, ScriptableData.AllData data, Transform missileStartPosition, StateController stateController)
    {
        _touch = touch;
        _camera = camera;
        _data = data;
        _missile = data.Missile.missilePrefab;
        _missileStartPosition = missileStartPosition;
        _stateController = stateController;

        _touch[(int) TouchInputState.InputTouchDown].OnChange += TouchDown;
        _touch[(int) TouchInputState.InputTouchUp].OnChange += TouchUp;
        _stateController.OnStateChange += ChangeState;
    }

    private void ChangeState(GameState state)
    {
        _isActive = state == GameState.ShootPlanet;
    }

    private void Shoot(Vector3 touchPosition)
    {
        var ray = _camera.ScreenPointToRay(touchPosition);
        var raycastHit = new RaycastHit[1];
        Physics.RaycastNonAlloc(ray, raycastHit, _camera.farClipPlane, GlobalData.LayerForAim);
        var cameraTransform = _camera.transform; 
        var missile = Object.Instantiate(_missile, _missileStartPosition.position, cameraTransform.rotation);
        var missileView = missile.GetComponent<MissileView>();
        missileView.SetParams(_data, raycastHit[0].point);
        _testShootCounter--;
        if (_testShootCounter <= 0)
        {
            OnFinish?.Invoke();
        }
    }

    private void TouchDown(Vector3 position)
    {
        if (!_isActive) return;

        _touchDownPosition = position;
    }

    private void TouchUp(Vector3 position)
    {
        if (!_isActive) return;

        if (_touchDownPosition != position) return;
        
        Shoot(position);
    }

    public void Dispose()
    {
        _touch[(int) TouchInputState.InputTouchDown].OnChange -= TouchDown;
        _touch[(int) TouchInputState.InputTouchUp].OnChange -= TouchUp;
        _stateController.OnStateChange -= ChangeState;
    }
}