using System;
using UnityEngine;

namespace InputClasses
{
    public class InputHorizontal : IUserInput<float>
    {
        public event Action<float> OnChange;

        public void GetInput()
        {
            OnChange?.Invoke(UnityEngine.Input.GetAxis("Horizontal"));
        }
    }
}