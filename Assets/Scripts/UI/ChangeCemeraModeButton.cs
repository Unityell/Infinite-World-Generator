using UnityEngine;
using UnityEngine.UI;

public class ChangeCemeraModeButton : EventBusSignaler
{
    private EnumCameraMode State;
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
            case EnumCameraMode.Game :
                Image.sprite = GameModeIcon;
                State = EnumCameraMode.Update;
                break;
            case EnumCameraMode.Update :
                Image.sprite = UpdateModeIcon;
                State = EnumCameraMode.Game;
                break;
            default: break;
        }

        CameraModeSignal Signal = new CameraModeSignal(State);
        EventBus.Invoke(Signal);
    }
}