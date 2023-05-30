using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public class InputManagerController : MonoBehaviour
{
    #region properties

    #region pubblic
    public static InputManagerController instance;
    public bool isTrackerEnabled = false;
    #endregion

    #region private
    #endregion
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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Start the game
    /// </summary>
    public void OnStart()
    {
        LevelManager.instance.StartPressed();
    }
    
    /// <summary>
    /// Pick Scene objects
    /// </summary>
    public void OnPickObject()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player is null) return;
        else player.GetComponent<PlayerController>().PickObjectPressed();
    }

    
    public void OnShootCenter()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player is null) return;
        else player.GetComponent<PlayerController>().ShootCenterPressed();
    }

    public void OnShootLeft()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player is null) return;
        else player.GetComponent<PlayerController>().ShootLeftPressed();
    }

    public void OnShootRight()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player is null) return;
        else player.GetComponent<PlayerController>().ShootRightPressed();
    }

    public void OnTraining()
    {
        LevelManager.instance.TrainingPressed();
    }

    public void OnEnableTracker()
    {
        isTrackerEnabled = !isTrackerEnabled;
        GetComponent<AudioSource>().Play();
    }

    public void OnExit() {
        Application.Quit();
    }
}