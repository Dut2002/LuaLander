using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Slider changeSoundVolumeSlider;
    [SerializeField] private Slider changeMusicVolumeSlider;

    private void Awake()
    {
        resumeButton.onClick.AddListener(() =>
        {
            GameManager.Instance.UnPauseGame();
        });

        restartButton.onClick.AddListener(() => {
            GameManager.Instance.RetryLevel();
        });
        quitButton.onClick.AddListener(() => {
            GameManager.Instance.GoToGameOver();
        });
        changeSoundVolumeSlider.onValueChanged.AddListener((float value) => {
            SoundManager.Instance.ChangeSoundVolume(value);
        });
        changeMusicVolumeSlider.onValueChanged.AddListener((float value) => {
            MusicManager.Instance.ChangeMusicVolume(value);
        });

    }

    private void Start()
    {
        GameManager.Instance.OnGamePaused += GameManager_OnGamePaused;
        GameManager.Instance.OnGameUnPaused += GameManager_OnGameUnPaused;

        changeSoundVolumeSlider.value = SoundManager.Instance.GetSoundVolume();
        changeMusicVolumeSlider.value = MusicManager.Instance.GetMusicVolume();
        Hide();
    }
    private void GameManager_OnGamePaused(object sender, System.EventArgs e)
    {
        Show();
    }

    private void GameManager_OnGameUnPaused(object sender, System.EventArgs e)
    {
        Hide();
    }



    private void Show()
    {
        this.gameObject.SetActive (true);
        resumeButton.Select();
    }

    private void Hide()
    {
        this.gameObject.SetActive(false);
    }
}
