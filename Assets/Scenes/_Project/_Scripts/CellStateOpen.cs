using System;
using System.Collections.Generic;

public class CellStateOpen : CellState
{
    private readonly List<Cell> _cellsPressed = new List<Cell>();

    public CellStateOpen(Cell cell) : base(cell)
    {
    }

    public override void Enter()
    {
        _cell.IsOpen = true;
        _cell.SetSprite(_cell.Data.open[_cell.NeighborsBombCount]);
    }

    public override void Exit()
    {
    }

    public override void Select()
    {
    }

    public override void Deselect()
    {
        PressOutNeighbors();
    }

    public override void LeftButtonDown()
    {
        TryToPressNeighbors();
    }

    public override void LeftButtonUp()
    {
        TryOpenNeighbors();
        PressOutNeighbors();
    }

    private void TryOpenNeighbors()
    {
        int markCount = 0;
        foreach (Cell neighbor in _cell.Neighbors)
        {
            if (neighbor.IsMark)
            {
                markCount++;
            }
        }

        if (markCount != _cell.NeighborsBombCount)
        {
            return;
        }

        foreach (Cell neighbor in _cell.Neighbors)
        {
            if (FitByPress(neighbor))
            {
                neighbor.Open();
            }
        }
        
        _cellsPressed.Clear();
    }

    private static bool FitByPress(Cell neighbor)
    {
        return !neighbor.IsMark && !neighbor.IsOpen;
    }

    public override void RightButtonDown()
    {
    }

    public override void Open()
    {
    }

    private void TryToPressNeighbors()
    {
        foreach (Cell neighbor in _cell.Neighbors)
        {
            if (FitByPress(neighbor))
            {
                neighbor.SetSprite(_cell.Data.press);
                _cellsPressed.Add(neighbor);
            }
        }
    }

    private void PressOutNeighbors()
    {
        foreach (Cell cell in _cellsPressed)
        {
            cell.SetSprite(_cell.Data.close);
        }
    }
}