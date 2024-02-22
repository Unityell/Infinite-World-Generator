using UnityEngine;
using Zenject;

public abstract class EventBusSignaler : MonoBehaviour
{
    [Inject] protected EventBus EventBus;
    public abstract void SendMessage();
}