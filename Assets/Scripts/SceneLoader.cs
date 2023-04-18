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

    private int currentLevel;
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

    public void LoadLevel(int index)
    {
        if (index >= 0 && index < levelSceneNames.Count)
        {
            currentLevel = index;
            SceneManager.LoadScene(levelSceneNames[index]);
        }
    }

    public void LoadNextLevel()
    {
        LoadLevel(currentLevel + 1);
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
