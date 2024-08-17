using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class LoadVolume : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private string paramName;
    [SerializeField] private Slider slider;    

    void Start()
    {
        float volume = PlayerPrefs.GetFloat(mixer.name + paramName, 0.5f);
        if(slider)
        {
            slider.value = volume;
        }
    }
}
