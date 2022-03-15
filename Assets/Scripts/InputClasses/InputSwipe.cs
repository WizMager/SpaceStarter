using System;
using UnityEngine;

namespace InputClasses
{
    public class InputSwipe : IUserInput<Vector3>
    {
        public event Action<Vector3> OnChange;
        
        private Vector3 _fingerDownPosition;
        private Vector3 _fingerUpPosition;
        private readonly float _minimalDistanceToSwipe;

        public InputSwipe(float minimalDistanceToSwipe)
        {
            _minimalDistanceToSwipe = minimalDistanceToSwipe;
        }
        
        public void GetInput()
        {
            if (Input.touchCount == 1)
            {
                var touch = Input.touches[0];
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        _fingerDownPosition = touch.position;
                        _fingerUpPosition = touch.position;
                        break;
                    case TouchPhase.Moved:
                        _fingerDownPosition = touch.position;
                        break;
                    case TouchPhase.Ended:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
        
        public void DetectSwipe()
        {
            if (SwipeDistanceCheck())
            {
                if (IsHorizontalSwipe())
                {
                    
                }
            }
        }

        private bool SwipeDistanceCheck()
        {
            return HorizontalMovementDistance() > _minimalDistanceToSwipe 
                   || VerticalMovementDistance() > _minimalDistanceToSwipe;
        }

        private float VerticalMovementDistance()
        {
            return Mathf.Abs(_fingerDownPosition.y - _fingerUpPosition.y);
        }
        
        private float HorizontalMovementDistance()
        {
            return Mathf.Abs(_fingerDownPosition.x - _fingerUpPosition.x);
        }

        private bool IsHorizontalSwipe()
        {
            return HorizontalMovementDistance() > VerticalMovementDistance();
        }
    }
}