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

    [SerializeField]
    private CameraFollow cameraFollow;

    [SerializeField]
    private Timer timer;

    [SerializeField]
    private StartCountdown startCountdown;

    [SerializeField]
    private WinScreen winScreen;

    [SerializeField]
    private GameObject winScreenObj;

    [SerializeField]
    private LevelController levelController;

    private List<Goal> goals;
    private List<GoalIndicator> goalIndicators;
    private int goalHitCount = 0;
    private InputAction respawnAction;
    private PlayerInput playerInput;
    private GameObject[] ballGameObjects;
    private CarController carController;
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
        winScreen.setWinTime(timer.getFormattedTime());
        winScreenObj.SetActive(true);
        levelWonAudioSource.Play();
    }

    private void InitializeVariables()
    {
        playerInput = GetComponent<PlayerInput>();
        respawnAction = playerInput.actions["Respawn"];
        ballGameObjects = GameObject.FindGameObjectsWithTag("Ball");

        GameObject carGameObject = GameObject.Find("Car");
        carController = carGameObject.GetComponent<CarController>();

        goals = new List<Goal>();
        goalIndicators = new List<GoalIndicator>();

        foreach (var ringGoalObject in GameObject.FindGameObjectsWithTag("RingGoal"))
        {
            goals.Add(ringGoalObject.GetComponent<Goal>());
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

        foreach (var ringGoal in goals)
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
