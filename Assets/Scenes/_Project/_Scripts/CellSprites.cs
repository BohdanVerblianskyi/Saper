using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CellSprites : ScriptableObject
{
    [SerializeField] private Sprite _bomb;
    [SerializeField] private List<Sprite> _empty;
    [SerializeField] private List<Sprite> _marks;
    
    public Sprite Bomb => _bomb;
    public List< Sprite> Empty => _empty;
    public List<Sprite> Marks => _marks;

}