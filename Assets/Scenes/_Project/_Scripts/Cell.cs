using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Cell : MonoBehaviour, IClick, IInitCell
{
    public event Action OnOpenEvent;

    [SerializeField] private CellData data;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private CellState _currentState;
    private Dictionary<Type, CellState> _states;
    private bool _isBomb;

    public int NeighborsBombCount { get; private set; }
    public List<Cell> Neighbors { get; private set; }
    public CellData Data => data;

    public bool IsMark { get; set; }

    public bool IsOpen { get; set; }

    public void Init(List<IInitCell> neighbors)
    {
        NeighborsBombCount = 0;
        Neighbors = new List<Cell>();

        foreach (IInitCell neighbor in neighbors)
        {
            if (neighbor is Cell cell)
            {
                Neighbors.Add(cell);
            }
        }

        _states = new Dictionary<Type, CellState>
        {
            { typeof(CellStateClose), new CellStateClose(this) },
            { typeof(CellStateOpen), new CellStateOpen(this) }
        };

        SwitchState<CellStateClose>();
    }

    public void AddBomb()
    {
        _isBomb = true;

        _states[typeof(CellStateClose)] = new CellStateBomb(this);
        SwitchState<CellStateClose>();
        
        foreach (Cell neighbor in Neighbors)
        {
            if (!neighbor._isBomb)
            {
                neighbor.AddNeighborBomb();
            }
        }
    }

    public void SwitchState<TState>() where TState : CellState
    {
        if (_states.TryGetValue(typeof(TState), out CellState state))
        {
            if (_currentState != null)
            {
                _currentState.Exit();
                _currentState.OnOpenEvent -= OnOpen ;
            }
            
            _currentState = state;
            _currentState.Enter();
            _currentState.OnOpenEvent += OnOpen;
        }
    }

    private void OnOpen()
    {
        OnOpenEvent?.Invoke();
    }
    
    public void SetSprite(Sprite sprite) => _spriteRenderer.sprite = sprite;

    public void Open()
    {
        _currentState.Open();
    }

    private void AddNeighborBomb()
    {
        NeighborsBombCount++;
    }

    public void Select()
    {
        _currentState.Select();
    }

    public void Deselect()
    {
        _currentState.Deselect();
    }

    public void LeftButtonDown()
    {
        _currentState.LeftButtonDown();
    }

    public void LeftButtonUp()
    {
        _currentState.LeftButtonUp();
    }

    public void RightButtonDown()
    {
        _currentState.RightButtonDown();
    }
}