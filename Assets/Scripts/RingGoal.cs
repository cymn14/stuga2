using UnityEngine;

public class RingGoal : MonoBehaviour
{
    private GameController gameController;
    private bool hasBallPassed = false;

    private void Awake()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("BallTrigger"))
        {
            if (!hasBallPassed)
            {
                hasBallPassed = true;
                Renderer renderer = GetComponent<Renderer>();
                renderer.material.color = Color.green;
                gameController.GoalHit();
            }
        }
    }

    public void Reset()
    {
        hasBallPassed = false;
    }
}
