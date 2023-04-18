using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField]
    private GoalTriggerEnum goalTrigger = GoalTriggerEnum.Ball;

    public enum GoalTriggerEnum
    {
        Ball,
        Car,
    }

    private GameController gameController;
    private bool hasBallPassed = false;
    private Color hitColor = Color.green;
    private Color ballTriggerColor = Color.cyan;
    private Color carTriggerColor = Color.magenta;
    private Renderer ringRenderer;

    private void Awake()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
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
