using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SetVolume : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private string paramName;

    public void SetLevel (float value)
    {
        mixer.SetFloat(paramName, Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat(mixer.name + paramName, value);
    }
}
