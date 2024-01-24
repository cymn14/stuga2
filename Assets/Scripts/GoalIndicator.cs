using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // This namespace is used for both RawImage and Image

public class GoalIndicator : MonoBehaviour
{
    [SerializeField]
    private Color hitColor = Color.green;

    [SerializeField]
    private Color notHitColor = Color.white;

    private Image goalImage;

    private void Awake()
    {
        goalImage = gameObject.GetComponent<Image>();
        goalImage.color = notHitColor;
    }

    public void GoalHit()
    {
        goalImage.color = hitColor;
    }

    public void Reset()
    {
        goalImage.color = notHitColor;
    }
}