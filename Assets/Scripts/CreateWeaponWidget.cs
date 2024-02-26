using UnityEngine;
using UnityEngine.UI;

public class CreateWeaponWidget : Widgets
{
    [SerializeField] Button CreateButton;
    [SerializeField] GameObject PickImage;
    Pivot CurrentPivot;
    Weapon CurrentWeaponPrefab;
    int CurrentPrice;
    const string CloseInfoPannel = "CloseInfoPannel";

    void OnEnable()
    {
        CreateButton.interactable = false;
    }

    void Start()
    {
        Subscribe();
    }

    protected override void SignalBox(object Obj)
    {
        if(Obj.GetType() == typeof(PivotSignal))
        {
            PickImage.SetActive(false);
            PivotSignal Signal = Obj as PivotSignal;

            CurrentPivot = Signal.Pivot;

            Enable(true);
        }

        if(Obj.GetType() == typeof(CreateWeaponSignal))
        {
            CreateWeaponSignal Signal = Obj as CreateWeaponSignal;
            
            CurrentWeaponPrefab = Signal.Prefab;
            CurrentPrice = Signal.Price;

            CreateButton.interactable = true;
        }

        if(Obj.GetType() == typeof(GameModeSignal))
        {
            Enable(false);
        }
    }

    public void Create()
    {
        if(PlayerPrefs.GetInt("Gold") > CurrentPrice)
        {
            PlayerPrefs.SetInt("Gold", PlayerPrefs.GetInt("Gold") - CurrentPrice);
            PlayerPrefs.Save();
            Instantiate(CurrentWeaponPrefab, CurrentPivot.transform.position, Quaternion.identity);
            Enable(false);
        }
        
        EventBus.Invoke(CloseInfoPannel);
        CurrentPivot.State = EnumWeaponPivotState.Active;
        CurrentPivot.gameObject.SetActive(false);
        Instantiate(CurrentWeaponPrefab, CurrentPivot.transform.position, Quaternion.identity);
        Enable(false);
    }
}