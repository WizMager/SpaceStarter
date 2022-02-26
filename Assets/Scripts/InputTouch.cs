using System;
using UnityEngine;

public class InputTouch : IUserInput<Vector3>
{
     public event Action<Vector3> OnChange;
     
     public void GetInput()
     {
          OnChange?.Invoke(Input.mousePosition);
     }
}