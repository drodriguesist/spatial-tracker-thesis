using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PickableObjects.Audio
{
    public class PickableObjectAudioController : MonoBehaviour
    {
        #region PROPERTIES
        ActiveAudioController activeAC;
        #endregion

        // Start is called before the first frame update
        void Start()
        {
            activeAC = GetComponentInChildren<ActiveAudioController>();
        }

        void FixedUpdate()
        {
            activeAC.PlayAudio();
            activeAC.CheckDistanceToPlayer();
        }
    }
}
