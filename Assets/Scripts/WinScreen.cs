using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class WinScreen : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI winTime;

    [SerializeField]
    private TextMeshProUGUI personalBesttime;

    [SerializeField]
    private GameObject personalHighscoreGameObject;

    [SerializeField]
    private GameObject newHighscoreGameObject;

    [SerializeField]
    private GameObject buttonToSelect;

    [SerializeField]
    private GameObject noneRating;

    [SerializeField]
    private GameObject bronzeRating;

    [SerializeField]
    private GameObject silverRating;

    [SerializeField]
    private GameObject goldRating;

    [SerializeField]
    private EventSystem eventSystem;

    private void Awake()
    {
        gameObject.SetActive(false);
        newHighscoreGameObject.SetActive(false);
    }

    public void Show(string time, string bestTime, bool newHighscore, int levelRating)
    {
        noneRating.SetActive(false);
        bronzeRating.SetActive(false);
        silverRating.SetActive(false);
        goldRating.SetActive(false);

        winTime.text = time;
        personalBesttime.text = bestTime;
        newHighscoreGameObject.SetActive(newHighscore);
        personalHighscoreGameObject.SetActive(!newHighscore);

        if (levelRating == 0)
        {
            noneRating.SetActive(true);
        }
        else if (levelRating == 1)
        {
            bronzeRating.SetActive(true);
        }
        else if (levelRating == 2)
        {
            silverRating.SetActive(true);
        }
        else if (levelRating == 3)
        {
            goldRating.SetActive(true);
        }

        gameObject.SetActive(true);
        eventSystem.SetSelectedGameObject(buttonToSelect);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
