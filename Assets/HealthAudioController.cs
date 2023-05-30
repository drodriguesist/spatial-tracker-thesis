using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UtilsController;

public class HealthAudioController : MonoBehaviour
{
    [SerializeField] AudioClip[] healthAC;
    public AudioSource healthAS;

    // Start is called before the first frame update
    void Start()
    {
        healthAS = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdatePlayerHealthClip(HealthAudioState state)
    {
        if (state == HealthAudioState.Damage)
        {
            healthAS.clip = healthAC[0];
        }
        else if(state == HealthAudioState.Recover)
        {
            healthAS.clip = healthAC[1];
        }
        else
        {
            healthAS.clip = null;//none
        }
    }

    public void PlayPickUpAudio(HealthAudioState state)
    {
        if(state == HealthAudioState.Recover)
        {
            healthAS.volume = 0.35f;
            healthAS.PlayOneShot(healthAC[2]);
            Invoke("PlayHealthAudio", healthAC[2].length);
        }
        else if (state == HealthAudioState.Damage)
        {
            PlayPainAudio();
        }
    }

    public void PlayHealthAudio()
    {
        healthAS.volume = 1f;
        healthAS.PlayOneShot(healthAS.clip);
    }

    public void PlayPainAudio()
    {
        healthAS.volume = 1f;
        healthAS.PlayOneShot(healthAC[0]);
    }
}
