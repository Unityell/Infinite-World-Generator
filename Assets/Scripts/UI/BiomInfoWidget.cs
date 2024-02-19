using TMPro;
using UnityEngine;

public class BiomInfoWidget : Widgets
{
    [SerializeField] 
    private TextMeshProUGUI _text;
    [SerializeField] 
    private Animator        _animator;

    private void Start()
    {
        Subscribe();
    }

    protected override void SignalBox(object Obj)
    {
        if(Obj.GetType() == typeof(BiomInfoSignal))
        {
            _animator.Play("BiomInfoAnim", 0, 0);
            BiomInfoSignal Signal = Obj as BiomInfoSignal;
            _text.text = Signal.name;
        }
    }
}