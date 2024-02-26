using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Zenject;

public class InfoPannel : Widgets
{
    [Inject] GameInfo GameInfo;
    [SerializeField] TextMeshProUGUI NameText;
    [SerializeField] TextMeshProUGUI HintText;
    [SerializeField] TextMeshProUGUI PriceText;
    [SerializeField] Image Icon;

    void Start()
    {
        Subscribe();
    }

    protected override void SignalBox(object Obj)
    {
        switch (Obj)
        {
            case "CloseInfoPannel" :
                Enable(false);
                break;
            default: break;
        }
        if(Obj.GetType() == typeof(CreateWeaponSignal))
        {
            CreateWeaponSignal Signal = Obj as CreateWeaponSignal;
            
            Icon.sprite = Signal.Icon;
            PriceText.text = Signal.Price.ToString();

            HintText.text = Signal.Hint.Find(x => x.Lang == GameInfo.GetLang()).Text;
            NameText.text = Signal.Name.Find(x => x.Lang == GameInfo.GetLang()).Text;

            Enable(true);
        }

        if(Obj.GetType() == typeof(PivotSignal) || Obj.GetType() == typeof(GameModeSignal))
        {
            Enable(false);
        }
    }
}