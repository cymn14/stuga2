using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GoalIndicator : MonoBehaviour
{
    private Color hitColor = Color.green;
    private Color notHitColor = Color.white;
    private TextMeshProUGUI ringGoalText;

    private void Awake()
    {
        ringGoalText = gameObject.GetComponent<TextMeshProUGUI>();
        ringGoalText.color = notHitColor;
    }

    public void GoalHit()
    {
        ringGoalText.color = hitColor;
    }

    public void Reset()
    {
        ringGoalText.color = notHitColor;
    }
}
