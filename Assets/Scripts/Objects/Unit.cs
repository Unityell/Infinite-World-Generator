using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public abstract class Unit : BaseSignal
{
    [ReadOnly] public HashSet<object> Systems = new HashSet<object>();
    [SerializeField, ReadOnly] public HashSet<Components> Components = new HashSet<Components>();

    public virtual void Initialization(HashSet<object> Systems){}

    public T GetComponentByType<T>() where T : Components
    {
        foreach (var component in Components)
        {
            if (component is T)
            {
                return component as T;
            }
        }
        return null;
    }

    public T GetSystemByType<T>() where T : class
    {
        return Systems.OfType<T>().FirstOrDefault();
    }

    protected virtual void SubsribeOnComponentsSignals()
    {
        foreach (var Component in Components)
        {
            if(!Component) continue;
            Component.MySignal += SignalBox;            
        }
    }

    protected virtual void UnSubscribeOnComponentsSignals()
    {
        foreach (var Component in Components)
        {
            if(!Component) continue;
            Component.MySignal -= SignalBox;
        }
    }

    protected virtual void InitComponents()
    {
        foreach (var Component in Components)
        {
            Component.Initialization(this);
        }
    }

    protected virtual void ExtractSystems(HashSet<object> Systems){}
    protected virtual void ExtractComponents(HashSet<Components> Components){}

    protected virtual void SignalBox(object Obj){}

    void OnDestroy() => UnSubscribeOnComponentsSignals();
}