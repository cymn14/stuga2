using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private AudioSource levelWonAudioSource;

    [SerializeField]
    private AudioSource soundtrack;

    [SerializeField]
    private GameObject goalIndicatorPrefab;

    [SerializeField]
    private CameraFollow cameraFollow;

    [SerializeField]
    private Timer timer;

    [SerializeField]
    private StartCountdown startCountdown;

    [SerializeField]
    private WinScreen winScreen;

    [SerializeField]
    private GameMenu gameMenu;

    [SerializeField]
    private SceneLoader sceneLoader;

    [SerializeField]
    private GameObject maxSpeedIndicator;

    [SerializeField]
    private LevelSettings levelSettings;

    private List<Goal> goals;
    private List<GoalIndicator> goalIndicators;
    private int goalHitCount = 0;
    private InputAction respawnAction;
    private InputAction showMenuAction;
    private PlayerInput playerInput;
    private GameObject[] ballGameObjects;
    private CarController carController;
    private bool isLevelRunning = false;
    private bool isGameMenuShowing = false;

    private void Awake()
    {
        InitializeVariables();
    }

    private void Start()
    {
        HandleInputs();
        StartLevel();

        // PlayerDataManager.instance.DeleteAll();
    }

    public void GoalHit()
    {
        goalHitCount++;
        UpdateGoalIndicators();

        if (goalHitCount == goals.Count)
        {
            LevelWon();
        }
    }

    private void LevelWon()
    {
        carController.TurnOffCar();
        PauseGame();
        isLevelRunning = false;
        timer.StopTimer();
        PlayerDataManager.instance.UpdateBiggestClearedLevel();
        UpdatePlayerDataAndShowWinScreen();
        levelWonAudioSource.Play();
    }

    private void InitializeVariables()
    {
        playerInput = GetComponent<PlayerInput>();
        respawnAction = playerInput.actions["Respawn"];
        showMenuAction = playerInput.actions["Show Menu"];
        ballGameObjects = GameObject.FindGameObjectsWithTag("Ball");
        maxSpeedIndicator.SetActive(false);

        GameObject carGameObject = GameObject.Find("Car");
        carController = carGameObject.GetComponent<CarController>();

        goals = new List<Goal>();
        goalIndicators = new List<GoalIndicator>();

        foreach (var ringGoalObject in GameObject.FindGameObjectsWithTag("RingGoal"))
        {
            goals.Add(ringGoalObject.GetComponent<Goal>());
        }

        CreateGoalIndicatorGameObjects();
    }

    private void HandleInputs()
    {
        respawnAction.performed += context => Respawn();
        showMenuAction.performed += context => ToggleMenu();
    }

    private void ResetAllBalls()
    {
        foreach (var ballGameObject in ballGameObjects)
        {
            Ball ball = ballGameObject.GetComponent<Ball>();
            ball.Reset();
        }
    }

    private void ResetCar()
    {
        carController.Reset();
    }

    private void ResetCamera()
    {
        cameraFollow.Reset();
    }

    public void FellDown()
    {
        Retry();
    }

    public void Respawn()
    {
        Retry();
    }

    public void Retry()
    {
        isLevelRunning = false;
        goalHitCount = 0;
        timer.StopTimer();
        timer.ResetTimer();

        foreach (var ringGoal in goals)
        {
            ringGoal.Reset();
        }

        ResetAllBalls();
        carController.TurnOffCar();
        ResetCar();
        ResetCamera();
        winScreen.Close();
        gameMenu.Close();
        StartLevel();
    }

    public void StartLevel()
    {
        UpdateGoalIndicators();
        ContinueGame();
        carController.StartCar();
        startCountdown.BeginCountdown();
    }

    public void StartCountdownFinished()
    {
        isLevelRunning = true;
        timer.StartTimer();
        soundtrack.Play();
    }


    public void NextLevel()
    {
        sceneLoader.LoadNextLevel();
    }

    public bool getIsLevelRunning()
    {
        return isLevelRunning;
    }

    public void showMaxSpeedIndicator()
    {
        maxSpeedIndicator.SetActive(true);
    }

    public void hideMaxSpeedIndicator()
    {
        maxSpeedIndicator.SetActive(false);
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
    }

    private void ContinueGame()
    {
        Time.timeScale = 1;
    }

    private void CreateGoalIndicatorGameObjects()
    {
        string parentFolderName = "Goal Indicators";
        GameObject parentFolder = GameObject.Find(parentFolderName);

        int i = 0;

        foreach (var ringGoal in goals)
        {
            GameObject newGoalIndicatorGameObject = Instantiate(goalIndicatorPrefab);
            newGoalIndicatorGameObject.transform.SetParent(parentFolder.transform, false);
            GoalIndicator goalIndicator = newGoalIndicatorGameObject.GetComponent<GoalIndicator>();
            RectTransform rectTransform = newGoalIndicatorGameObject.GetComponent<RectTransform>();

            goalIndicators.Add(goalIndicator);

            int copyNumber = i + 1;
            newGoalIndicatorGameObject.name = goalIndicatorPrefab.name + " " + copyNumber;

            i++;
        }
    }

    private void UpdateGoalIndicators()
    {
        for (int i = 0; i < goalIndicators.Count; i++)
        {
            if (i < goalHitCount)
            {
                goalIndicators[i].GoalHit();
            }
            else
            {
                goalIndicators[i].Reset();
            }
        }
    }

    private void ToggleMenu()
    {
        if (isGameMenuShowing)
        {
            CloseMenu();
        }
        else
        {
            ShowMenu();
        }
    }

    private void ShowMenu()
    {
        gameMenu.Show();
        carController.TurnOffCar();
        PauseGame();
    }

    public void CloseMenu()
    {
        gameMenu.Close();
        carController.StartCar();
        ContinueGame();
    }

    private void UpdatePlayerDataAndShowWinScreen()
    {
        float currentTime = timer.GetElapsedTime();
        bool isNewBesttime = false;
        float? previousBesttime = PlayerDataManager.instance.GetBesttime();
        float besttime;

        if (previousBesttime == null || currentTime < previousBesttime)
        {
            besttime = currentTime;
            PlayerDataManager.instance.SetBesttime(currentTime);
            isNewBesttime = true;
        }
        else
        {
            besttime = (float)previousBesttime;
        }

        int rating = this.getAndSaveRating(currentTime);

        winScreen.Show(
            GetFormattedTime(timer.GetElapsedTime()),
            GetFormattedTime((float)besttime),
            isNewBesttime,
            rating
        );
    }

    private int getAndSaveRating(float currentTime)
    {
        int rating = 0;

        if (currentTime <= levelSettings.goldMedalTime)
        {
            rating = 3;
        }
        else if (currentTime <= levelSettings.silverMedalTime)
        {
            rating = 2;
        }
        else if (currentTime <= levelSettings.bronzeMedalTime)
        {
            rating = 1;
        }

        int? previousRating = PlayerDataManager.instance.GetRating();

        if (previousRating == null || rating > previousRating)
        {
            PlayerDataManager.instance.SetRating(rating);
        }

        return rating;
    }

    private string GetFormattedTime(float time)
    {
        int minutes = (int)(time / 60f);
        int seconds = (int)(time % 60f);
        int milliseconds = (int)((time * 1000f) % 1000f);

        return string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds / 10);
    }
}