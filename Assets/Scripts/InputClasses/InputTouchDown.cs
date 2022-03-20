using System;
using Interface;
using UnityEngine;

namespace InputClasses
{
     public class InputTouchDown : IUserInput<Vector3>
     {
          public event Action<Vector3> OnChange;

          public void GetInput()
          {
               if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
                    OnChange?.Invoke(Input.GetTouch(0).position);
          }
     }
}