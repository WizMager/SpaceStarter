using UnityEngine;

public class InputInitialization : IInitialization
{
    private IUserInput<float> _inputVertical;
    private IUserInput<float> _inputHorizontal;
    private IUserInput<Vector3> _inputTouch;
    
    public InputInitialization()
    {
        _inputVertical = new InputVertical();
        _inputHorizontal = new InputHorizontal();
        _inputTouch = new InputTouch();
    }

    public (IUserInput<float> inputVertical, IUserInput<float> inputHorizontal, IUserInput<Vector3> inputTouch) GetAllInput()
    {
        (IUserInput<float> inputVertical, IUserInput<float> inputHorizontal, IUserInput<Vector3> inputTouch) result =
            (_inputVertical, _inputHorizontal, _inputTouch);
        return result;
    }
    
    public (IUserInput<float> inputVertical, IUserInput<float> inputHorizontal) GetAxisInput()
    {
        (IUserInput<float> inputVertical, IUserInput<float> inputHorizontal) result =
            (_inputVertical, _inputHorizontal);
        return result;
    }
    
    public IUserInput<Vector3> GetTouchInput()
    {
        return _inputTouch;
    }
    
    public void Initialization()
    {
    }
}