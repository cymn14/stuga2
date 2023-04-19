using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameController : MonoBehaviour
{

    [SerializeField]
    private AudioSource goalHitAudioSource;

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
    }

    public void GoalHit()
    {
        goalHitCount++;
        goalHitAudioSource.Play();

        UpdateGoalIndicators();

        if (goalHitCount == goals.Count)
        {
            LevelWon();
        }
    }

    private void LevelWon()
    {
        PauseGame();
        isLevelRunning = false;
        timer.StopTimer();
        winScreen.Show(timer.getFormattedTime());
        levelWonAudioSource.Play();
    }

    private void InitializeVariables()
    {
        playerInput = GetComponent<PlayerInput>();
        respawnAction = playerInput.actions["Respawn"];
        showMenuAction = playerInput.actions["Show Menu"];
        ballGameObjects = GameObject.FindGameObjectsWithTag("Ball");

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

        float top = 50f;
        float right = 50f;
        float itemSpacing = 30f; // The offset between each copy
        int i = 0;

        foreach (var ringGoal in goals)
        {
            GameObject newGoalIndicatorGameObject = Instantiate(goalIndicatorPrefab);
            newGoalIndicatorGameObject.transform.SetParent(parentFolder.transform, false);
            GoalIndicator goalIndicator = newGoalIndicatorGameObject.GetComponent<GoalIndicator>();
            RectTransform rectTransform = newGoalIndicatorGameObject.GetComponent<RectTransform>();

            Vector2 currentPosition = rectTransform.anchoredPosition;
            Vector2 newPosition = new Vector2(Screen.width / 2 - right - i * itemSpacing - rectTransform.sizeDelta.x / 2f, Screen.height / 2 - top - rectTransform.sizeDelta.y / 2f);
            rectTransform.anchoredPosition = newPosition;
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
        PauseGame();
    }

    public void CloseMenu()
    {
        gameMenu.Close();
        ContinueGame();
    }
}
