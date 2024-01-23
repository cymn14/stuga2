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
        string playerPrefsKey = GetPlayerPrefsKey();

        if (!PlayerPrefs.HasKey(playerPrefsKey))
        {
            return null;
        }

        return PlayerPrefs.GetFloat(GetPlayerPrefsKey());
    }

    public void SetBesttime(float besttime)
    {
        PlayerPrefs.SetFloat(GetPlayerPrefsKey(), besttime);
        Save();
    }

    public string GetPlayerPrefsKey()
    {
        return "level" + currentLevel + "_besttime";
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
