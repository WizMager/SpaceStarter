using UnityEngine;

public class InputController : IExecute
{
    private readonly IUserInput<float> _inputVertical;
    private readonly IUserInput<float> _inputHorizontal;
    private readonly IUserInput<Vector3> _inputTouch;

    public InputController((IUserInput<float> inputInputVertical, IUserInput<float> inputInputHorizontal, IUserInput<Vector3> inputInputTouch) allInput)
    {
        var (inputInputVertical, inputInputHorizontal, inputInputTouch) = allInput;
        _inputVertical = inputInputVertical;
        _inputHorizontal = inputInputHorizontal;
        _inputTouch = inputInputTouch;
    }
    public void Execute(float deltaTime)
    {
        _inputVertical.GetInput();
        _inputHorizontal.GetInput();
        _inputTouch.GetInput();
    }
}