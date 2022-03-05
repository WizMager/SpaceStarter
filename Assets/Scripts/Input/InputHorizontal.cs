using System;
using UnityEngine;

public class InputHorizontal : IUserInput<float>
{
    public event Action<float> OnChange;
    
    public void GetInput()
    {
        OnChange?.Invoke(Input.GetAxis("Horizontal"));
    }
}