using UnityEngine;

public class InputInitialization : IInitialization
{
    private IUserInput<float> _inputVertical;
    private IUserInput<float> _inputHorizontal;
    private IUserInput<Vector3> _inputTouchDown;
    private IUserInput<Vector3> _inputTouchUp;
    private IUserInput<Vector3> _inputTouchHold;

    public InputInitialization()
    {
        _inputVertical = new InputVertical();
        _inputHorizontal = new InputHorizontal();
        _inputTouchDown = new InputTouchDown();
        _inputTouchUp = new InputTouchUp();
        _inputTouchHold = new InputTouchHold();
    }

    public (IUserInput<float> inputVertical, IUserInput<float> inputHorizontal, IUserInput<Vector3> inputTouchDown, 
        IUserInput<Vector3> inputTouchUp, IUserInput<Vector3> inputTouchHold) GetAllInput()
    {
        (IUserInput<float> inputVertical, IUserInput<float> inputHorizontal, IUserInput<Vector3> inputTouch, 
            IUserInput<Vector3> inputTouchUp, IUserInput<Vector3> inputTouchHold) result =
            (_inputVertical, _inputHorizontal, _inputTouchDown, _inputTouchUp, _inputTouchHold);
        return result;
    }
    
    public (IUserInput<float> inputVertical, IUserInput<float> inputHorizontal) GetAxis()
    {
        (IUserInput<float> inputVertical, IUserInput<float> inputHorizontal) result =
            (_inputVertical, _inputHorizontal);
        return result;
    }
    
    public (IUserInput<Vector3> inputTouchDown, IUserInput<Vector3> inputTouchUp,IUserInput<Vector3> inputTouchHold) GetTouchAll()
    {
        (IUserInput<Vector3> inputTouchDown, IUserInput<Vector3> inputTouchUp, IUserInput<Vector3> inputTouchHold)
            result = (_inputTouchDown, _inputTouchUp, _inputTouchHold);
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
    
    public IUserInput<Vector3> GetTouchHold()
    {
        return _inputTouchHold;
    }
    
    public void Initialization()
    {
    }
}