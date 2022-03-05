using System;
using UnityEngine;

public class InputTouchHold : IUserInput<Vector3>
{
    public event Action<Vector3> OnChange;
    public void GetInput()
    {
        OnChange?.Invoke(Input.mousePosition);
    }
}