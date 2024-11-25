using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using Zenject;

public class GrabCoinComponent : MonoBehaviour
{
    [Inject] GameData GameData;
    [Inject] AudioManager AudioManager;
    [Inject] EventBus EventBus;
    [SerializeField] AudioSource AudioSource;
    [SerializeField] Config Config;
    [SerializeField] Volume Volume;
    [SerializeField] Volume EffectVolume;

    [Header("Materials")]
    [SerializeField] Material BrickMaterial;
    [SerializeField] Material Bus;
    [SerializeField] ParticleSystem[] Parts;

    void Start() => EventBus.Subscribe(SignalBox);
    bool God;
    float SaveSpeed;

    bool SaveAudioPos;
    bool SavePausePos;
    bool UnFocus;

    void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
        {
            if(UnFocus)
            {
                GameData.SetPause(SavePausePos);
                GameData.SetAudioState(SaveAudioPos); 

                UnFocus = false;             
            }
        }
        else
        {
            UnFocus = true;
            SaveAudioPos = GameData.AudioState;
            SavePausePos = GameData.IsPaused;
            GameData.SetPause(true);
            GameData.SetAudioState(false);         
        }
    }

    void SignalBox(object Obj)
    {
        switch (Obj)
        {
            case EnumSignals.StartGame :
                Config.CurrentGameSpeed = Mathf.Max(Config.CurrentGameSpeed * 0.8f, Config.StartGameSpeed);
                if(Volume.weight > 0) StartCoroutine(RemoveSaturateEffect());
                StartCoroutine(GodTimer(3, false));
                break;
            default: break;
        }
    }

    void OnTriggerEnter(Collider Other)
    {
        if(Other.CompareTag("Coin"))
        {
            Other.gameObject.SetActive(false);
            EventBus.Invoke(new CoinSignal(1));
            AudioManager.PlaySound("Coin");
        }

        if(Other.CompareTag("Obstacle") && !God)
        {
            AudioManager.PlaySound("Crash");
            EventBus.Invoke(EnumSignals.GameOver);
            StartCoroutine(AddSaturateEffect());
        }

        if(Other.CompareTag("God"))
        {
            Other.transform.parent.gameObject.SetActive(false);
            AudioManager.PlaySound("Smeh");
            if(!God) StartCoroutine(GodTimer(10f, true));
        }
    }

    IEnumerator RemoveSaturateEffect()
    {
        while (Volume.weight > 0)
        {
            Volume.weight -= Time.fixedDeltaTime;
            yield return new WaitForSecondsRealtime(Time.fixedDeltaTime);
        }
    }

    IEnumerator AddSaturateEffect()
    {
        while (Volume.weight < 1)
        {
            Volume.weight += Time.fixedDeltaTime;
            yield return new WaitForSecondsRealtime(Time.fixedDeltaTime);
        }
    }

    IEnumerator RemoveSpeedEffect()
    {
        while (EffectVolume.weight > 0)
        {
            EffectVolume.weight -= Time.fixedDeltaTime;
            yield return new WaitForSecondsRealtime(Time.fixedDeltaTime);
        }
    }

    IEnumerator AddSpeedEffect()
    {
        while (EffectVolume.weight < 1)
        {
            EffectVolume.weight += Time.fixedDeltaTime;
            yield return new WaitForSecondsRealtime(Time.fixedDeltaTime);
        }
    }

    IEnumerator GodTimer(float Timer, bool Sound)
    {
        var Renderer = GetComponentInChildren<Renderer>();
        God = true;
        Renderer.material = BrickMaterial;

        if(Sound)
        {
            StartCoroutine(AddSpeedEffect());

            EventBus.Invoke(EnumSignals.StopMusic);

            SaveSpeed = Config.CurrentGameSpeed;
            Config.CurrentGameSpeed = Mathf.Clamp(Config.CurrentGameSpeed + 300, 0, Config.MaxGameSpeed);

            foreach (var item in Parts)
            {
                item.Play();
            }   

            AudioSource.Play();         
        }

        float blinkDuration = 3f;
        float remainingTime = Timer - blinkDuration;

        if (remainingTime > 0)
        {
            yield return new WaitForSecondsRealtime(remainingTime);
        }

        float blinkTime = 0.25f;
        for (float t = 0; t < blinkDuration; t += blinkTime)
        {
            Renderer.material = (t % (blinkTime * 2) < blinkTime) ? BrickMaterial : Bus;
            yield return new WaitForSecondsRealtime(blinkTime);
        }

        Renderer.material = Bus;

        if(Sound)
        {
            StartCoroutine(RemoveSpeedEffect());

            EventBus.Invoke(EnumSignals.PlayMusic);

            foreach (var item in Parts)
            {
                item.Stop();
            }

            Config.CurrentGameSpeed = SaveSpeed;      

            AudioSource.Stop();      
        }

        God = false;
    }

    void OnDestroy() => EventBus.Unsubscribe(SignalBox);
}