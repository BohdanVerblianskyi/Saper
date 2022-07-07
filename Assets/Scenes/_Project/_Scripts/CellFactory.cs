using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.Mathematics;
using UnityEngine;

public class CellFactory : MonoBehaviour    
{
    [SerializeField] private Cell _prefab;

    private List<Cell> _cells = new List<Cell>();

    public IInitCell Create(Vector3 position)
    {
           Cell cell = Instantiate(_prefab, position, Quaternion.identity,transform);
           _cells.Add(cell);
           return cell;
    }

    public void Clear()
    {
        foreach (var cell in _cells)
        {
            Destroy(cell);
        }
        _cells.Clear();
    }

}