using Player.Audio.Footsteps;
using SuicideBomber.Audio.Roar;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuicideBomber.Audio
{
    public class EnemyAudioController : MonoBehaviour
    {
        #region PROPERTIE
        AttackAudioController attackAC;
        DieAudioController dieAC;
        FootstepsAudioController footstepsAC;
        HitAudioController hitAC;
        PainAudioController painAC;
        RoarAudioController roarAC;
        SuicideBomberController suicideC;
        bool soundEnabled;
        #endregion

        // Start is called before the first frame update
        void Start()
        {
            soundEnabled = true;
            suicideC = GetComponentInParent<SuicideBomberController>();
            attackAC = GetComponentInChildren<AttackAudioController>();
            dieAC = GetComponentInChildren<DieAudioController>();
            footstepsAC = GetComponentInChildren<FootstepsAudioController>();
            hitAC = GetComponentInChildren<HitAudioController>();
            painAC = GetComponentInChildren<PainAudioController>();
            roarAC = GetComponentInChildren<RoarAudioController>();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if(soundEnabled)
            {
                if (suicideC.GetHealthStateChanged())
                {
                    if(suicideC.IsDead())
                    {
                        if(suicideC.IsAttacking()) attackAC.PlayAudio();
                        roarAC.StopAudio();
                        footstepsAC.StopPlayingAudio();
                        hitAC.StopAudio();
                        painAC.StopAudio();
                        Debug.Log("Suicide Bomber died");
                        dieAC.PlayAudio();
                        soundEnabled = false;
                    }
                    else
                    {
                        roarAC.StopAudio();
                    }
                    suicideC.SetHealthStateChanged(false); //reset value by setting to false
                }
                if(!painAC.IsPlaying() && !suicideC.IsDead())
                {
                    roarAC.PlayAudio();
                    roarAC.CheckDistanceToPlayer();
                }
                if(!suicideC.IsDead())
                {
                    footstepsAC.PlayPlayersFootstepsAudio();
                    footstepsAC.CheckDistanceToPlayer();
                }
            }
        }
    }
}