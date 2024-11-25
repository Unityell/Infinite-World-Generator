using UnityEngine;

public class AudioManager
{
    Factory Factory;
    AudioData AudioData;

    public AudioManager(AudioData AudioData, Factory Factory, GameData GameData)
    {
        this.AudioData = AudioData;
        this.Factory = Factory;
        GameData.SetAudioState(true);
        GameData.ChangeAudioState += SignalBox;
    }

    void SignalBox(object Obj)
    {
        AudioListener.volume = (bool)Obj ? 1 : 0;
    }

    public AudioSource PlaySound(string Name)
    {
        AudioSetting Setting = AudioData.AudioCollection.Find(x => x.Name == Name);

        if(Setting != null)
        {
            var Audio = Factory.CreateByType<AudioEntity>();

            Audio.AudioSource.clip = Setting.Clips[Random.Range(0, Setting.Clips.Length)];

            Audio.AudioSource.volume = Setting.Volume;

            Audio.name = Name;

            Audio.AudioSource.spatialBlend = 0;

            Audio.Play();

            return Audio.AudioSource;
        }
        else
        {
            Debug.Log($"Sound name: {Name} is not find!");

            return null;
        }
    }

    public AudioSource PlaySound(string Name, float spatialBlend, float minDistancem, float maxDistance)
    {
        AudioSetting Setting = AudioData.AudioCollection.Find(x => x.Name == Name);

        if(Setting != null)
        {
            var Audio = Factory.CreateByType<AudioEntity>();

            Audio.AudioSource.clip = Setting.Clips[Random.Range(0, Setting.Clips.Length)];

            Audio.AudioSource.volume = Setting.Volume;

            Audio.name = Name;

            if(spatialBlend > 0)
            {
                Audio.AudioSource.spatialBlend = spatialBlend;
                Audio.AudioSource.minDistance = minDistancem;
                Audio.AudioSource.maxDistance = maxDistance;
            }
            else
            {
                Audio.AudioSource.spatialBlend = 0;
            }

            Audio.Play();

            return Audio.AudioSource;
        }
        else
        {
            Debug.Log($"Sound name: {Name} is not find!");

            return null;
        }
    }
}