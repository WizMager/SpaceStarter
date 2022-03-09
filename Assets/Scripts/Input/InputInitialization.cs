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
            IUserInput<Vector3> inputTouchHold, IUserInput<Vector3> inputTouchUp) GetAllInput()
    {
        (IUserInput<float> inputVertical, IUserInput<float> inputHorizontal, IUserInput<Vector3> inputTouch, 
            IUserInput<Vector3> inputTouchHold, IUserInput<Vector3> inputTouchUp) result =
            (_inputVertical, _inputHorizontal, _inputTouchDown, _inputTouchHold,_inputTouchUp);
        return result;
    }
    
    public IUserInput<float>[] GetAxis()
    {
        var result = new[] {_inputVertical, _inputHorizontal};
        return result;
    }
    
    public IUserInput<Vector3>[] GetTouchAll()
    {
        var result = new[]
        {
            _inputTouchDown, _inputTouchHold, _inputTouchUp
        };
        return result;
    }
    
    public void Initialization()
    {
    }
}