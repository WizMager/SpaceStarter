using System;

namespace InputClasses
{
        public class InputVertical : IUserInput<float>
        {
                public event Action<float> OnChange;
        
                public void GetInput()
                {
                        OnChange?.Invoke(UnityEngine.Input.GetAxis("Vertical"));
                }
        }
}