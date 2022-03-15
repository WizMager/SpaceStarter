using System.Collections.Generic;
using InputClasses;
using UnityEngine;
using Utils;

namespace Controller
{
    public class InputController : IExecute
    {
        private readonly IUserInput<Vector3> _inputTouchDown;
        private readonly IUserInput<Vector3> _inputTouchUp;
        private readonly IUserInput<Vector3> _inputTouchHold;
        private readonly IUserInput<SwipeData> _inputSwipe;

        public InputController(IReadOnlyList<IUserInput<Vector3>> touchInput, 
            IUserInput<SwipeData> inputSwipe)
        {
            _inputTouchDown = touchInput[(int) TouchInput.InputTouchDown];
            _inputTouchUp = touchInput[(int) TouchInput.InputTouchUp];
            _inputTouchHold = touchInput[(int) TouchInput.InputTouchHold];
            _inputSwipe = inputSwipe;
        }
        public void Execute(float deltaTime)
        {
            _inputTouchDown.GetInput();
            _inputTouchUp.GetInput();
            _inputTouchHold.GetInput();
            _inputSwipe.GetInput();
        }
    }
}