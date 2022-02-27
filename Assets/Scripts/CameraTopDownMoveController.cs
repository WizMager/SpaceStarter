using UnityEngine;

public class CameraTopDownMoveController : IExecute, IClean
{
    private Camera _camera;
    private IUserInput<float> _inputVertical;
    private IUserInput<float> _inputHorizontal;
    private float _vertical;
    private float _horizontal;
    private bool _isChangeAxis;

    public CameraTopDownMoveController((IUserInput<float> vertical, IUserInput<float> horizontal) axisInput, Camera camera)
    {
        _camera = camera;
        var (vertical, horizontal) = axisInput;
        _inputVertical = vertical;
        _inputHorizontal = horizontal;
        _inputVertical.OnChange += VerticalAxisOnChange;
        _inputHorizontal.OnChange += HorizontalAxisOnChange;
    }

    private void VerticalAxisOnChange(float vertical)
    {
        _vertical = vertical;
    }

    private void HorizontalAxisOnChange(float horizontal)
    {
        _horizontal = horizontal;
    }

    private void CameraMove()
    {
        
    }
    
    public void Execute(float deltaTime)
    {
        
    }

    public void Clean()
    {
        
    }
}