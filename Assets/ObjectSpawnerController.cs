using System.Collections;
using Player;
using Pistol;
using System.Collections.Generic;
using UnityEngine;
using static UtilsController;

public class ObjectSpawnerController : MonoBehaviour
{
    #region properties

    #region private
    [SerializeField] Transform lane1Sp;
    [SerializeField] Transform lane2Sp;
    [SerializeField] Transform lane3Sp;
    [SerializeField] List<Transform> objToSpawn;
    [SerializeField] List<Transform>? listGridCells;
    [SerializeField] List<AudioClip>? listGridCellsClips;
    AudioSource trainingAnouncerAS;
    GameObject currentRobot;
    GameObject currentEnemyGrid;
    GameObject currentEnemyGridRumble;
    GameObject GameManager;
    GameObject transitionBGM;
    List<int> listEmptyCells;
    List<int> listDummySP;
    Transform ObjLane;
    Vector3 currentGridCellPos;
    bool callOnce;
    bool canSpawnTraining;
    bool enableTrainingMode;
    bool startPractice;
    bool ready;
    bool spawnAnother;
    int farRow = 0;
    int intermediateRow = 0;
    int nearRow = 0;
    int currentGridCell = 0;
    int robotKillsCounter = 0;
    int ammoCaughtCounter = 0;
    int controlVar = 3;
    #endregion

    #region public
    public bool canSpawn = true;
    public bool developerMode = false;
    public bool stopSpawning = false;
    public float spawnDelay = 3;
    public float spawnTime = 1;
    #endregion

    #endregion

    #region methods

    #region private

    #region unity
    // Start is called before the first frame update
    void Start()
    {
        callOnce = false;
        canSpawnTraining = false;
        enableTrainingMode = false;
        startPractice = false;
        spawnAnother = false;
        ready = false;
        trainingAnouncerAS = GetComponent<AudioSource>();
        GameManager = GameObject.FindGameObjectWithTag("GameController");
        listEmptyCells = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
        listDummySP = new List<int> { 0, 1, 2 };
        Invoke("StartSpawning", 3);
    }

    void Update()
    {
        if (enableTrainingMode)
        {
            if (listEmptyCells.Count == 0 && currentGridCell == 0)
            {
                Debug.Log("Training mode completed");
                enableTrainingMode = false;
                AudioManagerController.instance.LoadMainMenuBGM();
                LevelManager.instance.OnRestart();
            }
            else if (spawnAnother)
            {
                spawnAnother = false;
                DestroyEnemyOnGridCell();
            }
            else
            {
                SpawnEnemyTrainingMode();
            }
        }
    }
    #endregion

    /// <summary>
    /// selects a lane randomly
    /// </summary>
    /// <returns>int</returns>
    Vector3 RandomLaneSelector(bool special)
    {
        int laneNumber;
        if (special)
        {

            var laneNumberIndex = Random.Range(0, 2);
            int[] laneArray = { 0, 2 };
            laneNumber = laneArray[laneNumberIndex];
        }
        else
        {
            laneNumber = Random.Range(0, 3);
        }

        if (laneNumber == 0)
        {
            ObjLane = lane1Sp.parent;
            return lane1Sp.position;
        }
        else if (laneNumber == 1)
        {
            ObjLane = lane2Sp.parent;
            return lane2Sp.position;
        }
        else
        {
            ObjLane = lane3Sp.parent;
            return lane3Sp.position;
        }
    }

    Vector3 TrainningLaneSelector(int kills)
    {
        int listIdx = Random.Range(0, listDummySP.Count);
        int laneNumber;
        if (listDummySP.Count == 1)
        {
            laneNumber = listDummySP[0];
            listDummySP.RemoveAt(0);
        }
        else
        {
            string result = "OBSpawn List Empty Cells contents: ";
            Debug.Log(listIdx);
            foreach (var item in listDummySP)
            {
                result += item.ToString() + ", ";
            }
            Debug.Log(result);
            laneNumber = listDummySP[listIdx];
            listDummySP.RemoveAt(listIdx);
        }
        // switch(kills)
        // {
        //     case 0:
        //         laneNumber = 0;
        //         break;
        //     case 1:
        //         laneNumber = 1;
        //         break;
        //     case 2:
        //         laneNumber = 2;
        //         break;
        // }

        if (laneNumber == 0)
        {
            ObjLane = lane1Sp.parent;
            return new Vector3(lane1Sp.position.x - 6f, lane1Sp.position.y, lane1Sp.position.z);
        }
        else if (laneNumber == 1)
        {
            ObjLane = lane2Sp.parent;
            return lane2Sp.position;
        }
        else
        {
            ObjLane = lane3Sp.parent;
            return new Vector3(lane3Sp.position.x + 6f, lane3Sp.position.y, lane3Sp.position.z);
        }
    }

