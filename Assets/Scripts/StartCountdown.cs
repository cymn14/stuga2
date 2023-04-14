using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StartCountdown : MonoBehaviour
{
    private TextMeshProUGUI countdownText;
    private GameController gameController;
    private int secondsToWait = 2;
    private int countdown;

    private void Awake()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        countdownText = gameObject.GetComponent<TextMeshProUGUI>();
    }

    public void BeginCountdown()
    {
        gameObject.SetActive(true);
        countdown = secondsToWait;
        StartCoroutine(Countdown());
    }

    IEnumerator Countdown()
    {
        while (countdown > 0)
        {
            countdownText.text = countdown.ToString("0");
            yield return new WaitForSeconds(1f);
            countdown--;
        }

        gameObject.SetActive(false);
        gameController.StartCountdownFinished();
    }
}
