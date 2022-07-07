using System;

public interface ICell
{
    void Open();
    event Action OnOpenEvent;
}