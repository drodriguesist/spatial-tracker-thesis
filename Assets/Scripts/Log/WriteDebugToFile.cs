using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class WriteDebugToFile : MonoBehaviour
{
    string filename = "";
    public static WriteDebugToFile instance;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    void OnEnable()
    {
        Application.logMessageReceived += Log;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= Log;
    }

    // Start is called before the first frame update
    void Start()
    {
        filename = Application.dataPath + "/LogFile.log";
    }

    public void Log(string logString, string stackTrace, LogType type)
    {
        if(!string.IsNullOrEmpty(filename))
        {
            TextWriter tw = new StreamWriter(filename, true);

            tw.WriteLine("[" + System.DateTime.Now + "] " + logString);

            tw.Close();
        }
    }
}
