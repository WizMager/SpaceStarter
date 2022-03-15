using UnityEngine;

namespace InputClasses
{
    public class InputInitialization : IInitialization
    {
        private readonly IUserInput<Vector3> _inputTouchDown;
        private readonly IUserInput<Vector3> _inputTouchUp;
        private readonly IUserInput<Vector3> _inputTouchHold;
        private readonly IUserInput<SwipeData> _inputSwipe;

        public InputInitialization(float minimalDistanceToSwipe)
        {
            _inputTouchDown = new InputTouchDown();
            _inputTouchUp = new InputTouchUp();
            _inputTouchHold = new InputTouchHold();
            _inputSwipe = new InputSwipe(minimalDistanceToSwipe);
        }

        public IUserInput<SwipeData> GetSwipe()
        {
            return _inputSwipe;
        }

        public IUserInput<Vector3>[] GetAllTouch()
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
}