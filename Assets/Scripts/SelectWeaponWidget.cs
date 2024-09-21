using System;
using UnityEngine;
using UnityEngine.UI;

public class SelectWeaponWidget : Widgets
{
    [SerializeField] Button CreateButton;
    [SerializeField] GameObject PickImage;
    SelectedWeaponInfoSignal LastSignal;
    Pivot LastPivot;

    void Start()
    {
        Subscribe();
    }

    protected override void SignalBox(object Obj)
    {
        Type Type = Obj.GetType();

        if(Type == typeof(PivotSignal))
        {
            PivotSignal Signal = Obj as PivotSignal;
            LastPivot = Signal.Pivot;
            PickImage.SetActive(false);
            CreateButton.interactable = false;
            Enable(true);
        }

        if(Type == typeof(SelectedWeaponInfoSignal))
        {
            LastSignal = Obj as SelectedWeaponInfoSignal;

            CreateButton.interactable = true;
        }
    }

    public void Create()
    {
        CreateWeaponSignal Signal = new CreateWeaponSignal(LastSignal.Price, LastSignal.Prefab, LastPivot);
        EventBus.Invoke(Signal);
    }
}