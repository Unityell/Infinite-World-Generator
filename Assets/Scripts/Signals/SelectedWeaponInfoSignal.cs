using System.Collections.Generic;
using UnityEngine;

public class SelectedWeaponInfoSignal
{
    public Sprite Icon;
    public List<SimpleLocalize> Name;
    public List<SimpleLocalize> Hint;
    public int Price;
    public Weapon Prefab;

    public SelectedWeaponInfoSignal(Sprite Icon, List<SimpleLocalize> Name, List<SimpleLocalize> Hint, int Price, Weapon Prefab)
    {
        this.Icon = Icon;
        this.Price = Price;
        this.Name = Name;
        this.Hint = Hint;
        this.Prefab = Prefab;
    }
}