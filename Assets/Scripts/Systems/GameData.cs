using UnityEngine;

public class GameData
{
    public delegate void GameDataEvent(object Message);
    public event GameDataEvent ChangePauseState;
    public event GameDataEvent ChangeAudioState;

    public bool IsPaused {get; private set;}
    public bool AudioState {get; private set;}
    public float Volume;
    public bool GameOver;

    public void SetPause(bool Switch)
    {
        Time.timeScale = Switch == true ? 0 : 1;
        IsPaused = Switch;
        ChangePauseState?.Invoke(Switch);
        SetMouseVisualMode(Switch);
    }

    public void SetAudioState(bool Switch)
    {
        AudioState = Switch;
        ChangeAudioState?.Invoke(Switch);
    }

    public void SetMouseVisualMode(bool Visible)
    {
        if(Visible)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;            
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}