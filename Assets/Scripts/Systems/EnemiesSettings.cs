using UnityEngine;

[System.Serializable]
public class EnemiesSettings
{
    public Enemy Prefab;
    [Range(0, 100)] public float SpawnChance;
    public float ScaleMin;
    public float ScaleMax;
}