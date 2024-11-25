using UnityEngine;
using TMPro;

public class CountWidget : Widgets
{
    [SerializeField] Config Config;
    [SerializeField] TextMeshProUGUI Text;

    void Start()
    {
        Subscribe();
        Config.Score = 0;
    }

    protected override void SignalBox(object Obj)
    {
        if(Obj is CoinSignal)
        {
            CoinSignal Signal = (CoinSignal)Obj;

            Config.Score += Signal.ExtraCoin;

            Text.text = Config.Score.ToString();
        }
        if(Obj is EnumSignals.GameOver)
        {
            Enable(false);
        }
        if(Obj is EnumSignals.StartGame)
        {
            Enable(true);
        }
    }
}