using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicControlScript : MonoBehaviour
{
    AudioSource myAudioSource;

    float musicVolume = 1f;

    void Start()
    {
        myAudioSource = GetComponent<AudioSource>();
    }

   void Update()
    {
        myAudioSource.volume = musicVolume;
    }

    public void Volume(float volume)
    {
        musicVolume = volume;
    }
}