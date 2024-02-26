using System.Collections.Generic;
using UnityEngine;

public class CreateWeaponButton : EventBusSignaler
{
    [SerializeField] RectTransform PickImage;
    [SerializeField] List<SimpleLocalize> Name;
    [SerializeField] List<SimpleLocalize> Hint;
    [SerializeField] Sprite Icon;
    [SerializeField] int Price;
    [SerializeField] Weapon Prefab;

    public override void SendMessage()
    {
        PickImage.position = transform.position;
        PickImage.gameObject.SetActive(true);
        CreateWeaponSignal Signal = new CreateWeaponSignal(Icon, Name, Hint, Price, Prefab);
        EventBus.Invoke(Signal);
    }
}