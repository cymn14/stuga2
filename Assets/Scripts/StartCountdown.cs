using System.Collections;
using TMPro;
using UnityEngine;

public class StartCountdown : MonoBehaviour
{
    [SerializeField]
    private GameController gameController;

    [SerializeField]
    private int secondsToWait = 3;

    [SerializeField]
    private AudioSource countdownSingleSound;

    [SerializeField]
    private AudioSource countdownLastSound;

    private TextMeshProUGUI countdownText;
    private int countdown;

    private void Awake()
    {
        countdownText = gameObject.GetComponent<TextMeshProUGUI>();
    }

    public void BeginCountdown()
    {
        //gameObject.SetActive(true);
        countdown = secondsToWait;
        StartCoroutine(Countdown());
    }

    IEnumerator Countdown()
    {
        countdownText.enabled = true;

        while (countdown >= 0)
        {
            if (countdown > 0)
            {
                countdownText.text = countdown.ToString("0");
                countdownSingleSound.Play();
            } else
            {
                countdownText.text = "GO!";
                countdownLastSound.Play();
                gameController.StartCountdownFinished();
            }

            yield return new WaitForSeconds(1f);
            countdown--;
        }

        countdownText.enabled = false;
        //gameObject.SetActive(false);
    }
}
