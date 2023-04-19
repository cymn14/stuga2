using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsManager : MonoBehaviour
{
    public void DeleteAll()
    {
        PlayerPrefs.DeleteAll();
    }

    private void Save()
    {
        PlayerPrefs.Save();
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
        return "level" + GetCurrentLevel() + "_besttime";
    }

    public int GetCurrentLevel()
    {
        return PlayerPrefs.GetInt("currentLevel", 0);
    }

    public void SetCurrentLevel(int level)
    {
        PlayerPrefs.SetInt("currentLevel", level);
        Save();
    }

    public int GetBiggestClearedLevel()
    {
        return PlayerPrefs.GetInt("biggestClearedLevel", 0);
    }

    public void SetBiggestClearedLevel(int biggestClearedLevel)
    {
        PlayerPrefs.SetInt("biggestClearedLevel", biggestClearedLevel);
        Save();
    }
}
