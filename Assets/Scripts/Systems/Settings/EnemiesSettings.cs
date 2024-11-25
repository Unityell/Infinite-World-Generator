using UnityEngine;

[System.Serializable]
public class EnemiesSettings
{
    //public Animal Prefab;
    [Range(0, 100)] public float SpawnChance;
    public float ScaleMin;
    public float ScaleMax;
}