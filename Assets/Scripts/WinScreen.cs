using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class WinScreen : MonoBehaviour
{
    private InputAction retryAction;
    private InputAction nextLevelAction;
    private PlayerInput playerInput;
    private GameController gameController;
    private TextMeshProUGUI winTime;

    private void Awake()
    {
        InitializeVariables();
    }

    private void Start()
    {
        HandleInputs();
    }

    private void InitializeVariables()
    {
        playerInput = GetComponent<PlayerInput>();
        retryAction = playerInput.actions["Retry"];
        nextLevelAction = playerInput.actions["Next Level"];

        gameController = GameObject.Find("GameController").GetComponent<GameController>();

        winTime = GameObject.Find("Win Time").GetComponent<TextMeshProUGUI>();
    }

    private void HandleInputs()
    {
        retryAction.performed += context => gameController.Retry();
        nextLevelAction.performed += context => gameController.NextLevel();
    }

    public void setWinTime(string time)
    {
        winTime.text = time;
    }
}
