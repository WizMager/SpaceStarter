using System;
using UnityEngine;

public class InputTouch : IUserInput<bool>
{
     
     public event Action<bool> OnChange;
     
     public void GetInput()
     {
          OnChange?.Invoke(Input.GetMouseButtonDown(0));
     }
}