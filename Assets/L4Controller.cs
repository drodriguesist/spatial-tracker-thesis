using System.Collections;
using System.Collections.Generic;
using Player;
using Pistol;
using UnityEngine;
using UnityEngine.Audio;

public class L4Controller : MonoBehaviour
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
            AudioManagerController.instance.AddSfxPrefab("Locations/location_main_hall_PT_sfx");
        }
        else
        {
            AudioManagerController.instance.AddSfxPrefab("Locations/location_main_hall_sfx");
        }
    }

    void SetWeapons()
    {
        PlayerController.instance.weaponInventory[0].GetComponent<PistolController>().InitInvisibleGuns();
    }

    public void SetUpLevel()
    {
        SetLevelAudio();
        SetWeapons();
    }
}
