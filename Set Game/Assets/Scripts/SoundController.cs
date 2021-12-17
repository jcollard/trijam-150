using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public static SoundController Instance;

    void Start()
    {
        Instance = this;
    }
    
    public AudioSource Success;
    public AudioSource Failure;
    public AudioSource Discard;
    public AudioSource Music;
    public UnityEngine.UI.Slider MusicBar;
    public UnityEngine.UI.Slider VolumeBar;

    public void SetVolume()
    {
        Success.volume = VolumeBar.value;
        Failure.volume = VolumeBar.value;
        Discard.volume = VolumeBar.value;
        Music.volume = MusicBar.value;
    }

}
