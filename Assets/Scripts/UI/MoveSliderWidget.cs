using UnityEngine;
using UnityEngine.UI;

public class MoveSliderWidget : Widgets
{
    [SerializeField] 
    private Slider Slider;
    [SerializeField] 
    private Config Config;

    private void Start()
    {
        Slider.maxValue = Config.MaxGameSpeed;
        Slider.value = 0;

        Slider.onValueChanged.AddListener(delegate {OnSliderValueChange();});
    }

    protected override void SignalBox(object Obj)
    {
        throw new System.NotImplementedException();
    }

    private void OnSliderValueChange()
    {
        Config.TargetGameSpeed = Slider.value;
    }
}