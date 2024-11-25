using UnityEngine;
using Zenject;

public class AudioInstaller : MonoInstaller
{   
    [Inject] Factory Factory;
    [Inject] GameData GameData;
    [SerializeField] private AudioData AudioData;
    
    public override void InstallBindings()
    {
        AudioManager AudioManager = new AudioManager(AudioData, Factory, GameData);
        Container.BindInstance<AudioManager>(AudioManager).AsSingle().NonLazy();
    }
}