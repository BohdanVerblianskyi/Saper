using System;
using System.Collections.Generic;
using UnityEngine;

public class CellStateClose : CellState
{
    public override event Action OnOpenEvent;

    public CellStateClose(Cell cell) : base(cell)
    {
    }

    public override void Enter()
    {
        _cell.SetSprite(_cell.Data.close);
    }

    public override void Exit()
    {
    }

    public override void Select()
    {
        
    }

    public override void Deselect()
    {
        if (_cell.IsMark)
        {
            return;
        }
        _cell.SetSprite(_cell.Data.close);
    }

    public override void LeftButtonDown()
    {
        if (_cell.IsMark)
        {
            return;
        }
        _cell.SetSprite(_cell.Data.press);
    }

    public override void LeftButtonUp()
    {
        _cell.Open();
    }

    public override void RightButtonDown()
    {
        if (_cell.IsMark)
        {
            _cell.IsMark = false;
            _cell.SetSprite(_cell.Data.close);
        }
        else
        {
            _cell.IsMark = true;
            _cell.SetSprite(_cell.Data.mark);            
        }
    }

    public override void Open()
    {
        OnOpenEvent?.Invoke();
        _cell.SwitchState<CellStateOpen>();

        if (_cell.NeighborsBombCount == 0)
        {
            OpenNeighbor();
        }
    }

    private void OpenNeighbor()
    {
        foreach (Cell neighbor in _cell.Neighbors)
        {
            if (!neighbor.IsOpen)
            {
                neighbor.Open();
            }
        }

        _cell.Neighbors.Clear();
    }
}