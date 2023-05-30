using SuicideBomber.Audio.Roar;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DummyRobot.Audio
{
    public class DummyRobotAudioController : MonoBehaviour
    {
        #region PROPERTIES
        DieAudioController dieAC;
        HitAudioController hitAC;
        RoarAudioController roarAC;
        DummyRobotCpntroller dummyRobotC;
        bool soundEnabled;
        #endregion

        // Start is called before the first frame update
        void Start()
        {
            soundEnabled = true;
            dummyRobotC = GetComponentInParent<DummyRobotCpntroller>();
            dieAC = GetComponentInChildren<DieAudioController>();
            hitAC = GetComponentInChildren<HitAudioController>();
            roarAC = GetComponentInChildren<RoarAudioController>();
        }

        public void Mute()
        {
            soundEnabled = false;
        }

        public void UnMute()
        {
            soundEnabled = true;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if(soundEnabled)
            {
                if (dummyRobotC.GetHealthStateChanged())
                {
                    if(dummyRobotC.IsDead())
                    {
                        roarAC.StopAudio();
                        hitAC.StopAudio();
                        Debug.Log("Dummy Robot died");
                        dieAC.PlayAudio();
                        soundEnabled = false;
                    }
                    else
                    {
                        roarAC.StopAudio();
                    }
                    dummyRobotC.SetHealthStateChanged(false); //reset value by setting to false
                }
                if(!dummyRobotC.IsDead())
                {
                    roarAC.SetVolume(1f);
                    roarAC.PlayAudio();
                }
            }
        }
    }
}