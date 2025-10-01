using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private static int levelNumber = 1;
    private static int totalScore = 0;

    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnPaused;

    [SerializeField] private List<GameLevel> gameLevels;
    [SerializeField] private CinemachineCamera cinemachineCamera;

    public static GameManager Instance { get; private set; }

    private int score;
    private float time;
    private bool isTimerActive;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Lander.Instance.OnCoinPickup += Lander_OnCoinPickup;
        Lander.Instance.OnLanded += Lander_OnLanded;
        Lander.Instance.OnStateChanged += Lander_OnStateChanged;
        GameInput.Instance.OnMenuButtonPress += GameInput_OnMenuButtonPress;

        LoadCurrentLevel();
    }

    private void GameInput_OnMenuButtonPress(object sender, EventArgs e)
    {
        PauseUnPauseGame();
    }

    private void LoadCurrentLevel()
    {
        GameLevel gameLevel = GetGameLevel();
        GameLevel spawnedGameLevel = Instantiate(gameLevel, Vector3.zero, Quaternion.identity);
        Lander.Instance.transform.position = spawnedGameLevel.GetLanderStartPosition();
        cinemachineCamera.Target.TrackingTarget = gameLevel.GetCameraStartPositionTransform();
        CinemachineCameraZoom2D.Instance.SetTargetOrthographicSize(spawnedGameLevel.GetZoomOutOthorgrahicSize());
        return;
    }

    private GameLevel GetGameLevel()
    {
        foreach (var gameLevel in gameLevels)
        {
            if (levelNumber == gameLevel.GetLevel())
            {
                return gameLevel;
            }
        }
        return null;
    }

    private void Lander_OnStateChanged(object sender, Lander.OnStateChangedEventArgs e)
    {
        isTimerActive = e.state == Lander.State.Normal;
        if (isTimerActive)
        {
            cinemachineCamera.Target.TrackingTarget = Lander.Instance.transform;
            CinemachineCameraZoom2D.Instance.SetTargetOrthographicSizeNormal();
        }
    }

    private void Update()
    {
        if (isTimerActive)
        {
            time += Time.deltaTime;
        }
    }

    private void Lander_OnLanded(object sender, Lander.OnLandedEventArgs e)
    {
        AddScore(e.score);
    }

    private void Lander_OnCoinPickup(object sender, System.EventArgs e)
    {
        AddScore(500);
    }

    public void AddScore(int addScoreAmount)
    {
        score += addScoreAmount;
    }

    public int GetLevelNumber() => levelNumber;
    public int GetScore() => score;
    public int GetTotalScore() => totalScore;
    public float GetTime() => time;

    public void RetryLevel()
    {
        SceneLoader.LoadScene(SceneLoader.Scene.GameScene);
    }

    public void GoToNextLevel()
    {
        levelNumber++;
        totalScore += score;
        if(GetGameLevel() == null)
        {
            GoToGameOver();
        }
        else
        {
            SceneLoader.LoadScene(SceneLoader.Scene.GameScene);
        }
    }

    public void GoToGameOver()
    {
        SceneLoader.LoadScene(SceneLoader.Scene.GameOverScene);
    }

    public void GoMainMenu()
    {
        SceneLoader.LoadScene(SceneLoader.Scene.MainMenuScene);
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        OnGamePaused?.Invoke(this, EventArgs.Empty);
    }
    public void UnPauseGame()
    {
        Time.timeScale = 1f;
        OnGameUnPaused?.Invoke(this, EventArgs.Empty);
    }

    private void PauseUnPauseGame()
    {
        if(Time.timeScale == 1f)
        {
            PauseGame();
        }
        else
        {
            UnPauseGame();
        }
    }

    internal static void ResetStaticData()
    {
        levelNumber = 1;
        totalScore = 0;
    }
}
