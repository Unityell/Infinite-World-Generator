using UnityEngine;
using UnityEngine.UI;

public class ChangeGameModeButton : EventBusSignaler
{
    private EnumGameMode State;
    [SerializeField] private Sprite GameModeIcon;
    [SerializeField] private Sprite UpdateModeIcon;
    Image Image;

    void Start()
    {
        Image = GetComponent<Image>();
    }

    public override void SendMessage()
    {
        switch (State)
        {
            case EnumGameMode.Game :
                Image.sprite = GameModeIcon;
                State = EnumGameMode.Update;
                break;
            case EnumGameMode.Update :
                Image.sprite = UpdateModeIcon;
                State = EnumGameMode.Game;
                break;
            default: break;
        }

        GameModeSignal Signal = new GameModeSignal(State);
        EventBus.Invoke(Signal);
    }
}