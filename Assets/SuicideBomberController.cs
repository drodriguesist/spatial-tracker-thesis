using SuicideBomber.Audio;
using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SuicideBomberController : MonoBehaviour
{
    #region PROPERTIES
    AudioSource audioSource;
    HitAudioController hitAC;
    PainAudioController painAC;
    GameObject GameManager;
    GameObject Player;
    bool healthStateChanged;
    bool isAttacking;
    bool isDead;
    float currentDistance;
    float distancePercentage;
    float startingDistance;
    #region PUBLIC
    public Transform Lane;
    public float Health { get; set; }
    #endregion

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        isAttacking = false;
        isDead = false;
        Health = 30f;
        audioSource = gameObject.GetComponent<AudioSource>();
        GameManager = GameObject.FindGameObjectWithTag("GameController");
        Player = GameObject.FindGameObjectWithTag("Player");
        currentDistance = 0f;
        startingDistance = Vector3.Distance(transform.position, Player.transform.position);
        hitAC = GetComponentInChildren<HitAudioController>();
        painAC = GetComponentInChildren<PainAudioController>();
    }

    void Update()
    {
        currentDistance = Vector3.Distance(transform.position, Player.transform.position);
        CheckDistanceToPlayer();
    }

    public void SetLane(Transform lane)
    {
        Lane = lane;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name.StartsWith("Lane"))
        {
            AudioManagerController.instance.RestoreToDefaultBGM();
            GameManager.GetComponent<GameManagerController>().SetEnemiesCounter(0);
            Destroy(gameObject);
        }
    }

    void CheckDistanceToPlayer()
    {
        distancePercentage = (currentDistance * 100) / startingDistance;
        if(distancePercentage <= 10)
        {
            if(!isAttacking) DoDamage(40);
        }
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
    public bool IsAttacking()
    {
        return isAttacking;
    }

    public void TakeDamage(float amount)
    {
        //SetHealthStateChanged(true);
        Health -= amount;
        SetHealthStateChanged(true);
        PlayAudios();
        if(Health <= 0f)
        {
            Die();
        }
    }

    public void DoDamage(int amount)
    {
        isAttacking = true;
        Player.GetComponent<PlayerController>().TakeDamage(amount);
        SetHealthStateChanged(true); //set true to trigger death sounds
        Die();
    }
    #endregion

    # region DEATH METHODS
    public bool IsDead()
    {
        return isDead;
    }

    void Die()
    {
        AudioManagerController.instance.RestoreToDefaultBGM();
        isDead = true;
        var boxCollider = gameObject.GetComponent<BoxCollider>();
        boxCollider.enabled = false;
    }

    void PlayAudios()
    {
        hitAC.PlayAudio();
        if(!isDead)
        {
            painAC.PlayAudio();
        }
    }
    #endregion
}