    string RandomObjSelector()
    {
        return objToSpawn[Random.Range(0, objToSpawn.Count)].name;
    }

    void AddSFXBuilding()
    {
        AudioManagerController.instance.AddSfxPrefab("Locations/location_armory_practice_shooting_buidling_sfx");
        Invoke("AddSFXStart", 4); //TODO Fix hardcoded

    }

    void AddSFXStart()
    {
        if (AudioManagerController.instance.language == "PT")
        {
            AudioManagerController.instance.AddSfxPrefab("Locations/location_armory_start_shooting_PT_sfx");

        }
        else
        {
            AudioManagerController.instance.AddSfxPrefab("Locations/location_armory_start_shooting_sfx");
        }
        Invoke(nameof(AddPracticeShootingBGM), 1);
    }

    void AddPracticeShootingBGM()
    {
        transitionBGM = (GameObject)Instantiate(Resources.Load("Prefabs/Audio/Transitions/PracticeShootingBGM", typeof(GameObject)), Vector3.zero, Quaternion.identity);
        transitionBGM.GetComponent<TransitionBGMController>().StartTransition();
        callOnce = true;
    }

    void SpawnDoor()
    {
        Debug.Log("Reached Door");
        if (LevelManager.instance.GetCurrentScene() == 4)
        {
            AudioSource audioSource = GetComponent<AudioSource>();
            if (AudioManagerController.instance.language == "PT")
            {
                audioSource.clip = Resources.Load<AudioClip>("Audio/Levels/MainHall/level_completed_PT");
            }
            else
            {
                audioSource.clip = Resources.Load<AudioClip>("Audio/Levels/MainHall/level_completed");
            }
            audioSource.PlayOneShot(audioSource.clip);
        }
        Instantiate(Resources.Load("Prefabs/Door", typeof(GameObject)), lane2Sp.position, Quaternion.Euler(0, 180, 0));
    }

    void StartSpawning()
    {
        //TODO clean this
        if (LevelManager.instance.GetCurrentScene() == 2) InvokeRepeating("MedicalCenterSpawnObjects", 3, 8);
        else if (LevelManager.instance.GetCurrentScene() == 3) InvokeRepeating("ArmorySpawnObjects", 2, 7);
        else if (LevelManager.instance.GetCurrentScene() == 5) Invoke("EnableTrainingSpawn", 0);
        else InvokeRepeating("SpawnObjects", spawnTime, spawnDelay);
    }

    /// <summary>
    /// Returns a different lane set depending on the object to spawn
    /// if its a healthkit or ammo the lanes returned will be 1 or 3
    /// the rest will be randomly distributed
    /// </summary>
    /// <returns></returns>
    Vector3 LaneSelectionMode()
    {
        if (objToSpawn[0].CompareTag("HealthKit") || objToSpawn[0].CompareTag("Ammo"))
        {
            return RandomLaneSelector(true);
        }
        else
        {
            return RandomLaneSelector(false);
        }
    }

    void EnableTrainingSpawn()
    {
        Debug.Log("Start Training mode");
        enableTrainingMode = true;
        canSpawnTraining = true;
    }

    (Vector3, int) GetRandomCell()
    {
        int cellIdx = Random.Range(listEmptyCells[0], listEmptyCells.Count);
        int toReturn = 0;
        if (listEmptyCells.Count == 1)
        {
            toReturn = listEmptyCells[0];
            listEmptyCells.RemoveAt(0);
        }
        else
        {
            toReturn = listEmptyCells[cellIdx];
            listEmptyCells.RemoveAt(cellIdx);
        }
        return (listGridCells[toReturn].position, toReturn);
    }

    Transform GetLaneTraining(int cellIdx)
    {
        switch (cellIdx)
        {
            case 0:
            case 3:
            case 6:
                ObjLane = lane1Sp.parent;
                break;
            case 1:
            case 4:
            case 7:
                ObjLane = lane2Sp.parent;
                break;
            case 2:
            case 5:
            case 8:
                ObjLane = lane3Sp.parent;
                break;
        }
        return ObjLane;
    }

    void SetRowInfo(int cellIdx)
    {
        bool doFarRow = false;
        bool doIntermediateRow = false;
        bool doNearRow = false;
        switch (cellIdx)
        {
            case 0:
            case 1:
            case 2:
                doFarRow = true;
                break;
            case 3:
            case 4:
            case 5:
                doIntermediateRow = true;
                break;
            case 6:
            case 7:
            case 8:
                doNearRow = true;
                break;
        }

        if (doFarRow && farRow == 0)
        {
            farRow = cellIdx;
        }
        else if (doIntermediateRow && intermediateRow == 0)
        {
            intermediateRow = cellIdx;
        }
        else if (doNearRow && nearRow == 0)
        {
            nearRow = cellIdx;
        }
    }

