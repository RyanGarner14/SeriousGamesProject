using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class UISound : MonoBehaviour
{
    public AudioSource UISoundsource;
    public AudioSource ManualSoundSource;

    public void PlayButtonSound()
    {
        UISoundsource.Play();
    }

    public void PlaySwitchPageSound()
    {
        ManualSoundSource.Play();
    }
}
