using UnityEngine;

public class InputController : IExecute
{
    private readonly IUserInput<float> _inputVertical;
    private readonly IUserInput<float> _inputHorizontal;
    private readonly IUserInput<Vector3> _inputTouchDown;
    private readonly IUserInput<Vector3> _inputTouchUp;
    private readonly IUserInput<Vector3> _inputTouchHold;

    public InputController((IUserInput<float> inputInputVertical, IUserInput<float> inputInputHorizontal, 
        IUserInput<Vector3> inputTouchDown, IUserInput<Vector3> inputTouchUp, IUserInput<Vector3> inputTouchHold) allInput)
    {
        var (inputInputVertical, inputInputHorizontal, inputTouchDown, inputTouchUp, inputTouchHold) = allInput;
        _inputVertical = inputInputVertical;
        _inputHorizontal = inputInputHorizontal;
        _inputTouchDown = inputTouchDown;
        _inputTouchUp = inputTouchUp;
        _inputTouchHold = inputTouchHold;
    }
    public void Execute(float deltaTime)
    {
        _inputVertical.GetInput();
        _inputHorizontal.GetInput();
        _inputTouchDown.GetInput();
        _inputTouchUp.GetInput();
        _inputTouchHold.GetInput();
    }
}