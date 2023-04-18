using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    [SerializeField]
    private GameObject levelButtonPrefab;

    [SerializeField]
    private SceneLoader sceneLoader;

    private string levelsFolder = "Scenes/Levels";
    private List<string> levelSceneNames = new List<string>();

    public List<LevelButton> levelButtons;


    // Start is called before the first frame update
    void Start()
    {
        GetLevelScenes();
        CreateLevelButtons();

        int levelAt = PlayerPrefs.GetInt("levelAt", 1); /* < Change this int value to whatever your
                                                             level selection build index is on your
                                                             build settings */
        for (int i = 0; i < levelButtons.Count; i++)
        {
            if (i + 1 > levelAt)
            {
                levelButtons[i].LockLevel();
            } else
            {
                levelButtons[i].UnlockLevel();
            }
        }
    }

    private void GetLevelScenes()
    {
        string[] scenePaths = System.IO.Directory.GetFiles("Assets/" + levelsFolder, "*.unity");

        foreach (string path in scenePaths)
        {
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(path);
            levelSceneNames.Add(sceneName);
        }
    }

    private void CreateLevelButtons()
    {
        string parentFolderName = "Level Grid";
        GameObject parentFolder = GameObject.Find(parentFolderName);

        int levelIndex = 1;

        foreach (var levelSceneName in levelSceneNames)
        {
            GameObject newLevelButtonPrefabGameObject = Instantiate(levelButtonPrefab);
            newLevelButtonPrefabGameObject.transform.SetParent(parentFolder.transform, false);
            LevelButton levelButton = newLevelButtonPrefabGameObject.GetComponent<LevelButton>();
            levelButton.SetLevelIndex(levelIndex);
            levelButtons.Add(levelButton);
            levelIndex++;
        }
    }


}