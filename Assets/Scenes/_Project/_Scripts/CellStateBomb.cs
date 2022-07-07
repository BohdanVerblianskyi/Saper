using System;

public class CellStateBomb : CellStateClose
{
    public override event Action OnOpenEvent;

    public CellStateBomb(Cell cell) : base(cell)
    {
    }

    public override void Open()
    {
        OnOpenEvent?.Invoke();
        if (_cell.IsMark)
        {
            return;
        }
        _cell.SetSprite(_cell.Data.bomb);
    }
}