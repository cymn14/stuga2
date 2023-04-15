using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private TextMeshProUGUI timerText;
    private float elapsedTime = 0f;
    private bool isTimerRunning = false;

    private void Awake()
    {
        timerText = gameObject.GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        UpdateTimerText();
    }

    private void Update()
    {
        if (isTimerRunning)
        {
            elapsedTime += Time.deltaTime;
            UpdateTimerText();
        }
    }

    private void UpdateTimerText()
    {
        timerText.text = getFormattedTime();
    }

    public string getFormattedTime()
    {
        int minutes = (int)(elapsedTime / 60f);
        int seconds = (int)(elapsedTime % 60f);
        int milliseconds = (int)((elapsedTime * 1000f) % 1000f);
        return string.Format("{0:00}:{1:00}:{2:0}", minutes, seconds, milliseconds / 100);
    }

    public void StartTimer()
    {
        isTimerRunning = true;
    }

    public void StopTimer()
    {
        isTimerRunning = false;
    }

    public void ResetTimer()
    {
        elapsedTime = 0;
        UpdateTimerText();
    }
}
