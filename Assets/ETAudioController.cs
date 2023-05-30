using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ETAudioController : MonoBehaviour
{
    #region Properties
    AudioSource enemyTrackerAS;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        enemyTrackerAS = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Play the clip provided by the Enemy Tracker
    /// </summary>
    /// <param name="clip"></param>
    public void PlayEnemyTrackerAudio(AudioClip clip)
    {
        enemyTrackerAS.clip = clip;
        if(!enemyTrackerAS.isPlaying)
            enemyTrackerAS.Play();
    }

    public AudioSource GetAudioSource()
    {
        return enemyTrackerAS;
    }

    public void PlayOneShotEnemyTrackerAudio(AudioClip clip)
    {
        enemyTrackerAS.clip = clip;
        enemyTrackerAS.loop = false;
        enemyTrackerAS.Play();
    }

    public bool IsPlaying()
    {
        return enemyTrackerAS.isPlaying;
    }

}
