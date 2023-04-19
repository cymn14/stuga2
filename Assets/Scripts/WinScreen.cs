using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class WinScreen : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI winTime;

    [SerializeField]
    private GameObject buttonToSelect;

    [SerializeField]
    private EventSystem eventSystem;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void Show(string time)
    {
        winTime.text = time;
        gameObject.SetActive(true);
        eventSystem.SetSelectedGameObject(buttonToSelect);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
