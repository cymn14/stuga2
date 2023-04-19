using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI buttonText;

    [SerializeField]
    private string buttonTextPrefix = "Level";

    [SerializeField]
    private Button button;

    [SerializeField]
    private SceneLoader sceneLoader;

    private int levelIndex = 0;

    private void Awake()
    {
        button.interactable = false;
    }

    public void LockLevel()
    {
        button.interactable = false;
    }

    public void UnlockLevel()
    {
        button.interactable = true;
    }

    public void SelectLevel()
    {
        sceneLoader.LoadLevel(levelIndex);
    }

    public void SetLevelIndex(int index)
    {
        levelIndex = index;
        SetButtonName();
    }

    private void SetButtonName()
    {
        buttonText.text = buttonTextPrefix + " " + (levelIndex + 1);
    }
}
