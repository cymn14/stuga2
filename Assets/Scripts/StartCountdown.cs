using System.Collections;
using TMPro;
using UnityEngine;

public class StartCountdown : MonoBehaviour
{
    [SerializeField]
    private GameController gameController;

    [SerializeField]
    private int secondsToWait = 2;

    private TextMeshProUGUI countdownText;
    private int countdown;

    private void Awake()
    {
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
