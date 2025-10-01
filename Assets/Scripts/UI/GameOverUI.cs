using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private TextMeshProUGUI scoreTextMesh;

    private void Awake()
    {
        mainMenuButton.onClick.AddListener(() =>
        {
            GameManager.Instance.GoMainMenu();
        });

    }

    private void Start()
    {
        scoreTextMesh.text = "Final Score: " + GameManager.Instance.GetTotalScore();
        mainMenuButton.Select();
    }
}
