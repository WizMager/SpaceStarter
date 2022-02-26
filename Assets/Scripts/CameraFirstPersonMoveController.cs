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

    public CameraFirstPersonMoveController((IUserInput<float> inputVertical, IUserInput<float> inputHorizontal) axisInput, Camera camera, Transform planetCenter, float swipeSensitivity)
    {
        _camera = camera;
        _planetCenter = planetCenter;
        _swipeSensitivity = swipeSensitivity;
        var (inputVertical, inputHorizontal) = axisInput;
        _inputVertical = inputVertical;
        _inputHorizontal = inputHorizontal;
        _inputVertical.OnChange += VerticalOnAxisChange;
        _inputHorizontal.OnChange += HorizontalOnAxisChange;
    }

    private void VerticalOnAxisChange(float value)
    {
        _verticalChange = value;
        
    }

    private void HorizontalOnAxisChange(float value)
    {
        _horizontalChange = value;
    }

    public void Execute(float deltaTime)
    {
        MoveHorizontal();
        MoveVertical();
    }
    
    private void MoveHorizontal()
    {
        _camera.transform.RotateAround(_planetCenter.position, Vector3.up, -_horizontalChange * _swipeSensitivity);
        _horizontalChange = 0;
    }
    
    private void MoveVertical()
    {
        _camera.transform.RotateAround(_planetCenter.position, Vector3.right, _verticalChange * _swipeSensitivity);
        _verticalChange = 0;
    }

    public void Clean()
    {
        _inputVertical.OnChange -= VerticalOnAxisChange;
        _inputHorizontal.OnChange -= HorizontalOnAxisChange;
    }
}