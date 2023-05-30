using Player.Audio.Footsteps;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Audio
{
    public class PlayerAudioController : MonoBehaviour
    {
        PlayerController pc;
        DeathAudioController dAC;
        BreathingAudioController bAC;
        FootstepsAudioController fAC;
        HealthAudioController hdAC;
        HeartbeatAudioController hAC;
        //AudioSource footstepsAS;
        //CharacterController cc;
        //Transform footstepsT;

        // Start is called before the first frame update
        void Start()
        {
            pc = GetComponentInParent<PlayerController>();
            bAC = GetComponentInChildren<BreathingAudioController>();
            fAC = GetComponentInChildren<FootstepsAudioController>();
            hdAC = GetComponentInChildren<HealthAudioController>();
            hAC = GetComponentInChildren<HeartbeatAudioController>();
            dAC = GetComponentInChildren<DeathAudioController>();
            //cc = GetComponentInParent<CharacterController>();
            //footstepsT = transform.Find("Footsteps");
            //footstepsAS = footstepsT.gameObject.GetComponent<AudioSource>();
        }

        private void FixedUpdate()
        {
            if(pc.GetIsWalking() || pc.GetIsWalkingKeyboard()) fAC.PlayPlayersFootstepsAudio();
            bAC.UpdatePlayerBreathingClip(pc.GetPlayerHealth());
            bAC.PlayBreathingAudio();
            hAC.UpdatePlayerHeartbeatClip(pc.GetPlayerHealth());
            hAC.PlayHeartbeatAudio();
            if (pc.GetHealthStateChanged())
            {
                fAC.StopPlayingAudio();
                bAC.StopPlayingAudio();
                hAC.StopPlayingAudio();
                if(pc.IsDead())
                {
                    Debug.Log("Player died");
                    dAC.PlayAudio();
                }
                else
                {
                    hdAC.UpdatePlayerHealthClip(pc.GetHealthState());
                    hdAC.PlayPickUpAudio(pc.GetHealthState());
                    //hdAC.PlayHealthAudio();
                }
                pc.SetHealthStateChanged(false); //reset value by setting to false
            }
        }
    }
}