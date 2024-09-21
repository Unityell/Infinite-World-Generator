using System;
using UnityEngine;

public class HUD : Widgets
{
    [SerializeField] Widgets SelectWeaponWidget;
    [SerializeField] Widgets InfoPannelWidget;
    [SerializeField] Widgets WeaponPivotsWidget;

    void Start()
    {
        Subscribe();
    }

    protected override void SignalBox(object Obj)
    {
        Type Type = Obj.GetType();

        if(Type == typeof(GameModeSignal))
        {
            SelectWeaponWidget.Enable(false);
            InfoPannelWidget.Enable(false);
            WeaponPivotsWidget.Enable(false);
        }

        if(Type == typeof(RefreshUpdateWidgets))
        {
            SelectWeaponWidget.Enable(false);
            InfoPannelWidget.Enable(false);            
        }
    }
}