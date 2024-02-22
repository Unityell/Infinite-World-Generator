using UnityEngine;

public class Enemy : Unit
{
    [SerializeField, Range(0, 100)] 
    public float SpawnChance;
    public float SpawnDistanceMin;
    public float SpawnDistanceMax;
}