using System;

namespace Interface
{
    public interface IUserInput<T>
    {
        event Action<T> OnChange;
        void GetInput();
    }
}