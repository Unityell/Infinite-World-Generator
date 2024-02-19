using UnityEngine;
using TMPro;

public class DistanceWidget : Widgets
{   
    [SerializeField] 
    private Config          _config;
    [SerializeField] 
    private TextMeshProUGUI _text;

    protected override void SignalBox(object Obj)
    {
        throw new System.NotImplementedException();
    }

    private void FixedUpdate()
    {
        _text.text = $"{Mathf.FloorToInt(_config.CurrentDistance)} m";
    }
}