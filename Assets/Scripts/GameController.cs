using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    [SerializeField]
    private AudioSource goalHitAudioSource;

    [SerializeField]
    private AudioSource levelWonAudioSource;

    [SerializeField]
    private GameObject goalIndicatorPrefab;

    private List<RingGoal> ringGoals;
    private List<GoalIndicator> goalIndicators;
    private StartCountdown startCountdown;
    private int goalHitCount = 0;
    private InputAction respawnAction;
    private PlayerInput playerInput;
    private WinScreen winScreen;
    private GameObject winScreenObj;
    private Timer timer;
    private GameObject[] ballGameObjects;
    private CarController carController;
    private LevelController levelController;
    private bool isLevelRunning = false;


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

        if (goalHitCount == ringGoals.Count)
        {
            LevelWon();
        }
    }

    private void LevelWon()
    {
        PauseGame();
        isLevelRunning = false;
        timer.StopTimer();
        winScreen.setWinTime(timer.getFormattedTime());
        winScreenObj.SetActive(true);
        levelWonAudioSource.Play();
    }

    private void InitializeVariables()
    {
        playerInput = GetComponent<PlayerInput>();
        respawnAction = playerInput.actions["Respawn"];

        timer = GameObject.Find("Timer").GetComponent<Timer>();
        startCountdown = GameObject.Find("Start Countdown").GetComponent<StartCountdown>();
        levelController = GameObject.Find("LevelController").GetComponent<LevelController>();

        winScreenObj = GameObject.Find("Win Screen");
        winScreen = winScreenObj.GetComponent<WinScreen>();

        ballGameObjects = GameObject.FindGameObjectsWithTag("Ball");

        GameObject carGameObject = GameObject.Find("Car");
        carController = carGameObject.GetComponent<CarController>();

        ringGoals = new List<RingGoal>();
        goalIndicators = new List<GoalIndicator>();

        foreach (var ringGoalObject in GameObject.FindGameObjectsWithTag("RingGoal"))
        {
            ringGoals.Add(ringGoalObject.GetComponent<RingGoal>());
        }

        createGoalIndicatorGameObjects();
    }

    private void HandleInputs()
    {
        respawnAction.performed += context => Respawn();
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

        foreach (var ringGoal in ringGoals)
        {
            ringGoal.Reset();
        }

        ResetAllBalls();
        ResetCar();

        StartLevel();
    }

    public void StartLevel()
    {
        UpdateGoalIndicators();
        ContinueGame();
        winScreenObj.SetActive(false);
        startCountdown.BeginCountdown();
    }

    public void StartCountdownFinished()
    {
        isLevelRunning = true;
        timer.StartTimer();
    }


    public void NextLevel()
    {
        levelController.LoadNextLevel();
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

    private void createGoalIndicatorGameObjects()
    {
        string parentFolderName = "Goal Indicators";
        GameObject parentFolder = GameObject.Find(parentFolderName);

        float offset = 30f; // The offset between each copy
        int i = 0;

        foreach (var ringGoal in ringGoals)
        {
            GameObject newGoalIndicatorGameObject = Instantiate(goalIndicatorPrefab);
            newGoalIndicatorGameObject.transform.SetParent(parentFolder.transform, false);
            GoalIndicator goalIndicator = newGoalIndicatorGameObject.GetComponent<GoalIndicator>();
            goalIndicators.Add(goalIndicator);

            // Set the position of the new object relative to the original object
            Vector3 originalPosition = goalIndicatorPrefab.transform.position;
            newGoalIndicatorGameObject.transform.position = new Vector3(originalPosition.x - (i + 1) * offset, originalPosition.y, originalPosition.z);

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
            } else
            {
                goalIndicators[i].Reset();
            }
        }
    }
}
