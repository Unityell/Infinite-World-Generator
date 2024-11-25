public class Decoration : Unit
{
    public float ScaleMin {get; private set;}
    public float ScaleMax {get; private set;}

    public void SetupScale(float ScaleMin, float ScaleMax)
    {
        this.ScaleMin = ScaleMin;
        this.ScaleMax = ScaleMax;
    }
}