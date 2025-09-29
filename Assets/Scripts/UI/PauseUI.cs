using UnityEngine;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button quitButton;

    private void Awake()
    {
        resumeButton.onClick.AddListener(() =>
        {
            GameManager.Instance.UnPauseGame();
        });

        restartButton.onClick.AddListener(() => {
            GameManager.Instance.UnPauseGame();
            GameManager.Instance.RetryLevel();
        });
        quitButton.onClick.AddListener(() => GameManager.Instance.GoMainMenu());
    }

    private void Start()
    {
        GameManager.Instance.OnGamePaused += GameManager_OnGamePaused;
        GameManager.Instance.OnGameUnPaused += GameManager_OnGameUnPaused;
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
    }

    private void Hide()
    {
        this.gameObject.SetActive(false);
    }
}
