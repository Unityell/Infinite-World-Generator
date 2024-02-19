using UnityEngine;

public class Decoration : Unit
{
    [SerializeField, Range(0, 100)] 
    public float SpawnChance;
    public float SpawnDistanceMin;
    public float SpawnDistanceMax;
    public float ScaleMin;
    public float ScaleMax;
}