using System;
using UnityEngine;

public class WeaponPivotsWidget : Widgets
{
    [SerializeField] WeaponButton[] WeaponPivotsButtons;
    [Header("Colors")]
    [SerializeField] Color PickColor;
    [SerializeField] Color UnPickColor;
    Camera Camera;

    void Start()
    {
        Subscribe();
        Camera = Camera.main;
    }

    protected override void SignalBox(object Obj)
    {
        Type Type = Obj.GetType();

        if(Type == typeof(GameModeSignal))
        {
            GameModeSignal Signal = Obj as GameModeSignal; 

            if(Signal.State == EnumGameMode.Game)
            {
                for (int i = 0; i < WeaponPivotsButtons.Length; i++)
                {
                    WeaponPivotsButtons[i].gameObject.SetActive(false);
                    WeaponPivotsButtons[i].ChangeColor(UnPickColor);
                }
            }

            Enable(false);
        }

        if(Type == typeof(WeaponPivotsSignal))
        {
            WeaponPivotsSignal Signal = Obj as WeaponPivotsSignal; 

            for (int i = 0; i < Signal.Pivots.Count; i++)
            {
                WeaponPivotsButtons[i].Setup(Signal.Pivots[i].transform, Camera, Signal.Pivots[i], PickColor, UnPickColor);
                WeaponPivotsButtons[i].gameObject.SetActive(true);
            }

            Enable(true);
        }
    }
}