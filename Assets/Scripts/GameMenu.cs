using UnityEngine;
using UnityEngine.EventSystems;
public class GameMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject buttonToSelect;

    [SerializeField]
    private EventSystem eventSystem;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
        eventSystem.SetSelectedGameObject(buttonToSelect);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
