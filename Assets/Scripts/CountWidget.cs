using UnityEngine;
using TMPro;

public class CountWidget : Widgets
{
    [SerializeField] TextMeshProUGUI Text;
    int CoinCount;
    void Start()
    {
        Subscribe();
    }

    protected override void SignalBox(object Obj)
    {
        if(Obj is CoinSignal)
        {
            CoinSignal Signal = (CoinSignal)Obj;

            CoinCount += Signal.ExtraCoin;

            Text.text = CoinCount.ToString();
        }
    }
}