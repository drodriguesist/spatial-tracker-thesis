using System.Collections;
using UnityEngine;
using static UtilsController;

public class EnemyTrackerController : MonoBehaviour
{
    #region Properties
    [SerializeField] AudioClip[] enemyTrackerAC;
    Transform Target {get; set;}
    SuicideBomberController LaneHolderSC { get; set; }
    DummyRobotCpntroller LaneHolderDC { get; set; }
    int currentGridCell;
    int numberTimesToPlay;
    float distanceToTarget;
    float previousDistance;
    bool callOnce = false;
    bool stopPlaying = false;
    public bool trainingMode = false;
    public bool IsRead {get; set;}
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        currentGridCell = 0;
        numberTimesToPlay = 10;
        Target = FetchTarget(transform.parent.gameObject);
        distanceToTarget = Vector3.Distance(Target.position, transform.parent.position);
		previousDistance = float.MaxValue;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(null != LaneHolderSC)
        {
            if(LaneHolderSC.IsDead())
                gameObject.GetComponent<RumbleController>().StopRumble();
        }
        else if(null != LaneHolderDC)
        {
            if(LaneHolderDC.IsDead())
                gameObject.GetComponent<RumbleController>().StopRumble();
        }
        if(!trainingMode)
            PlayEnemyTrackerAudio();
        else
        {
            PlayEnemyTrackerAudioTraining();
        }
    }

    private Transform FetchTarget(GameObject parent)
    {
        Transform lane;
        //step1 get parent lane
        LaneHolderSC = parent.GetComponent<SuicideBomberController>(); //this needs to be updated later to be more generic
        if(LaneHolderSC is null)
        {
            //try Dummy Robot
            LaneHolderDC = parent.GetComponent<DummyRobotCpntroller>();
            lane = LaneHolderDC.Lane;
        }
        else
        {
            lane = LaneHolderSC.Lane;
        }

        //setp2 get target from lane
        foreach(Transform trf in lane.transform)
        {
            if(trf.name.StartsWith("ETAudio")) return trf;
        }

        return null;
    }

    /// <summary>
    /// Assign a EnemyTrackerState according to the distance between the Enemy and the Player
    /// </summary>
    /// <returns></returns>
    private EnemyTrackerState CheckDistanceToPlayer()
    {
        if(null != LaneHolderSC)
        {
            if(LaneHolderSC.IsDead())
                return EnemyTrackerState.None;
        }
        else if(null != LaneHolderDC)
        {
            if(LaneHolderDC.IsDead())
                return EnemyTrackerState.None;
        }
        float newdistanceToTarget = Vector3.Distance(Target.position, transform.parent.position);
        var distancePercentage = newdistanceToTarget / distanceToTarget;
        if (newdistanceToTarget < previousDistance)
        {
            previousDistance = newdistanceToTarget;
            if (distancePercentage > 0.7f)
            {
                return EnemyTrackerState.Slow;
            }
            else if (distancePercentage > 0.35f && distancePercentage <= 0.7f)
            {
                if (!callOnce)
                {
                    if(LaneHolderDC is null)
                    {
                        StartRumble(LaneHolderSC.Lane.transform);
                    }
                    else
                    {
                        StartRumble(LaneHolderDC.Lane.transform);
                    }
                }
                return EnemyTrackerState.Fast;
            }
            else if (distancePercentage <= 0.35f)
            {
                return EnemyTrackerState.Continuos;
            }
        }
        return EnemyTrackerState.None;
    }
    
    private EnemyTrackerState CheckDistanceToPlayerTraining()
    {
        currentGridCell = GameObject.FindGameObjectWithTag("ObjectSpawner").GetComponent<ObjectSpawnerController>().GetGridCell();
        switch(currentGridCell)
        {
            case 0:
            case 1:
            case 2:
                return EnemyTrackerState.Slow;
            case 3:
            case 4:
            case 5:
                if(!callOnce) StartRumble(LaneHolderSC.Lane.transform);
                return EnemyTrackerState.Fast;
            case 6:
            case 7:
            case 8:
                if(!callOnce) StartRumble(LaneHolderSC.Lane.transform);
                return EnemyTrackerState.Continuos;
        }
        return EnemyTrackerState.None;
    }

    /// <summary>
    /// Convert the provided state into a real audio clip
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    public AudioClip UpdateEnemyTrackerClip(EnemyTrackerState state)
    {
        if (state == EnemyTrackerState.Slow)
        {
            return enemyTrackerAC[0];
        }
        else if (state == EnemyTrackerState.Fast)
        {
           return enemyTrackerAC[1];
        }
        else if (state == EnemyTrackerState.Continuos)
        {
            return enemyTrackerAC[2];
        }
        else
        {
            return null;//none
        }
    }

    /// <summary>
    /// Calculates the right audio clip according to the distance between Enemy to the Player
    /// then sends the clip to the ETAudioController to be played
    /// </summary>
    public void PlayEnemyTrackerAudio()
    {
        AudioClip clip = UpdateEnemyTrackerClip(CheckDistanceToPlayer());
        Target.GetComponent<ETAudioController>().PlayEnemyTrackerAudio(clip);
    }

    ETAudioController test;
    public void PlayEnemyTrackerAudioTraining()
    {
        AudioClip clip = UpdateEnemyTrackerClip(CheckDistanceToPlayerTraining());
        Target.GetComponent<ETAudioController>().PlayEnemyTrackerAudio(clip);
        test = Target.GetComponent<ETAudioController>();
        Invoke("StopTraining", 9);
    }

    void StopTraining()
    {
        test.GetAudioSource().Stop();
        var objSpawner = GameObject.FindGameObjectWithTag("ObjectSpawner").GetComponent<ObjectSpawnerController>();
        
        // step 1 - reset variables
        currentGridCell = 0;
        trainingMode = false;

        // step 2 - destroy object
        LaneHolderSC.transform.Find("EnemyTrackerAS(Clone)").GetComponent<RumbleController>().StopRumble();
        objSpawner.SetSpawnAnother(true);
    }

    void StartRumble(Transform lane)
    {
        if(gameObject.GetComponent<RumbleController>().GamepadPresent())
        {
            switch(lane.name)
            {
                case "Lane1":
                    gameObject.GetComponent<RumbleController>().RumblePulse(1f, 0f);
                    break;
                case "Lane2":
                    gameObject.GetComponent<RumbleController>().RumbleConstant();
                    break;
                case "Lane3":
                    gameObject.GetComponent<RumbleController>().RumblePulse(0f, 1f);
                    break;
            }
        }   
        callOnce = true;
    }
}
