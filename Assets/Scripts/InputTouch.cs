﻿using System;
using UnityEngine;

public class InputTouch : IUserInput<Vector3>
{
     public event Action<Vector3> OnChange;
     
     public void GetInput()
     {
          if (Input.GetMouseButtonDown(0))
          {
               OnChange?.Invoke(Input.mousePosition);   
          }
     }
}