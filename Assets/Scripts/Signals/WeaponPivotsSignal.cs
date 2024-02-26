using System.Collections.Generic;

public class WeaponPivotsSignal
{
    public List<Pivot> Pivots = new List<Pivot>();

    public WeaponPivotsSignal(List<Pivot> Pivots)
    {
        this.Pivots.AddRange(Pivots);
    }
}