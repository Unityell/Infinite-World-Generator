public class CreateWeaponSignal
{
    public int Price;
    public Weapon Prefab;
    public Pivot Pivot;

    public CreateWeaponSignal(int Price, Weapon Prefab, Pivot Pivot)
    {
        this.Price = Price;
        this.Prefab = Prefab;
        this.Pivot = Pivot;
    }
}