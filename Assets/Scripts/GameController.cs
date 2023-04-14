using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private List<RingGoal> ringGoals;

    private StartCountdown startCountdown;
    private int goalHitCount = 0;
    private InputAction respawnAction;
    private PlayerInput playerInput;
    private WinScreen winScreen;
    private GameObject winScreenObj;
    private Timer timer;
    
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
    }

    private void InitializeVariables()
    {
        playerInput = GetComponent<PlayerInput>();
        respawnAction = playerInput.actions["Respawn"];

        timer = GameObject.Find("Timer").GetComponent<Timer>();
        startCountdown = GameObject.Find("Start Countdown").GetComponent<StartCountdown>();

        winScreenObj = GameObject.Find("Win Screen");
        winScreen = winScreenObj.GetComponent<WinScreen>();

        ballGameObjects = GameObject.FindGameObjectsWithTag("Ball");

        GameObject carGameObject = GameObject.Find("Car");
        carController = carGameObject.GetComponent<CarController>();
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
        Debug.Log("next level");
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
}
