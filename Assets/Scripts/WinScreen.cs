using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class WinScreen : MonoBehaviour
{
    [SerializeField]
    private GameController gameController;

    [SerializeField]
    private TextMeshProUGUI winTime;

    private InputAction retryAction;
    private InputAction nextLevelAction;
    private PlayerInput playerInput;    

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
