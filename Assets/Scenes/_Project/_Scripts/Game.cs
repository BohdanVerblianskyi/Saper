using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class Game : MonoBehaviour
{
    [SerializeField] private CellController cellController;
    [SerializeField] private Cell cellPrefab;
    [SerializeField,Range(3,20)] private int _hightGrid;
    [SerializeField,Range(0.1f,0.6f)] private float _bombProcent;
    
    private void Start()
    {
        ChangeCameraSize();
        Vector2Int sizeGrid = new Vector2Int((_hightGrid - 1) * 2,_hightGrid);
        int bombsCount = (int)((sizeGrid.x * sizeGrid.y) * _bombProcent);
        cellController.Init(this,cellPrefab,sizeGrid,bombsCount);
    }

    private void ChangeCameraSize()
    {
        Camera.main.orthographicSize = (_hightGrid /1.8f);
    }

    public void Victory()
    {
        Debug.Log("GG");
    }

    public void Failure()
    { 
        Debug.Log("F");
    }
    
}