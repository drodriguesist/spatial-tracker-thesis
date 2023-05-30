using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManagerController : MonoBehaviour
{
    #region Properties

    #region public
    public AudioClip defaultBGM;
    public AudioMixerGroup defaultAMG;
    public static AudioManagerController instance;
    public string language;
    Transform sfx;
    #endregion

    #region private
    AudioSource currentAS;
    AudioSource newAS;
    bool isCurrentASPlaying;

    bool loop;
    #endregion

    #endregion

    #region Unity methods
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // init
        currentAS = gameObject.AddComponent<AudioSource>();
        newAS = gameObject.AddComponent<AudioSource>();
        sfx = transform.Find("SFX");
        isCurrentASPlaying = false;
        loop = true;
        language = "PT";
        UpdateBGM(defaultAMG, defaultBGM);
    }
    #endregion

    #region class methods

    #region public
    public void UpdateBGM(AudioMixerGroup newAMG, AudioClip newClip)
    {
        StopAllCoroutines();

        StartCoroutine(FadeTransition(newAMG, newClip));

        isCurrentASPlaying = !isCurrentASPlaying;
    }

    public void RestoreToDefaultBGM()
    {
        UpdateBGM(defaultAMG, defaultBGM);
    }

    public void LoadMainMenuBGM()
    {
        defaultBGM = Resources.Load<AudioClip>("Audio/Levels/DefaultBGM");
        UpdateBGM(defaultAMG, defaultBGM);
    }

    public void SetDefaultBGM(AudioClip clip)
    {
        defaultBGM = clip;
    }

    public void SetLoop(bool value)
    {
        loop = value;
    }

    public void AddSfxPrefab(string sfxPrefab)
    {
        GameObject sfxObj = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Audio/"+sfxPrefab), Vector3.zero, Quaternion.identity);
        sfxObj.transform.parent = sfx;
    }

    public void RemoveAllSfx()
    {
        int count = sfx.childCount;
        for (int i = 0; i <= count - 1; i++)
        {
            Destroy(sfx.GetChild(i).gameObject);
        }
    }
    #endregion

    #region private
    private IEnumerator FadeTransition(AudioMixerGroup newAMG, AudioClip newClip)
    {
        float timeToFade = 0.75f;
        float timeElapsed = 0;

        if(isCurrentASPlaying)
        {
            newAS.clip = newClip;
            newAS.outputAudioMixerGroup = newAMG;
            newAS.loop = loop;
            newAS.Play();

            while(timeElapsed < timeToFade)
            {
                newAS.volume = Mathf.Lerp(0, 1, timeElapsed / timeToFade);
                currentAS.volume = Mathf.Lerp(1, 0, timeElapsed / timeToFade);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            currentAS.Stop();
        }
        else
        {
            currentAS.clip = newClip;
            currentAS.outputAudioMixerGroup = newAMG;
            currentAS.loop = loop;
            currentAS.Play();

            while(timeElapsed < timeToFade)
            {
                currentAS.volume = Mathf.Lerp(0, 1, timeElapsed / timeToFade);
                newAS.volume = Mathf.Lerp(1, 0, timeElapsed / timeToFade);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            newAS.Stop();
        }
    }
    #endregion

    #endregion
}
