using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour, IClick
{
    public event Action<Cell> onClick;

    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private CellSprites _cellSprites;

    private int _maskSpriteCounter = 0;
    
    public int NeighborsWithBombCount { get; private set; }
    public Index Index { get; private set; }
    public bool IsBomb { get; private set; }
    public bool IsOpen { get; private set; }

    public void Init(Index index)
    {
      Index = index;
      IsOpen = false;
    }

    public void AddBomb() => IsBomb = true;

    public void AddOneNeighborsWithBomb() => NeighborsWithBombCount++;

    
    public void Open()
    {
        ChengOpenSprite();
        IsOpen = true;
    }

    private void ChangeMarcSprite()
    {
        _maskSpriteCounter++;
        if (_maskSpriteCounter >= _cellSprites.Marks.Count)
        {
            _maskSpriteCounter = 0;
        }

        _spriteRenderer.sprite = _cellSprites.Marks[_maskSpriteCounter];
    }

    private void ChengOpenSprite()
    {
        Sprite sprite;
        if (IsBomb)
        {
            sprite = _cellSprites.Bomb;
        }
        else
        {
            sprite = _cellSprites.Empty[NeighborsWithBombCount];
        }

        _spriteRenderer.sprite = sprite;
    }

    public void Click()
    {
        if (IsOpen)
        {
            return;   
        }
        onClick?.Invoke(this);
    }

    public void AlternativeClick()
    {
        if (IsOpen)
        {
         return;   
        }
        ChangeMarcSprite();
    }
}

public class Index
{
    public int X { get; private set; }
    public int Y { get;private set; }

    public Index(int x, int y)
    {
        X = x;
        Y = y;
    }
}