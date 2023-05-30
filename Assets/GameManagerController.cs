using Player;
using Rail;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class GameManagerController : MonoBehaviour
{
    #region Properties
    public GameObject[] Lanes;
    public GameObject player;
    public GameObject objectPicker;
    GameObject rail;
    bool segmentsGenerated;
    Dictionary<int, (int, int)> railSegments;
    int enemiesCounter;
    public static GameManagerController instance;
    #endregion

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

    // Start is called before the first frame update
    void Start()
    {
        enemiesCounter = 0;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void InitGameManager()
    {
        objectPicker = GameObject.FindGameObjectWithTag("ObjectPicker");
        Lanes = new GameObject[3];
        for(int i = 1; i <= 3; i++)
        {
            Lanes[i-1] = GameObject.FindGameObjectWithTag("Lane"+i.ToString());
        }
    }

    public void SetEnemiesCounter(int value)
    {
        enemiesCounter = value;
    }

    public int GetEnemiesCounter()
    {
        return enemiesCounter;
    }

    int HealthkitCounter()
    {
        var lstHealthkits = GameObject.FindGameObjectsWithTag("HealthKit");
        return lstHealthkits.Length;
    }

    void GenerateSegments(int limit)
    {
        for (int i = 0; i < limit; i++)
        {
            railSegments.Add(i, (i, i + 1));
        }
        segmentsGenerated = true;
    }
}
