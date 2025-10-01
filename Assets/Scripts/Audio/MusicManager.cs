using System;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    public event EventHandler OnMusicVolumeChanged;

    private static float musicTime;

    private AudioSource musicAudioSource;
    private float musicVolume  = .5f;

    private void Awake()
    {
        Instance = this;
        musicAudioSource = GetComponent<AudioSource>();
        musicAudioSource.time = musicTime;
    }

    private void Update()
    {
        musicTime = musicAudioSource.time;
    }

    public void ChangeMusicVolume(float changeMusicValue)
    {
        musicVolume = changeMusicValue;
        musicAudioSource.volume = musicVolume;
        OnMusicVolumeChanged?.Invoke(this, EventArgs.Empty);
    }

    public float GetMusicVolume() { return musicVolume; }

}
