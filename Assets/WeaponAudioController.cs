using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAudioController : MonoBehaviour
{
    #region PROPERTIES
    [SerializeField] AudioClip weaponAC;
    AudioSource weaponAS;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        weaponAS = GetComponent<AudioSource>();
    }

    public void PlayAudio()
    {
        if(!weaponAS.isPlaying)
        {
            weaponAS.clip = weaponAC;
            weaponAS.Play();
        }
    }

    public void StopAudio()
    {
        weaponAS.Stop();
    }

    public bool IsPlaying()
    {
        return weaponAS.isPlaying;
    }
}
