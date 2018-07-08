using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class OptionsMenu : MonoBehaviour {

    public AudioMixer audioMixer;

    public void SetVolume(float volume)
    {
        
        audioMixer.SetFloat("GameVolume", volume); 

        Debug.Log(volume);
    }

    public void SetEffectsVolume(float volume)
    {

        audioMixer.SetFloat("EffectsVolume", volume);

        Debug.Log(volume);
    }
}
