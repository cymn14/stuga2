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
    private GameObject newHighscoreGameObject;

    [SerializeField]
    private GameObject buttonToSelect;

    [SerializeField]
    private EventSystem eventSystem;

    private void Awake()
    {
        gameObject.SetActive(false);
        newHighscoreGameObject.SetActive(false);
    }

    public void Show(string time, string bestTime, bool newHighscore)
    {
        winTime.text = time;
        personalBesttime.text = bestTime;

        if(newHighscore)
        {
            newHighscoreGameObject.SetActive(true);
        }

        gameObject.SetActive(true);
        eventSystem.SetSelectedGameObject(buttonToSelect);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
