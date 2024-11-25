using UnityEngine;
using UnityEngine.UI;
using YG;
using Zenject;

public class PauseWidget : Widgets
{
    [Inject] GameData GameData;
    [SerializeField] GameObject Button;
    [SerializeField] Sprite On;
    [SerializeField] Sprite Off;
    [SerializeField] Button SoundButton;
    [SerializeField] GameObject TabScript;

    void Start()
    {
        TabScript.SetActive(!YandexGame.EnvironmentData.isMobile);
        GameData.ChangePauseState += SignalBox;
    }

    protected override void SignalBox(object Obj)
    {
        Button.SetActive(!(bool)Obj);
    }

    public void Pause()
    {
        GameData.SetPause(true);
        Enable(true);
    }

    public void Unpause()
    {
        GameData.SetPause(false);     
        Enable(false); 
    }

    public void Sound()
    {
        GameData.SetAudioState(!GameData.AudioState);
        SoundButton.image.sprite = GameData.AudioState ? On : Off;
    }

    void OnDestroy()
    {
        GameData.ChangePauseState -= SignalBox;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab) && !GameData.IsPaused)
        {
            Pause();
        }
    }
}