using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.IO;

public class SceneLoader : MonoBehaviour
{
    [SerializeField]
    private string mainMenuSceneName = "Main Menu";

    [SerializeField]
    private string levelSelectorSceneName = "Level Selector";

    [SerializeField]
    private string levelsFolder = "Scenes/Levels";

    private List<string> levelSceneNames = new List<string>();

    private void Start()
    {
        InitializeLevelSceneNames();
    }

    private void InitializeLevelSceneNames()
    {
        levelSceneNames.Clear();

        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            if (path.Contains(levelsFolder))
            {
                string sceneName = Path.GetFileNameWithoutExtension(path);
                levelSceneNames.Add(sceneName);
            }
        }
    }

    public void LoadLevel(int levelIndex)
    {
        if (levelIndex >= 0 && levelIndex < levelSceneNames.Count)
        {
            PlayerDataManager.instance.SetCurrentLevel(levelIndex + 1);
            SceneManager.LoadScene(levelSceneNames[levelIndex]);
        }
    }

    public void LoadNextLevel()
    {
        int currentLevelIndex = PlayerDataManager.instance.GetCurrentLevel() - 1;
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