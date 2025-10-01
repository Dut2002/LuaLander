using UnityEngine;

public class LanderAudio : MonoBehaviour
{
    [SerializeField] private AudioSource thrusterAudioSource;

    private void Start()
    {
        Lander.Instance.OnBeforeForce += Lander_OnBeforeForce;
        Lander.Instance.OnUpForce += Lander_OnUpForce; ;
        Lander.Instance.OnLeftForce += Lander_OnLeftForce; ;
        Lander.Instance.OnRightForce += Lander_OnRightForce;
        SoundManager.Instance.OnSoundVolumeChanged += Lander_OnSoundVolumeChanged;
        thrusterAudioSource.Pause();
    }

    private void Lander_OnSoundVolumeChanged(object sender, System.EventArgs e)
    {
        thrusterAudioSource.volume = SoundManager.Instance.GetSoundVolume();
    }

    private void Lander_OnRightForce(object sender, System.EventArgs e)
    {
        if (!thrusterAudioSource.isPlaying)
        {
            thrusterAudioSource.Play();
        }
    }

    private void Lander_OnLeftForce(object sender, System.EventArgs e)
    {
        if (!thrusterAudioSource.isPlaying)
        {
            thrusterAudioSource.Play();
        }
    }

    private void Lander_OnUpForce(object sender, System.EventArgs e)
    {
        if (!thrusterAudioSource.isPlaying)
        {
            thrusterAudioSource.Play();
        }
    }

    private void Lander_OnBeforeForce(object sender, System.EventArgs e)
    {
        thrusterAudioSource.Pause();
    }
}
