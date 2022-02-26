using System;

public interface IUserInput<T>
{
    event Action<T> OnChange;
    void GetInput();
}