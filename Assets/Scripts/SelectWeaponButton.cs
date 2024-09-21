using UnityEngine;
using UnityEngine.UI;

public class SelectWeaponButton : EventBusSignaler
{
    [SerializeField] WeaponData Weapon;
    Image Image;

    void OnValidate()
    {
        if(!Image) Image = GetComponent<Image>();
        if(Image && Weapon) Image.sprite = Weapon.Icon;
    }

    public override void SendMessage()
    {
        SelectedWeaponInfoSignal Signal = new SelectedWeaponInfoSignal(Weapon.Icon, Weapon.Name, Weapon.Hint, Weapon.Price, Weapon.Prefab);
        EventBus.Invoke(Signal);
    }
}