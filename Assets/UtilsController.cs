using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilsController : MonoBehaviour
{
    public enum OpenArm
    {
        L_ARM,
        R_ARM,
        NONE
    }

    public enum HoldType
    {
        SINGLE_HANDED,
        TWO_HANDED
    }

    public enum HealthAudioState
    {
        Damage,
        PickUp,
        Recover,
        None
    }

    public enum EnemyTrackerState
    {
        Slow,
        Fast,
        Continuos,
        None
    }

    public enum ArmPosition
    {
        Center = 0,
        Left = -1,
        Right = 1
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
