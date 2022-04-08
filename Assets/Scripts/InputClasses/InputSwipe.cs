using System;
using Interface;
using UnityEngine;

namespace InputClasses
{
    public class InputSwipe : IUserInput<SwipeData>
    {
        public event Action<SwipeData> OnChange;
        
        private Vector3 _fingerDownPosition;
        private Vector3 _fingerUpPosition;
        private readonly float _minimalDistanceToSwipe;

        public InputSwipe(float minimalDistanceToSwipe)
        {
            _minimalDistanceToSwipe = minimalDistanceToSwipe;
        }
        
        public void GetInput()
        {
            if (Input.touchCount < 1) return;
            
            var touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    _fingerDownPosition = touch.position;
                    _fingerUpPosition = touch.position;
                    break;
                case TouchPhase.Moved:
                    _fingerDownPosition = touch.position;
                    DetectSwipe();
                    break;
                case TouchPhase.Ended:
                    _fingerDownPosition = touch.position;
                    DetectSwipe();
                    break;
                case TouchPhase.Stationary:
                    break;
                case TouchPhase.Canceled:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void DetectSwipe()
        {
            if (!SwipeDistanceCheck()) return;
            
            var swipe = new SwipeData();
            if (IsHorizontalSwipe())
            {
                var value = _fingerDownPosition.x - _fingerUpPosition.x;
                var direction = value > 0
                    ? SwipeDirection.Right
                    : SwipeDirection.Left;
                swipe.Direction = direction;
                swipe.Value = Mathf.Abs(value);
                OnChange?.Invoke(swipe);
            }
            else
            {
                var value = _fingerDownPosition.y - _fingerUpPosition.y;
                var direction = value > 0
                    ? SwipeDirection.Up
                    : SwipeDirection.Down;
                swipe.Direction = direction;
                swipe.Value = Mathf.Abs(value);
                OnChange?.Invoke(swipe);
            }
            _fingerUpPosition = _fingerDownPosition;
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