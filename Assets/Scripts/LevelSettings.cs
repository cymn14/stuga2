using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSettings : MonoBehaviour
{
    public static LevelSettings instance;

    [SerializeField]
    public float goldMedalTime = 10f;

    [SerializeField]
    public float silverMedalTime = 20f;

    [SerializeField]
    public float bronzeMedalTime = 30f;

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
}
