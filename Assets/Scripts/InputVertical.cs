using System;
using UnityEngine;

public class InputVertical : IUserInput<float>
{
        public event Action<float> OnChange;
        
        public void GetInput()
        {
                OnChange?.Invoke(Input.GetAxis("Vertical"));
        }
}