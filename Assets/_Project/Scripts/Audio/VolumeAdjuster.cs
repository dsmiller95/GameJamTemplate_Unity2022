using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeAdjuster : MonoBehaviour
{
    public AudioMixer masterMixer;
    
    public float minDB = -80;
    public float maxDB = 20;

    public Slider[] slidersToResetOnStart;

    private void Start()
    {
        var toSetVal = InvertScaledVolume(0f);
        foreach (Slider slider in slidersToResetOnStart)
        {
            slider.value = toSetVal;
        }
    }

    public void SetMasterVolume(float volume)
    {
        masterMixer.SetFloat ("MasterVolume", GetScaledVolume(volume));
    }
    
    public void SetMusicVolume(float volume)
    {
        masterMixer.SetFloat ("MusicVolume", GetScaledVolume(volume));
    }
    
    public void SetSFXVolume(float volume)
    {
        masterMixer.SetFloat ("SfxVolume", GetScaledVolume(volume));
    }

    private float GetScaledVolume(float normalizedVolume)
    {
        return Mathf.Lerp(minDB, maxDB, Mathf.Sqrt(normalizedVolume));
    }
    
    private float InvertScaledVolume(float absoluteVolume)
    {
        var normalized = Mathf.InverseLerp(minDB, maxDB, absoluteVolume);
        return Mathf.Pow(normalized, 2);
    }
}
