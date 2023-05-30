using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAudioController : MonoBehaviour
{
    #region PROPERTIES
    OpenDoorController openDoorController;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        openDoorController = GetComponentInChildren<OpenDoorController>();
    }

    public void PlayAudio()
    {
        openDoorController.PlayAudio();
    }
}
