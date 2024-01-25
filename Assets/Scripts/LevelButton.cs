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
    private Button button;

    [SerializeField]
    private SceneLoader sceneLoader;

    [SerializeField]
    private GameObject noneRating;

    [SerializeField]
    private GameObject bronzeRating;

    [SerializeField]
    private GameObject silverRating;

    [SerializeField]
    private GameObject goldRating;

    private int levelIndex = 0;
    private int rating = 0;

    private void Awake()
    {
        button.interactable = false;
        noneRating.SetActive(false);
        bronzeRating.SetActive(false);
        silverRating.SetActive(false);
        goldRating.SetActive(false);
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

    public void SetRating(int ratingToSet)
    {
        rating = ratingToSet;
    }

    public void SetLevelIndex(int index)
    {
        levelIndex = index;
    }

    public void UpdateTextAndRating()
    {
        buttonText.text = (levelIndex + 1).ToString();

        if (rating == 0)
        {
            noneRating.SetActive(true);
        }
        else if (rating == 1)
        {
            bronzeRating.SetActive(true);
        }
        else if (rating == 2)
        {
            silverRating.SetActive(true);
        }
        else if (rating == 3)
        {
            goldRating.SetActive(true);
        }
    }
}
