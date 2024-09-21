using UnityEngine;

[System.Serializable]
public class DecorationSettings
{
    public Decoration Prefab;
    [Range(0, 100)] public float SpawnChance;
    public float ScaleMin;
    public float ScaleMax;
}