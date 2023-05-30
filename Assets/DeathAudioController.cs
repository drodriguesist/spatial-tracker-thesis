using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class DeathAudioController : MonoBehaviour
{
    [SerializeField] AudioClip deathAC;
    [SerializeField] AudioClip defaultBGM;
    [SerializeField] AudioMixerGroup defaultAMG;
    AudioSource deathAS;
    GameObject owner;
    Transform audioObj;

    // Start is called before the first frame update
    void Start()
    {
        deathAS = GetComponent<AudioSource>();
        audioObj = gameObject.transform.parent;
        owner = audioObj.transform.parent.gameObject;
    }

    private IEnumerator WaitForSound()
    {
        yield return new WaitUntil(() => !deathAS.isPlaying);
    }

    IEnumerator Example()
    {
        yield return new WaitForSeconds(deathAS.clip.length);
    }

    public void PlayAudio()
    {
        deathAS.clip = deathAC;
        deathAS.PlayOneShot(deathAS.clip);
        Invoke("YouDied", deathAS.clip.length);
    }

    void YouDied()
    {
        if(AudioManagerController.instance.language == "PT")
        {
            deathAS.clip = Resources.Load<AudioClip>("Audio/Player/Death/you_died_PT");
        }
        else
        {
            deathAS.clip = Resources.Load<AudioClip>("Audio/Player/Death/you_died");
        }
        deathAS.PlayOneShot(deathAS.clip);
        Invoke("Restart", deathAS.clip.length);
    }

    public void Restart()
    {
        AudioManagerController.instance.LoadMainMenuBGM();
        FindObjectOfType<LevelManager>().GetComponent<LevelManager>().OnRestart();
    }

    public void StopAudio()
    {
        deathAS.Stop();
    }

    public AudioSource GetAudioSource()
    {
        return deathAS;
    }

    public bool IsPlaying()
    {
        return deathAS.isPlaying;
    }
}
