using System;
using UnityEngine;

public class CameraFirstPersonMoveController : IExecute, IClean
{
    private readonly Camera _camera;
    private readonly Transform _planetCenter;
    private readonly float _swipeSensitivity;
    private float _verticalChange;
    private float _horizontalChange;
    private IUserInput<float> _inputVertical;
    private IUserInput<float> _inputHorizontal;
    private bool _isChangeVertical;
    private bool _isChangeHorizontal;

    public CameraFirstPersonMoveController((IUserInput<float> inputVertical, IUserInput<float> inputHorizontal) axisInput, Camera camera, Transform planetCenter, float swipeSensitivity)
    {
        _camera = camera;
        _planetCenter = planetCenter;
        _swipeSensitivity = swipeSensitivity;
        var (inputVertical, inputHorizontal) = axisInput;
        _inputVertical = inputVertical;
        _inputHorizontal = inputHorizontal;
        _inputVertical.OnChange += VerticalAxisOnChange;
        _inputHorizontal.OnChange += HorizontalAxisOnChange;
    }

    private void VerticalAxisOnChange(float value)
    {
        _verticalChange = value;
        _isChangeVertical = true;
    }

    private void HorizontalAxisOnChange(float value)
    {
        _horizontalChange = value;
        _isChangeHorizontal = true;
    }

    public void Execute(float deltaTime)
    {
        MoveHorizontal(_isChangeHorizontal);
        MoveVertical(_isChangeVertical);
    }
    
    private void MoveHorizontal(bool isChangeHorizontal)
    {
        var position = _planetCenter.position;
        _camera.transform.RotateAround(position, Vector3.up, -_horizontalChange * _swipeSensitivity);
        _horizontalChange = 0;
        _isChangeHorizontal = false;
    }
    
    private void MoveVertical(bool isChangeVertical)
    {
        var position = _planetCenter.position;
        _camera.transform.RotateAround(position, Vector3.right, _verticalChange * _swipeSensitivity);
        _verticalChange = 0;
        _isChangeVertical = false;
    }

    public void Clean()
    {
        _inputVertical.OnChange -= VerticalAxisOnChange;
        _inputHorizontal.OnChange -= HorizontalAxisOnChange;
    }
}