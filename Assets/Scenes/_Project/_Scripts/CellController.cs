using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using Random = UnityEngine.Random;

public class CellController : MonoBehaviour
{
    private Vector2Int _size;
    private Cell[,] _calls;
    private int _bombsCount;
    private bool _isInitBomb = false;
    private Game _game;
    private int _emptyCellCount;
    
    public void Init(Game game, Cell prefab, Vector2Int size, int bombsCount)
    {
        _game = game;
        _size = size;
        _emptyCellCount = _size.x * _size.y;
        _calls = new Cell[_size.x, _size.y];
        _bombsCount = bombsCount;
        CreateCells(prefab);
        InitCells();
    }

    private void OnClick(Cell cell)
    {
        if (!_isInitBomb)
        {
            InitBomb(cell);
            _isInitBomb = true;
        }

        OpenCell(cell);
    }

    private void OnClickBomb(Cell cell)
    {
        _game.Failure();
        OpenAllCell();
    }

    private void OpenCell(Cell cell)
    {
        cell.Open();
        _emptyCellCount--;
        
        if (_emptyCellCount == 0)
        {
            _game.Victory();
            OpenAllCell();
        }
        
        if (cell.NeighborsWithBombCount == 0)
        {
            TryOpenNeighborsCells(cell.Index);
        }
    }

    private void TryOpenNeighborsCells(Index indexCentral)
    {
        List<Cell> neighbors = GetNeighbors(indexCentral);
        foreach (Cell neighbor in neighbors)
        {
            if (neighbor.IsOpen)
            {
                continue;
            }
            OpenCell(neighbor);
        }
    }

    private void OpenAllCell()
    {
        for (int x = 0; x < _size.x; x++)
        {
            for (int y = 0; y < _size.y; y++)
            {
                if (_calls[x, y].IsOpen)
                {
                    continue;
                }

                _calls[x, y].Open();
            }
        }
    }

    private void InitBomb(Cell cell)
    {
        while (_bombsCount > 0)
        {
            Index randomIndex = new Index(Random.Range(0, _size.x), Random.Range(0, _size.y));

            Cell randomCell = _calls[randomIndex.X, randomIndex.Y];

            if (cell != randomCell && !randomCell.IsBomb)
            {
                randomCell.AddBomb();
                randomCell.onClick -= OnClick;
                randomCell.onClick += OnClickBomb;
                ReportNeighborsOnBomb(randomCell);
                _emptyCellCount--;
                _bombsCount--;
            }
        }
    }

    private void ReportNeighborsOnBomb(Cell cellWithBomb)
    {
        List<Cell> neighbors = GetNeighbors(cellWithBomb.Index);
        foreach (Cell neighbor in neighbors)
        {
            if (neighbor.IsBomb)
            {
                continue;
            }
            
            neighbor.AddOneNeighborsWithBomb();
        }
    }
    
    private void CreateCells(Cell prefab)
    {
        Vector2 offset = (Vector2)_size / 2f - new Vector2(0.5f, 0.5f);

        for (int x = 0; x < _size.x; x++)
        {
            for (int y = 0; y < _size.y; y++)
            {
                Vector2 position = new Vector2(x, y) - offset;
                _calls[x, y] = Instantiate(prefab, position, Quaternion.identity);
            }
        }
    }

    private void InitCells()
    {
        for (int x = 0; x < _size.x; x++)
        {
            for (int y = 0; y < _size.y; y++)
            {
                _calls[x, y].Init(new Index(x, y));
                _calls[x, y].onClick += OnClick;
            }
        }
    }

    private List<Cell> GetNeighbors(Index index)
    {
        List<Cell> cells = new List<Cell>();

        for (int x = index.X - 1; x < index.X + 2; x++)
        {
            for (int y = index.Y - 1; y < index.Y + 2; y++)
            {
                if (DidNotGoBeyondLimits(new Index(x,y)) && NotTheSame(index,new Index(x,y)))
                {
                    cells.Add(_calls[x, y]);
                }
            }
        }

        return cells;
    }

    private bool NotTheSame(Index a,Index b)
    {
        return !(a.X == b.X && a.Y == b.Y);
    }
    
    private bool DidNotGoBeyondLimits(Index index)
    {
        if (index.X < 0 || index.Y < 0 || index.X >= _size.x || index.Y >= _size.y)
        {
            return false;
        }
        return true;
    }
}