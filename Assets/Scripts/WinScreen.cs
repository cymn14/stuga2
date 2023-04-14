using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WinScreen : MonoBehaviour
{
    private InputAction retryAction;
    private PlayerInput playerInput;
    private GameObject GameControllerObj;
    private GameController GameController;

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

        GameControllerObj = GameObject.Find("GameController");
        GameController = GameControllerObj.GetComponent<GameController>();
    }

    private void HandleInputs()
    {
        retryAction.performed += context => GameController.Retry();
    }
}