    void DestroyEnemyOnGridCell()
    {
        Destroy(currentEnemyGrid);

        currentGridCell = 0;
        canSpawnTraining = true;
    }

    void SpawnEnemyTrainingMode()
    {
        if (canSpawnTraining)
        {
            Debug.Log("Estou a entrar");
            try
            {
                var test2 = GameObject.FindGameObjectsWithTag("Enemy");
                foreach (var n in test2)
                {
                    Destroy(n);
                }
                canSpawnTraining = false;
                //step 1 - Get grid cell
                (Vector3, int) cell = GetRandomCell();
                currentGridCellPos = cell.Item1;
                currentGridCell = cell.Item2;
                //step2 - announce localization
                trainingAnouncerAS.clip = listGridCellsClips[currentGridCell];
                trainingAnouncerAS.Play();
                Debug.Log("Estou a entrar4");
                Invoke(nameof(SpawnTrainingEnemy), trainingAnouncerAS.clip.length + 0.5f);
            }
            catch (System.Exception e)
            {
                Debug.Log("Something went wrong" + e.StackTrace);
            }

        }
    }

    void SpawnTrainingEnemy()
    {
        Debug.Log("Prepare to spawn");
        GameObject gObj = (GameObject)Instantiate(Resources.Load("Prefabs/SuicideBomber", typeof(GameObject)), currentGridCellPos, Quaternion.Euler(0, 180, 0));
        currentEnemyGrid = gObj;

        //step1 - Set lane
        SetRowInfo(currentGridCell);
        ObjLane = GetLaneTraining(currentGridCell);
        gObj.GetComponent<SuicideBomberController>().SetLane(ObjLane);

        //step2 - Instantiate Enemy Tracker
        var gObjEnTr = (GameObject)Instantiate(Resources.Load("Prefabs/EnemyTrackerAS", typeof(GameObject)), new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
        gObjEnTr.GetComponent<EnemyTrackerController>().trainingMode = true;
        //step3 - Set Enemy as parent of Enemy Tracker
        gObjEnTr.transform.SetParent(gObj.transform);
        Debug.Log("SuicidelBomber spawned during training");
    }
    #endregion

    #region public

    public void SetSpawnAnother(bool value)
    {
        spawnAnother = value;
    }

    public int GetGridCell()
    {
        return currentGridCell;
    }

    public void AddDummyRobotKills()
    {
        robotKillsCounter++;
    }

    public void SpawnObjects()
    {
        GameObject gObj;
        if (objToSpawn.Count == 0)
        {
            stopSpawning = true;
        }
        else
        {
            Vector3 lane = LaneSelectionMode();
            if (developerMode)
                gObj = (GameObject)Instantiate(Resources.Load("Prefabs/" + RandomObjSelector(), typeof(GameObject)), lane, Quaternion.Euler(0, 180, 0));
            else
            {
                gObj = (GameObject)Instantiate(Resources.Load("Prefabs/" + objToSpawn[0].name, typeof(GameObject)), lane, Quaternion.Euler(0, 180, 0));
                Debug.Log(objToSpawn[0].name + " spawned");
                objToSpawn.RemoveAt(0); //remove 
            }
            if (gObj.name.StartsWith("SuicideBomber") && InputManagerController.instance.isTrackerEnabled)
            {
                //step1 - Set lane
                gObj.GetComponent<SuicideBomberController>().SetLane(ObjLane);

                //step2 - Instantiate Enemy Tracker
                var gObjEnTr = (GameObject)Instantiate(Resources.Load("Prefabs/EnemyTrackerAS", typeof(GameObject)), new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));

                //step3 - Set Enemy as parent of Enemy Tracker
                gObjEnTr.transform.SetParent(gObj.transform);
            }
        }
        if (stopSpawning)
        {
            CancelInvoke("SpawnObject");
            Invoke("SpawnDoor", 3);
        }
    }

    public void MedicalCenterSpawnObjects()
    {
        if (PlayerController.instance.GetPlayerHealth() == 100)
        {
            CancelInvoke("MedicalCenterSpawnObjects");
            Invoke("SpawnDoor", 0);
        }
        else
        {
            SpawnMedicalkits();
        }
    }

    void SpawnMedicalkits()
    {
        Vector3 lane = LaneSelectionMode();
        Instantiate(Resources.Load("Prefabs/" + objToSpawn[0].name, typeof(GameObject)), lane, Quaternion.Euler(0, 180, 0));
        Debug.Log(objToSpawn[0].name + " spawned");
    }

