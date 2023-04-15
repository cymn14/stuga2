using UnityEngine;

public class Goal : MonoBehaviour
{
    private GameController gameController;
    private bool hasBallPassed = false;
    private Color hitColor = Color.green;
    private Color notHitColor = Color.cyan;
    private Renderer ringRenderer;

    private void Awake()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        ringRenderer = GetComponent<Renderer>();
        ringRenderer.material.color = notHitColor;
    }

    public void Trigger()
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
        hasBallPassed = false;
        ringRenderer.material.color = notHitColor;
    }
}
