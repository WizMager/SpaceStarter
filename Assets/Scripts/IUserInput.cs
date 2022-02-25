using System;

public interface IUserInput<out T>
{
    event Action<T> OnChange;
    void GetInput();
}