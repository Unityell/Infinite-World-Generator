public class EventBus
{
    public delegate void Action(object obj);
    public event Action Event;
    public void Invoke(object obj)
    {
        Event?.Invoke(obj);
    }
}