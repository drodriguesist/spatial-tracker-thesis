using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoorController : MonoBehaviour
{
    #region PROPERTIES
    [SerializeField] AudioSource openDoorAS;
    #endregion

    public void PlayAudio()
    {
        if(!openDoorAS.isPlaying)
        {
            openDoorAS.Play();
        }
        int currentScene = LevelManager.instance.GetCurrentScene();
        if(currentScene == 4) Invoke("Restart", openDoorAS.clip.length);
        else Invoke("LoadNextLevel", openDoorAS.clip.length);
    }

    public void LoadNextLevel()
    {
        LevelManager.instance.LoadNextScene();
    }

    public void Restart()
    {
        AudioManagerController.instance.LoadMainMenuBGM();
        LevelManager.instance.OnRestart();
    }
}
