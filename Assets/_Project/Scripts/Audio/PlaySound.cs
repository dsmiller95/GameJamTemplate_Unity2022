using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PlaySound : MonoBehaviour
{
    [SerializeField] private AudioClip[] sounds;
    [SerializeField] private AudioMixerGroup mixer;
    [SerializeField] private bool playSingleRandom = false;
    [SerializeField] private MinMaxFloat pitchRange;
    [SerializeField] private float volume = 1;

    public void Activate()
    {
        if(pitchRange.min == 0 && pitchRange.max == 0) // Probably wasn't set in the editor
        {
            pitchRange.min = 1;
            pitchRange.max = 1;
        }

        float pitch = Random.Range(pitchRange.min, pitchRange.max);

        if(playSingleRandom)
        {
            SpawnSound(sounds[Random.Range(0, sounds.Length)], transform.position, pitch, volume, mixer).Forget();
        }
        else
        {
            foreach (AudioClip sound in sounds)
            {
                SpawnSound(sound, transform.position, pitch, volume, mixer).Forget();
            }
        }
    }

    private async UniTask SpawnSound(AudioClip clip, Vector3 position, float pitch, float volume, AudioMixerGroup mixer)
    {
        GameObject soundEffect = new("SoundEffect");
        soundEffect.transform.position = position;
        AudioSource source = soundEffect.AddComponent<AudioSource>();
        source.clip = clip;
        source.pitch = pitch;
        source.volume = volume;
        source.outputAudioMixerGroup = mixer;
        source.Play();
        
        //Destroy soundEffect once it finished playing
        await UniTask.WaitUntil(() => !source.isPlaying);
        Destroy(soundEffect);
    }
}
