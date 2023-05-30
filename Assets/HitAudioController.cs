using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitAudioController : MonoBehaviour
{
    #region PROPERTIES
    [SerializeField] AudioClip hitAC;
    public AudioSource hitAS;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        hitAS = GetComponent<AudioSource>();
    }

    public void PlayAudio()
    {
        hitAS.clip = hitAC;
        hitAS.PlayOneShot(hitAS.clip);
    }

    public void StopAudio()
    {
        hitAS.Stop();
    }
}
