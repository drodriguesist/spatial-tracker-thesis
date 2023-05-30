using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartbeatAudioController : MonoBehaviour
{
    [SerializeField] AudioClip[] heartbeatAC;
    AudioSource heartbeatAS;

    // Start is called before the first frame update
    void Start()
    {
        heartbeatAS = GetComponent<AudioSource>();
    }

    public void UpdatePlayerHeartbeatClip(int pHealth)
    {
        if (pHealth > 0 && pHealth <= 25)
        {
            heartbeatAS.clip = heartbeatAC[0];
        }
        else if (pHealth > 25 && pHealth <= 50)
        {
            heartbeatAS.clip = heartbeatAC[1];
        }
        else if (pHealth > 50 && pHealth <= 75)
        {
            heartbeatAS.clip = heartbeatAC[2];
        }
        else if (pHealth > 75 && pHealth <= 100)
        {
            heartbeatAS.clip = null;//none
        }
    }

    public void PlayHeartbeatAudio()
    {
        if (!heartbeatAS.isPlaying) heartbeatAS.Play();
    }

    public void StopPlayingAudio()
    {
        heartbeatAS.Stop();
    }
}
