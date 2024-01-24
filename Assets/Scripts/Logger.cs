using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Logger : MonoBehaviour
{
    private string logFilePath;

    void Awake()
    {
        logFilePath = Path.Combine(Application.persistentDataPath, "gameLog.txt");
        Debug.Log("Log file path: " + logFilePath);
        Application.logMessageReceived += LogMessage;
    }

    void OnDestroy()
    {
        Application.logMessageReceived -= LogMessage;
    }

    private void LogMessage(string logString, string stackTrace, LogType type)
    {
        using (StreamWriter writer = new StreamWriter(logFilePath, true))
        {
            writer.WriteLine(logString);
            if (type == LogType.Exception || type == LogType.Error)
            {
                writer.WriteLine(stackTrace);
            }
        }
    }
}
