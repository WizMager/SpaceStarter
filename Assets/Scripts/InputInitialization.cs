using UnityEngine;

public class InputInitialization : IInitialization
{
    private IUserInput<float> _inputVertical;
    private IUserInput<float> _inputHorizontal;
    private IUserInput<Vector3> _inputTouchDown;
    private IUserInput<Vector3> _inputTouchUp;

    public InputInitialization()
    {
        _inputVertical = new InputVertical();
        _inputHorizontal = new InputHorizontal();
        _inputTouchDown = new InputTouchDown();
        _inputTouchUp = new InputTouchUp();
    }

    public (IUserInput<float> inputVertical, IUserInput<float> inputHorizontal, IUserInput<Vector3> inputTouchDown, 
        IUserInput<Vector3> inputTouchUp) GetAllInput()
    {
        (IUserInput<float> inputVertical, IUserInput<float> inputHorizontal, IUserInput<Vector3> inputTouch, IUserInput<Vector3> inputTouchUp) result =
            (_inputVertical, _inputHorizontal, _inputTouchDown, _inputTouchUp);
        return result;
    }
    
    public (IUserInput<float> inputVertical, IUserInput<float> inputHorizontal) GetAxisInput()
    {
        (IUserInput<float> inputVertical, IUserInput<float> inputHorizontal) result =
            (_inputVertical, _inputHorizontal);
        return result;
    }
    
    public (IUserInput<Vector3> inputTouchDown, IUserInput<Vector3> inputTouchUp) GetTouchAllInput()
    {
        (IUserInput<Vector3> inputTouchDown, IUserInput<Vector3> inputTouchUp)
            result = (_inputTouchDown, _inputTouchUp);
        return result;
    }

    public IUserInput<Vector3> GetTouchDown()
    {
        return _inputTouchDown;
    }
    
    public IUserInput<Vector3> GetTouchUp()
    {
        return _inputTouchUp;
    }
    
    public void Initialization()
    {
    }
}