using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager instance;

    private int currentLevel = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    public int GetCurrentLevel()
    {
        return currentLevel;
    }

    public void SetCurrentLevel(int level)
    {
        currentLevel = level;
    }

    public float? GetBesttime()
    {
        string keyBesttime = GetKeyForBesttime();

        if (!PlayerPrefs.HasKey(keyBesttime))
        {
            return null;
        }

        return PlayerPrefs.GetFloat(GetKeyForBesttime());
    }

    public void SetBesttime(float besttime)
    {
        PlayerPrefs.SetFloat(GetKeyForBesttime(), besttime);
        Save();
    }

    public int? GetRating()
    {
        string keyRating = GetKeyForRating();

        if (!PlayerPrefs.HasKey(keyRating))
        {
            return null;
        }

        return PlayerPrefs.GetInt(GetKeyForRating());
    }

    public void SetRating(int rating)
    {
        PlayerPrefs.SetInt(GetKeyForRating(), rating);
        Save();
    }

    public string GetKeyForBesttime()
    {
        return "level" + currentLevel + "_besttime";
    }

    public string GetKeyForRating()
    {
        return "level" + currentLevel + "_rating";
    }

    public int GetRatingForLevel(int level)
    {
        string keyRating = "level" + level + "_rating";

        if (!PlayerPrefs.HasKey(keyRating))
        {
            return 0;
        }

        return PlayerPrefs.GetInt(keyRating);
    }

    public int GetBiggestClearedLevel()
    {
        return PlayerPrefs.GetInt("biggestClearedLevel", 0);
    }

    public void UpdateBiggestClearedLevel()
    {
        if (currentLevel > GetBiggestClearedLevel())
        {
            SetBiggestClearedLevel(currentLevel);
        }
    }

    public void DeleteAll()
    {
        PlayerPrefs.DeleteAll();
        Save();
    }

    private void Save()
    {
        PlayerPrefs.Save();
    }

    private void SetBiggestClearedLevel(int biggestClearedLevel)
    {
        PlayerPrefs.SetInt("biggestClearedLevel", biggestClearedLevel);
        Save();
    }
}
