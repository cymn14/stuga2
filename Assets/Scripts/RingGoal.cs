using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RingGoal : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI ringGoalText;

    [SerializeField]
    private GameController gameController;

    private bool hasBallPassed = false;
    private Color hitColor = Color.green;
    private Color notHitColor = Color.black;
    private AudioSource audioSource;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("BallTrigger"))
        {
            if (!hasBallPassed)
            {
                audioSource.Play();
                hasBallPassed = true;
                ringGoalText.color = hitColor;
                gameController.GoalHit();
            }
        }
    }

    public void Reset()
    {
        hasBallPassed = false;
        ringGoalText.color = notHitColor;
    }
}
