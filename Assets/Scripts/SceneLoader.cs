using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField]
    private string mainMenuSceneName = "Main Menu";

    [SerializeField]
    private string levelSelectorSceneName = "Level Selector";

    [SerializeField]
    private string levelsFolder = "Scenes/Levels";

    [SerializeField]
    private PlayerPrefsManager playerPrefsManager;

    private List<string> levelSceneNames = new List<string>();

    private void Start()
    {
        string[] scenePaths = System.IO.Directory.GetFiles("Assets/" + levelsFolder, "*.unity");

        foreach (string path in scenePaths)
        {
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(path);
            levelSceneNames.Add(sceneName);
        }
    }

    public void LoadLevel(int levelIndex)
    {
        if (levelIndex >= 0 && levelIndex < levelSceneNames.Count)
        {
            playerPrefsManager.SetCurrentLevel(levelIndex + 1);
            SceneManager.LoadScene(levelSceneNames[levelIndex]);
        }
    }

    public void LoadNextLevel()
    {
        int currentLevelIndex = playerPrefsManager.GetCurrentLevel() - 1;
        LoadLevel(currentLevelIndex + 1);
    }

    public void LoadLevelSelector()
    {
        SceneManager.LoadScene(levelSelectorSceneName);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
