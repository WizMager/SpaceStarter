using System;
using UnityEngine;

public class InputTouchUp : IUserInput<Vector3>
{
    public event Action<Vector3> OnChange;
    public void GetInput()
    {
        if (Input.GetMouseButtonUp(0))
        {
            OnChange?.Invoke(Input.mousePosition);
        }
    }
}