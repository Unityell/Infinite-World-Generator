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
        if(Obj.GetType() == typeof(SelectedWeaponInfoSignal))
        {
            SelectedWeaponInfoSignal Signal = Obj as SelectedWeaponInfoSignal;
            
            Icon.sprite = Signal.Icon;
            PriceText.text = Signal.Price.ToString();

            HintText.text = Signal.Hint.Find(x => x.Lang == GameInfo.GetLang()).Text;
            NameText.text = Signal.Name.Find(x => x.Lang == GameInfo.GetLang()).Text;

            Enable(true);
        }
    }
}