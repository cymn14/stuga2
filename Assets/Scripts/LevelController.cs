using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    private int currentLevel;
    private string levelsFolder = "Scenes/Levels";
    private List<string> sceneNames = new List<string>();

    private void Start()
    {
        string[] scenePaths = System.IO.Directory.GetFiles("Assets/" + levelsFolder, "*.unity");

        foreach (string path in scenePaths)
        {
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(path);
            sceneNames.Add(sceneName);
        }
    }

    public void LoadScene(int index)
    {
        if (index >= 0 && index < sceneNames.Count)
        {
            currentLevel = index;
            SceneManager.LoadScene(sceneNames[index]);
        }
    }

    public void LoadNextLevel()
    {
        LoadScene(currentLevel + 1);
    }
}
