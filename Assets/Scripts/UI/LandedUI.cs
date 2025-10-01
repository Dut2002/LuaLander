using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LandedUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleTextMesh;
    [SerializeField] private TextMeshProUGUI statsTextMesh;
    [SerializeField] private TextMeshProUGUI nextButtonTextMesh;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button gameOverButton;
    private Action NextButtonAction;

    private void Awake()
    {
        nextButton.onClick.AddListener(() =>
        {
            NextButtonAction();
        });
        gameOverButton.onClick.AddListener(() =>
        {
            GameManager.Instance.GoToGameOver();
        });
    }

    private void Start()
    {
        Lander.Instance.OnLanded += Instance_OnLanded;

        Hide();
    }

    private void Instance_OnLanded(object sender, Lander.OnLandedEventArgs e)
    {
        if (e.landingType == Lander.LandingType.Success) {
            titleTextMesh.text = "SUCCESSFUL LANDING!";
            nextButtonTextMesh.text = "CONTINUE";
            NextButtonAction = GameManager.Instance.GoToNextLevel;

        }
        else
        {
            titleTextMesh.text = "<color=#ff0000>CRASH!</color>";
            nextButtonTextMesh.text = "RETRY";
            NextButtonAction = GameManager.Instance.RetryLevel;
        }

        statsTextMesh.text =
            Mathf.Round(e.landingSpeed) + "\n" +
            Mathf.Round(e.dotVector) + "\n" +
            "x" + e.scoreMultiplier + "\n" +
            e.score;
        Show();
    }

    private void Show()
    {
        gameObject.SetActive(true);
        nextButton.Select();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
