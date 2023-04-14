using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private List<RingGoal> ringGoals;

    [SerializeField]
    private TextMeshProUGUI timerText; // Reference to the TextMeshPro object in the scene

    private int goalHitCount = 0;
    private InputAction respawnAction;
    private PlayerInput playerInput;
    private Canvas canvas;
    private TextMeshProUGUI winText;
    private TextMeshProUGUI winTime;
    private GameObject winScreenObj;
    private float elapsedTime = 0f; // Elapsed time since the game started
    private bool isTimerRunning = false; // Flag to check if the stopwatch is running
    private GameObject[] ballGameObjects;

    private void Awake()
    {
        InitializeVariables();
    }

    private void Start()
    {
        HandleInputs();
        StartLevel();

    }

    private void Update()
    {
        if (isTimerRunning)
        {
            elapsedTime += Time.deltaTime; // Add the time since the last frame to the elapsed time
            UpdateTimerText(); // Update the timer text display
        }
    }

    private void UpdateTimerText()
    {
        int minutes = (int)(elapsedTime / 60f);
        int seconds = (int)(elapsedTime % 60f);
        int milliseconds = (int)((elapsedTime * 1000f) % 1000f);
        timerText.text = string.Format("{0:00}:{1:00}:{2:0}", minutes, seconds, milliseconds / 100);
    }

    public void GoalHit()
    {
        goalHitCount++;

        if (goalHitCount == ringGoals.Count)
        {
            levelWon();
        }
    }

    private void levelWon()
    {

        winText.enabled = true;
        isTimerRunning = false;
        winScreenObj.SetActive(true);
    }

    private void InitializeVariables()
    {
        playerInput = GetComponent<PlayerInput>();
        respawnAction = playerInput.actions["Respawn"];

        canvas = GetComponentInChildren<Canvas>();

        GameObject timerObj = GameObject.Find("Timer");
        timerText = timerObj.GetComponent<TextMeshProUGUI>();
        GameObject winTextObj = GameObject.Find("Win Text");
        winText = winTextObj.GetComponent<TextMeshProUGUI>();
        GameObject winTimeObj = GameObject.Find("Win Time");
        winTime = winTimeObj.GetComponent<TextMeshProUGUI>();
        winScreenObj = GameObject.Find("Win Screen");

        ballGameObjects = GameObject.FindGameObjectsWithTag("Ball");
    }

    private void HandleInputs()
    {
        respawnAction.performed += context => Retry();
    }

    private void ResetBalls()
    {
        foreach (var ballGameObject in ballGameObjects)
        {
            Ball ball = ballGameObject.GetComponent<Ball>();
            ball.Reset();
        }
    }

    public void Retry()
    {
        goalHitCount = 0;
        elapsedTime = 0;

        foreach (var ringGoal in ringGoals)
        {
            ringGoal.Reset();
        }

        ResetBalls();

        StartLevel();
    }

    public void StartLevel()
    {
        winScreenObj.SetActive(false);
        isTimerRunning = true;
    }

    public void NextLevel()
    {
        Debug.Log("next level");
    }
}
