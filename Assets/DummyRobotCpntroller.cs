using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyRobotCpntroller : MonoBehaviour
{
    HitAudioController hitAC;
    bool healthStateChanged;
    bool isDead;

    public Transform Lane;

    public float Health { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        Health = 30f;
        isDead = false;
        hitAC = GetComponentInChildren<HitAudioController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetLane(Transform lane)
    {
        Lane = lane;
    }

    #region HEALTH METHODS
    /// <summary>
    /// indicates if health counters has changed
    /// </summary>
    /// <returns></returns>
    public bool GetHealthStateChanged()
    {
        return healthStateChanged;
    }

    /// <summary>
    /// set health state changed
    /// </summary>
    /// <param name="value"></param>
    public void SetHealthStateChanged(bool value)
    {
        healthStateChanged = value;
    }
    #endregion

    #region DAMAGE METHODS
    public void TakeDamage(float amount)
    {
        //SetHealthStateChanged(true);
        Health -= amount;
        SetHealthStateChanged(true);
        PlayAudios();
        if(Health == 0f)
        {
            Die();
        }
    }

    void PlayAudios()
    {
        hitAC.PlayAudio();
    }

    public bool IsDead()
    {
        return isDead;
    }

    void Die()
    {
        GameObject.FindGameObjectWithTag("ObjectSpawner").GetComponent<ObjectSpawnerController>().canSpawn = true;
        GameObject.FindGameObjectWithTag("ObjectSpawner").GetComponent<ObjectSpawnerController>().AddDummyRobotKills();
        if(InputManagerController.instance.isTrackerEnabled) GetComponent<RumbleController>().StopRumble();
        isDead = true;
        var boxCollider = gameObject.GetComponent<BoxCollider>();
        boxCollider.enabled = false;
    }

    public void StartRumble(Transform lane)
    {
        if(gameObject.GetComponent<RumbleController>().GamepadPresent())
        {
            switch(lane.name)
            {
                case "Lane1":
                    gameObject.GetComponent<RumbleController>().RumblePulse(1f, 0f);
                    break;
                case "Lane2":
                    gameObject.GetComponent<RumbleController>().RumbleConstant();
                    break;
                case "Lane3":
                    gameObject.GetComponent<RumbleController>().RumblePulse(0f, 1f);
                    break;
            }
        }
    }
    #endregion
}
