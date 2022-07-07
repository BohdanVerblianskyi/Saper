using System.Collections.Generic;
using UnityEngine;

public class CellController
{
    private readonly Game _game;
    private readonly CellFactory _cellFactory;
    private readonly int _bombCount;
    private readonly Player _player;
    private readonly Vector2Int _gridSize;
    private readonly IInitCell[,] _cells;
    private readonly List<IInitCell> _cellBomb = new List<IInitCell>();
    private int _cellCount;
    
    public CellController(Game game, CellFactory cellFactory, Vector2Int gridSize, int bombCount, Player player)
    {
        _game = game;
        _cellFactory = cellFactory;
        _gridSize = gridSize;
        _bombCount = bombCount;
        _cellCount = gridSize.x * gridSize.y;
        _player = player;
        _cells = new IInitCell[gridSize.x, gridSize.y];
        _player.OnFirstClick += OnFirstClick;
        CreateCells();
        InitCell();
    }

    private void OnFirstClick(IClick click)
    {
        if (click is IInitCell cell)
        {
            InitBomb(cell);
            _player.OnFirstClick -= OnFirstClick;
        }
    }

    private void InitBomb(IInitCell except)
    {
        int bombCount = _bombCount;

        while (bombCount > 0)
        {
            int randomX = Random.Range(0, _gridSize.x);
            int randomY = Random.Range(0, _gridSize.y);

            IInitCell randomCell = _cells[randomX, randomY];
            if (randomCell != except && !_cellBomb.Contains(randomCell))
            {
                randomCell.AddBomb();
                _cellBomb.Add(randomCell);
                randomCell.OnOpenEvent -= OnOpen;
                randomCell.OnOpenEvent += OnOpenBomb;
                _cellCount--;
                bombCount--;
            }
        }
    }

    private void OnOpenBomb()
    {
        foreach (IInitCell cellBomb in _cellBomb)
        {
            cellBomb.OnOpenEvent -= OnOpenBomb;
            cellBomb.Open();
        }
        _game.StopGame(false);
    }

    private void OnOpen()
    {
        _cellCount--;
        if (_cellCount < 0)
        {
          _game.StopGame(true);   
        } 
    }

    private void InitCell()
    {
        for (int x = 0; x < _gridSize.x; x++)
        {
            for (int y = 0; y < _gridSize.y; y++)
            {
                _cells[x, y].Init(GetNeighbors(new Vector2Int(x, y)));
            }
        }
    }

    private List<IInitCell> GetNeighbors(Vector2Int index)
    {
        List<IInitCell> cells = new List<IInitCell>();
        
        for (int x = index.x - 1; x < index.x + 2; x++)
        {
            for (int y = index.y - 1; y < index.y + 2; y++)
            {
                if (WithinTheGrid(x, y) && index != new Vector2Int(x, y))
                {
                    cells.Add(_cells[x, y]);
                }
            }
        }

        return cells;
    }

    private bool WithinTheGrid(int x, int y)
    {
        if (x < 0 || x >= _gridSize.x || y < 0 || y >= _gridSize.y)
        {
            return false;
        }

        return true;
    }

    private void CreateCells()
    {
        Vector2 offset = (Vector2)_gridSize / 2f - new Vector2(0.5f, 0.5f);

        for (int x = 0; x < _gridSize.x; x++)
        {
            for (int y = 0; y < _gridSize.y; y++)
            {
                Vector2 position = new Vector2(x, y) - offset;
                IInitCell cell = _cellFactory.Create(position);
                cell.OnOpenEvent += OnOpen;
                _cells[x, y] = cell;
            }
        }
    }
}