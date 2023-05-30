using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class L3Controller : MonoBehaviour
{   
    #region private
    [SerializeField] AudioClip level_defaultBGM;
    [SerializeField] AudioMixerGroup level_defaultAMG;
    #endregion

    void SetLevelAudio()
    {
        AudioManagerController.instance.SetDefaultBGM(level_defaultBGM);
        AudioManagerController.instance.UpdateBGM(level_defaultAMG, level_defaultBGM);

        if(AudioManagerController.instance.language == "PT")
        {
            AudioManagerController.instance.AddSfxPrefab("Locations/location_armory_PT_sfx");
        }
        else
        {
            AudioManagerController.instance.AddSfxPrefab("Locations/location_armory_sfx");
        }
        
    }

    public void SetUpLevel()
    {
        SetLevelAudio();
    }
}
