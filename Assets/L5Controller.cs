using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class L5Controller : MonoBehaviour
{
    #region properties

    #region private
    [SerializeField] AudioClip level_defaultBGM;
    [SerializeField] AudioMixerGroup level_defaultAMG;
    #endregion

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetLevelAudio()
    {
        AudioManagerController.instance.SetDefaultBGM(level_defaultBGM);
        AudioManagerController.instance.UpdateBGM(level_defaultAMG, level_defaultBGM);
    }

    public void SetUpLevel()
    {
        SetLevelAudio();
    }
}
