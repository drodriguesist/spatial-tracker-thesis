using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreathingAudioController : MonoBehaviour
{
    [SerializeField] AudioClip[] breathingAC;
    AudioSource breathingAS;

    // Start is called before the first frame update
    void Start()
    {
        breathingAS = GetComponent<AudioSource>();
    }

    public void UpdatePlayerBreathingClip(int pHealth)
    {
        if (pHealth > 0 && pHealth <= 25)
        {
            breathingAS.clip = breathingAC[0];
        }
        else if (pHealth > 25 && pHealth <= 50)
        {
            breathingAS.clip = breathingAC[1];
        }
        else if (pHealth > 50 && pHealth <= 75)
        {
            breathingAS.clip = breathingAC[2];
        }
        else if (pHealth > 75 && pHealth <= 100)
        {
            breathingAS.clip = null;//none
        }
    }

    public void PlayBreathingAudio()
    {
        if (!breathingAS.isPlaying) breathingAS.Play();
    }

    public void StopPlayingAudio()
    {
        breathingAS.Stop();
    }
}
