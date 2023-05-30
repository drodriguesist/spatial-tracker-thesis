using UnityEngine;
using UnityEngine.Audio;

public class TransitionBGMController : MonoBehaviour
{  
    #region Properties

    #region public
    public AudioMixerGroup combatAMG;
    public AudioClip newAudioClip;
    #endregion

    #endregion
    
    #region Unity methods
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            AudioManagerController.instance.UpdateBGM(combatAMG, newAudioClip);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            AudioManagerController.instance.RestoreToDefaultBGM();
        }
    }

    public void StartTransition()
    {
       AudioManagerController.instance.UpdateBGM(combatAMG, newAudioClip); 
    }

    public void StopTransition()
    {
       AudioManagerController.instance.RestoreToDefaultBGM();
    }
    #endregion
}
