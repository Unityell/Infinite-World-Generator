using Zenject;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;
using YG;

public class GameOverWidget : Widgets
{    
    [Inject] GameData GameData;
    [SerializeField] Config Config;
    [SerializeField] GameObject ResultPannel;
    [SerializeField] TextMeshProUGUI Score;
    [SerializeField] TextMeshProUGUI Speed;
    [SerializeField] TextMeshProUGUI Distance;
    [SerializeField] GameObject LeaderBoard;

    bool RewardSucces;

    void Start()
    {
        GameData.SetMouseVisualMode(false);
        Subscribe();

        YandexGame.OpenFullAdEvent += OpenFull;
        YandexGame.CloseFullAdEvent += CloseFull;
        YandexGame.FullscreenShow();
    }

    void OpenFull()
    {
        GameData.SetPause(true);
        AudioListener.volume = 0;
        YandexGame.OpenFullAdEvent -= OpenFull;
    }

    void CloseFull()
    {
        GameData.SetPause(false);
        AudioListener.volume = GameData.AudioState ? 1 : 0;
        YandexGame.CloseFullAdEvent -= CloseFull;
    }

    protected override void SignalBox(object Obj)
    {
        switch (Obj)
        {
            case EnumSignals.GameOver :
                Enable(true);
                ResultPannel.SetActive(false);
                GameData.SetPause(true);
                StartCoroutine(Wait());
                break;
            default: break;
        }
    }

    public void StartAD()
    {
        RewardSucces = false;

        YandexGame.RewVideoShow(1);
        YandexGame.RewardVideoEvent += AD;
        YandexGame.CloseVideoEvent += Replay;
    }

    void AD(int Number)
    {
        if (Number == 1)
        {
            if(GameData.IsPaused) GameData.SetPause(true);

            RewardSucces = true;
            YandexGame.RewardVideoEvent -= AD;
            Enable(false);
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSecondsRealtime(1);
        ResultPannel.SetActive(true);
        
        LeaderBoard.SetActive(YandexGame.auth);  

        yield return new WaitForSecondsRealtime(0.5f);

        Score.text += $"<color=#FDE294>{Config.Score}</color>";

        float distance = Config.CurrentDistance;
        
        string distanceText = "";

        if (distance >= 1000)
        {
            distanceText = $"<color=#FDE294>{(Mathf.FloorToInt(distance / 10) / 100f).ToString("F2", System.Globalization.CultureInfo.InvariantCulture)} km</color>";
        }
        else
        {
            distanceText = $"<color=#FDE294>{Mathf.FloorToInt(distance).ToString(System.Globalization.CultureInfo.InvariantCulture)} m</color>";
        }

        Distance.text += distanceText;

        string KmH = YandexGame.lang == "ru" ? "km/ч" : "km/h";

        Speed.text += $"<color=#FDE294>{(Mathf.CeilToInt(Config.CurrentGameSpeed / 10f))} {KmH}</color>";

        if(Config.Score > PlayerPrefs.GetInt("Score"))
        {
            PlayerPrefs.SetInt("Score", Config.Score);
            YandexGame.NewLeaderboardScores("Leaderboard", Config.Score);
            PlayerPrefs.Save();
        }      
    }

    public void Replay()
    {
        GameData.SetPause(false); 
        EventBus.Invoke(EnumSignals.StartGame);

        if(!RewardSucces)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        
        RewardSucces = false;
        YandexGame.CloseVideoEvent -= Replay;
    }
}