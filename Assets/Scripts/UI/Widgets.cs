using UnityEngine;
using Zenject;

public abstract class Widgets : MonoBehaviour
{
    [Inject] protected EventBus EventBus;

    [Header("Widget")]
    [SerializeField] 
    protected GameObject        Widget;

    protected virtual void Subscribe()
    {
        EventBus.Event += SignalBox;
    }

    protected virtual void Unsubscribe()
    {
        EventBus.Event -= SignalBox;
    }

    protected abstract void SignalBox(object Obj);

    public void Enable(bool Switch)
    {
        Widget.SetActive(Switch);
    }
    
    private void OnDestroy()
    {
        Unsubscribe();
    }
}