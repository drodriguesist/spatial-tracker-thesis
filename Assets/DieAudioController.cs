using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieAudioController : MonoBehaviour
{
    #region PROPERTIES
    [SerializeField] AudioClip dieAC;
    AudioSource dieAS;
    GameObject owner;
    Transform audioObj;
    GameObject GameManager;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        dieAS = GetComponent<AudioSource>();
        GameManager = GameObject.FindGameObjectWithTag("GameController");
        audioObj = gameObject.transform.parent;
        owner = audioObj.transform.parent.gameObject;
    }

    public void PlayAudio()
    {
        dieAS.clip = dieAC;
        dieAS.PlayOneShot(dieAS.clip);
        //StartCoroutine(PlayDeathAudio());
        Destroy(owner, dieAS.clip.length);
        //audioObj.gameObject.SetActive(false);
        GameManager.GetComponent<GameManagerController>().SetEnemiesCounter(0);
    }

    IEnumerator PlayDeathAudio()
    {
        yield return new WaitUntil(()=>!dieAS.isPlaying);
    }

    public void StopAudio()
    {
        dieAS.Stop();
    }

    public AudioSource GetAudioSource()
    {
        return dieAS;
    }

    public bool IsPlaying()
    {
        return dieAS.isPlaying;
    }
}