    public void ArmorySpawnObjects()
    {
        GameObject gObj = null;
        if (startPractice)
        {
            if (!callOnce)
            {
                if (AudioManagerController.instance.language == "PT")
                {
                    AudioManagerController.instance.AddSfxPrefab("Locations/location_armory_practice_shooting_PT_sfx");
                }
                else
                {
                    AudioManagerController.instance.AddSfxPrefab("Locations/location_armory_practice_shooting_sfx");
                }
                Invoke("AddSFXBuilding", 2); // TODO Fix hardcoded
            }
            else
            {
                Debug.Log("Robot Kiils:" + robotKillsCounter.ToString());
                if (robotKillsCounter >= 3)
                {
                    //stopSpawning = true;
                    canSpawn = false;
                    robotKillsCounter = 0;
                    transitionBGM.GetComponent<TransitionBGMController>().StopTransition();
                    if (AudioManagerController.instance.language == "PT")
                    {
                        AudioManagerController.instance.AddSfxPrefab("Locations/location_armory_end_shooting_PT_sfx");
                    }
                    else
                    {
                        AudioManagerController.instance.AddSfxPrefab("Locations/location_armory_end_shooting_sfx");
                    }
                    CancelInvoke("ArmorySpawnObjects");
                    Invoke("SpawnDoor", 2);
                }
                else
                {
                    //start practice = false;
                    if (canSpawn)
                    {
                        if (listDummySP.Count > 0)
                        {
                            //Start spawning dummy robot
                            Vector3 lane = TrainningLaneSelector(robotKillsCounter);
                            gObj = (GameObject)Instantiate(Resources.Load("Prefabs/DummyRobot", typeof(GameObject)), new Vector3(lane.x, lane.y, lane.z - 90f), Quaternion.Euler(0, 180, 0));
                            Debug.Log("Dummy Robot spawned");

                            //step1 - set controller
                            var controller = gObj.GetComponent<DummyRobotCpntroller>();

                            //step1 - Set lane
                            controller.SetLane(ObjLane);

                            if (InputManagerController.instance.isTrackerEnabled)
                            {
                                gObj.AddComponent<RumbleController>();
                                controller.StartRumble(ObjLane);
                            }
                            currentRobot = gObj;
                            canSpawn = false;
                        }
                    }
                    else
                    {
                        if (PlayerController.instance.weaponInventory[0].GetComponent<PistolController>().AmmoCapacity == 0)
                        {
                            if (null != currentRobot)
                            {
                                if (InputManagerController.instance.isTrackerEnabled) currentRobot.GetComponent<RumbleController>().StopRumble();
                                currentRobot.SetActive(false);
                                Vector3 lane = LaneSelectionMode();
                                Instantiate(Resources.Load("Prefabs/" + objToSpawn[0].name, typeof(GameObject)), lane, Quaternion.Euler(0, 180, 0));
                                Debug.Log(objToSpawn[0].name + " spawned");
                            }
                            else if (controlVar > 0)
                            {
                                Vector3 lane = LaneSelectionMode();
                                Instantiate(Resources.Load("Prefabs/" + objToSpawn[0].name, typeof(GameObject)), lane, Quaternion.Euler(0, 180, 0));
                                Debug.Log(objToSpawn[0].name + " spawned v2");
                            }
                        }
                        else
                        {
                            if (!currentRobot.activeSelf)
                            {
                                currentRobot.SetActive(true);
                                var lane = currentRobot.GetComponent<DummyRobotCpntroller>().Lane;
                                currentRobot.GetComponent<DummyRobotCpntroller>().StartRumble(lane);
                            }
                        }
                    }
                }
            }
        }
        else
        {
            if (PlayerController.instance.weaponInventory.Count == 0 && LevelManager.instance.GetCurrentScene() == 3)
            {
                Instantiate(Resources.Load("Prefabs/Glock", typeof(GameObject)), new Vector3(lane2Sp.position.x, lane2Sp.position.y, lane2Sp.position.z - 90f), Quaternion.Euler(0, 180, 0));
                Debug.Log("Glock spawned");
            }
            else
            {
                Vector3 lane = LaneSelectionMode();
                Instantiate(Resources.Load("Prefabs/" + objToSpawn[0].name, typeof(GameObject)), lane, Quaternion.Euler(0, 180, 0));
                Debug.Log(objToSpawn[0].name + " spawned");
                if (ammoCaughtCounter > 0)
                {
                    objToSpawn.RemoveAt(0); //remove 
                    startPractice = true;
                }
            }
        }
        //}
    }

    public void SetAmmoCaughtCounter(int value)
    {
        ammoCaughtCounter = value;
    }

    #endregion


    #endregion
}