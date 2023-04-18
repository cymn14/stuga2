using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalTrigger : MonoBehaviour
{
    private Goal goal;

    private void Awake()
    {
        goal = transform.parent.gameObject.GetComponent<Goal>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("BallTrigger"))
        {
            goal.BallTrigger(other);
        }

        if (other.gameObject.CompareTag("CarTrigger"))
        {
            goal.CarTrigger();
        }
    }
}
