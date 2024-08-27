using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMusicController : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip[] musicCollection;
    [SerializeField] private int index = 0;
    [SerializeField] private bool randomSelection = false;

    void Update()
    {
        if (!source.isPlaying)
        {
            if (randomSelection)
            {
                source.clip = musicCollection[Random.Range(0, musicCollection.Length)];
            }
            else
            {
                source.clip = musicCollection[index];
                index++;
                if (index >= musicCollection.Length)
                {
                    index = 0;
                }
            }

            source.Play();
        }
    }
}
