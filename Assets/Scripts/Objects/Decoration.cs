using UnityEngine;

public class Decoration : Unit
{
    [HideInInspector] public float ScaleMin;
    [HideInInspector] public float ScaleMax;

    public void Initialization(float ScaleMin, float ScaleMax)
    {
        this.ScaleMin = ScaleMin;
        this.ScaleMax = ScaleMax;
    }
}