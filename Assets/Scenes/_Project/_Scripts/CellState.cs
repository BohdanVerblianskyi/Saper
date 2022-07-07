using System;
using System.Collections.Generic;

public abstract  class CellState: IClick,ICell
{
    public virtual event Action OnOpenEvent;
    protected readonly Cell _cell;

    protected CellState(Cell cell)
    {
        _cell = cell;
    }

    public abstract void Enter();
    public abstract void Exit();
    public abstract void Select();
    public abstract void Deselect();
    public abstract void LeftButtonDown();
    public abstract void LeftButtonUp();
    public abstract void RightButtonDown();

    public abstract void Open();
}