using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    [SerializeField]
    private GameObject levelButtonPrefab;

    [SerializeField]
    private SceneLoader sceneLoader;

    [SerializeField]
    private EventSystem eventSystem;

    [SerializeField]
    private PlayerPrefsManager playerPrefsManager;

    private string levelsFolder = "Scenes/Levels";
    private List<string> levelSceneNames = new List<string>();
    private List<LevelButton> levelButtons;

    private void Awake()
    {
        levelButtons = new List<LevelButton>();
    }

    void Start()
    {
        GetLevelScenes();
        CreateLevelButtons();

        for (int i = 0; i < levelButtons.Count; i++)
        {
            if (i > playerPrefsManager.GetBiggestClearedLevel())
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
        int levelIndex = 0;

        foreach (var levelSceneName in levelSceneNames)
        {
            GameObject newLevelButtonPrefabGameObject = Instantiate(levelButtonPrefab);
            newLevelButtonPrefabGameObject.transform.SetParent(parentFolder.transform, false);
            LevelButton levelButton = newLevelButtonPrefabGameObject.GetComponent<LevelButton>();
            levelButton.SetLevelIndex(levelIndex);
            levelButtons.Add(levelButton);

            if (levelIndex == playerPrefsManager.GetBiggestClearedLevel())
            {
                eventSystem.firstSelectedGameObject = newLevelButtonPrefabGameObject;
            }

            levelIndex++;
        }
    }
}
