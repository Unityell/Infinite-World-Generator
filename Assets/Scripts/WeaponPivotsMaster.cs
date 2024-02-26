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

    void SignalBox(object Obj)
    {
        if(Obj.GetType() == typeof(GameModeSignal))
        {
            GameModeSignal Signal = Obj as GameModeSignal; 

            if(Signal.State == EnumGameMode.Update)
            {
                WeaponPivotsSignal PivotsSignal = new WeaponPivotsSignal(Pivots.FindAll(x => x.State == EnumWeaponPivotState.Ready));
                EventBus.Invoke(PivotsSignal);
            }
        }
    }
}