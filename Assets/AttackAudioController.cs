using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAudioController : MonoBehaviour
{
    #region PROPERTIES
    [SerializeField] AudioClip[] attackAC;
    AudioSource attackAS;
    GameObject owner;
    Transform audioObj;
    GameObject GameManager;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        attackAS = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public AudioClip GetRandomClip()
    {
        return attackAS.clip = attackAC[Random.Range(0, 2)];
    }

    public void PlayAudio()
    {
        attackAS.clip = GetRandomClip();
        attackAS.PlayOneShot(attackAS.clip);
    }
}
