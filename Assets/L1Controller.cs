using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.Audio;

public class L1Controller : MonoBehaviour
{
    #region properties

    #region private
    [SerializeField] AudioClip level_defaultBGM;
    [SerializeField] AudioMixerGroup level_defaultAMG;
    [SerializeField] int playerHealth;
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
        AudioManagerController.instance.AddSfxPrefab("alarm_underattack_sfx");
        //AudioManagerController.instance.AddSfxPrefab("alarm_underattack_voice_sfx");
    }

    public void SetUpLevel()
    {
        SetLevelAudio();

        PlayerController.instance.SetPlayerHealth(playerHealth);
    }
}
