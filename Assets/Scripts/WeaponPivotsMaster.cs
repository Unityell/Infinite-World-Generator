using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class WeaponPivotsMaster : MonoBehaviour
{
    [Inject] EventBus EventBus;
    [SerializeField, ReadOnly] List<Pivot> Pivots; 

    void Start()
    {
        EventBus.Event += SignalBox;
    }

    void Create(Pivot Pivot, Weapon Prefab)
    {
        Pivot.State = EnumWeaponPivotState.Active;
        var Weapon = Instantiate(Prefab, Pivot.transform.position, Quaternion.identity);
        Weapon.Initialization(EventBus);
        EventBus.Invoke(new RefreshUpdateWidgets());
    }

    void SignalBox(object Obj)
    {
        Type Type = Obj.GetType();

        if(Type == typeof(GameModeSignal))
        {
            GameModeSignal Signal = Obj as GameModeSignal; 

            if(Signal.State == EnumGameMode.Update)
            {
                WeaponPivotsSignal PivotsSignal = new WeaponPivotsSignal(Pivots.FindAll(x => x.State == EnumWeaponPivotState.Ready));
                EventBus.Invoke(PivotsSignal);
            }
        }

        if(Type == typeof(CreateWeaponSignal))
        {
            CreateWeaponSignal Signal = Obj as CreateWeaponSignal;

            if(PlayerPrefs.GetInt("Gold") >= Signal.Price)
            {
                Create(Signal.Pivot, Signal.Prefab);
            }
        }
    }
}