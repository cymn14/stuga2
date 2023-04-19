using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField]
    private GoalTriggerEnum goalTrigger = GoalTriggerEnum.Ball;

    [SerializeField]
    private Color ballTriggerColor = Color.cyan;

    [SerializeField]
    private Color carTriggerColor = Color.magenta;

    public enum GoalTriggerEnum
    {
        Ball,
        Car,
    }

    private GameController gameController;
    private bool hasBallPassed = false;
    private Color hitColor = Color.green;
    private Renderer ringRenderer;
    private AudioSource goalSound;

    private void Awake()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        goalSound = GetComponent<AudioSource>();
        ringRenderer = GetComponent<Renderer>();
        SetColor();
    }

    public void CarTrigger()
    {
        if (goalTrigger == GoalTriggerEnum.Car)
        {
            Trigger();
        }
    }

    public void BallTrigger(Collider other)
    {
        if (goalTrigger == GoalTriggerEnum.Ball)
        {
            other.gameObject.transform.parent.gameObject.SetActive(false);
            Trigger();
        }
    }

    private void Trigger()
    {
        if (!hasBallPassed)
        {
            goalSound.Play();
            hasBallPassed = true;
            ringRenderer.material.color = hitColor;
            gameController.GoalHit();
        }
    }

    public void Reset()
    {
        gameObject.SetActive(true);
        hasBallPassed = false;
        SetColor();
    }

    private void SetColor()
    {
        if (goalTrigger == GoalTriggerEnum.Ball)
        {
            ringRenderer.material.color = ballTriggerColor;
        }
        else if (goalTrigger == GoalTriggerEnum.Car)
        {
            ringRenderer.material.color = carTriggerColor;
        }
    }
}
