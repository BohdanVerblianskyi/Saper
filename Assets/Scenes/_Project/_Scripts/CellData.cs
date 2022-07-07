using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CellData : ScriptableObject
{
    public Sprite close;
    public List<Sprite> open;
    public Sprite bomb;
    public Sprite mark;
    public Sprite press;
}