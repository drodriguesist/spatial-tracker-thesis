using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PainAudioController : MonoBehaviour
{
    #region PROPERTIES
    [SerializeField] AudioClip painAC;
    AudioSource painAS;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        painAS = GetComponent<AudioSource>();
    }

    public void PlayAudio()
    {
        painAS.clip = painAC;
        painAS.Play();
    }

    public void StopAudio()
    {
        painAS.Stop();
    }

    public bool IsPlaying()
    {
        return painAS.isPlaying;
    }
}
