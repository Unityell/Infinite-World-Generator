using UnityEngine;
using Zenject;

public class MusicManager : MonoBehaviour
{
    [Inject] EventBus EventBus;
    [SerializeField] AudioSource[] AudioSource;
    [SerializeField] AudioClip[] Clips;

    void Start()
    {
        EventBus.Subscribe(SignalBox);

        AudioSource[0].clip = Clips[Random.Range(0, Clips.Length)];
        AudioSource[0].Play();  
    }

    void SignalBox(object Obj)
    {
        switch (Obj)
        {
            case EnumSignals.StopMusic:
            case EnumSignals.GameOver :
                for (int i = 0; i < AudioSource.Length; i++)
                {
                    AudioSource[i].Stop();                    
                }
                break;
            case EnumSignals.PlayMusic:
            case EnumSignals.StartGame :
                AudioSource[0].clip = Clips[Random.Range(0, Clips.Length)];
                AudioSource[0].Play();  
                AudioSource[1].Play();                              
                break;
            default: break;
        }
    }

    void OnDestroy() => EventBus.Unsubscribe(SignalBox);
}