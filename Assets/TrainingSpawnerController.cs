using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingSpawnerController : MonoBehaviour
{
    #region properties

    #region private
    [SerializeField] List<Transform> listGridCells;
    [SerializeField] List<AudioClip> listGridCellsClips;
    [SerializeField] List<AudioClip> listGridCellsClipsPT;
    [SerializeField] List<AudioClip> listBeeps;
    [SerializeField] AudioClip[] announcerClips; //0 start training //1 training complete
    [SerializeField] AudioClip[] announcerClipsPT;
    AudioSource trainingSpawnerAS;
    GameObject trainingObject;

    List<int> listEmptyCells;
    bool firstTime;
    int currentGridCell;
    #endregion

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        firstTime = false;
        listEmptyCells = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
        trainingSpawnerAS = GetComponent<AudioSource>();
        if(AudioManagerController.instance.language == "PT")
            trainingSpawnerAS.clip = announcerClipsPT[0];
        else
        {
            trainingSpawnerAS.clip = announcerClips[0];   
        }
        trainingSpawnerAS.Play();
        Invoke(nameof(SpawnTrainObject), trainingSpawnerAS.clip.length);  
    }

    (Vector3, int) GetRandomCell()
    {
        int toReturn;
        int cellIdx = Random.Range(0, listEmptyCells.Count);
        string result = "List Empty Cells contents: ";
        Debug.Log("Index: " + cellIdx);
        if(listEmptyCells.Count == 1)
        {
            Debug.Log("Last one" );
            toReturn = listEmptyCells[0];
            listEmptyCells.RemoveAt(0);
        }
        else
        {
            Debug.Log("Count: " + listEmptyCells.Count);
            Debug.Log("Removing index: " + cellIdx);
            foreach (var item in listEmptyCells)
            {
                result += item.ToString() + ", ";
            }
            Debug.Log(result);
            toReturn = listEmptyCells[cellIdx];
            Debug.Log("To Return: "+toReturn);
            foreach (var item in listEmptyCells)
            {
                result += item.ToString() + ", ";
            }
            Debug.Log(result);
            listEmptyCells.RemoveAt(cellIdx);
        }
        return (listGridCells[toReturn].position, toReturn);
    }

    string GetBeepClipStr(int cellIdx)
    {
        switch(cellIdx)
        {
            case 0:
            case 1:
            case 2:
                return "Slow"; //slow
            case 3:
            case 4:
            case 5:
                return "Fast"; //fast
            case 6:
            case 7:
            case 8:
                return "Continuous"; //continuous
        }
        return "";
    }

    public void StartRumble(int currentGridCell)
    {
        if(GetComponent<RumbleController>().GamepadPresent())
        {
            switch(currentGridCell)
            {
                case 0:
                case 3:
                case 6:
                    gameObject.GetComponent<RumbleController>().RumblePulse(1f, 0f);
                    break;
                case 1:
                case 4:
                case 7:
                    gameObject.GetComponent<RumbleController>().RumbleConstant();
                    break;
                case 2:
                case 5:
                case 8:
                    gameObject.GetComponent<RumbleController>().RumblePulse(0f, 1f);
                    break;
            }
        }
    }

    void SpawnTrainObject()
    {
        Debug.Log("Start Training - Prepare to spawn training object");
        firstTime = true;

        // step 1 - Get Random Cell to spawn object
        (Vector3, int) cell = GetRandomCell();
        currentGridCell = cell.Item2;

        // step 2 - Spawn training object
        trainingObject = (GameObject)Instantiate(Resources.Load("Prefabs/SceneObjects/TrainingObject", typeof(GameObject)), cell.Item1, Quaternion.Euler(0, 180, 0));
        Debug.Log("Training object spawned");
        InvokeRepeating(nameof(UpdateGridCell), 0, 10);
    }

    void UpdateGridCell()
    {
        // step 1 - stop audio
        Transform test = trainingObject.transform.Find("SFX");
        AudioSource tObjAS = test.Find(GetBeepClipStr(currentGridCell)).GetComponent<AudioSource>();
        StartFade(tObjAS, 2, 0);
        tObjAS.Stop();

        // step 2 - stop rumble
        GetComponent<RumbleController>().StopRumble();

        if(listEmptyCells.Count > 0)
        {
            (Vector3, int) newCell;

            // step 1 -  get new grid cell
            if(!firstTime)
            {
                Debug.Log("Update grid cell");
                newCell = GetRandomCell();

                // update training object position
                trainingObject.transform.position = newCell.Item1;
                currentGridCell = newCell.Item2;
            }
            else firstTime = false;

            // step 2 - announce grid position
            Debug.Log("Announce cell position");
            if(AudioManagerController.instance.language == "PT")
            {
                trainingSpawnerAS.clip = listGridCellsClipsPT[currentGridCell];
            }
            else
            {
                trainingSpawnerAS.clip = listGridCellsClips[currentGridCell];
            }
            trainingSpawnerAS.Play();
            Invoke(nameof(PlayBeep), trainingSpawnerAS.clip.length+1f);
        }
        else
        {
            CancelInvoke(nameof(UpdateGridCell));
            Destroy(trainingObject);
            if(AudioManagerController.instance.language == "PT")
            {
                trainingSpawnerAS.clip = announcerClipsPT[1];
            }
            else
            {
                trainingSpawnerAS.clip = announcerClips[1];
            }
            trainingSpawnerAS.Play();
            Invoke(nameof(ReturnMainMenu), trainingSpawnerAS.clip.length);
        }
    }

    void PlayBeep()
    {
        // step 3 - play sound on loop
        Debug.Log("Play beep sound");
        Transform test = trainingObject.transform.Find("SFX");
        AudioSource tObjAS = test.Find(GetBeepClipStr(currentGridCell)).GetComponent<AudioSource>();
        StartFade(tObjAS, 2, 1);
        tObjAS.loop = true;
        tObjAS.Play();

        // step 4 - start rumble
        StartRumble(currentGridCell);
    }

    IEnumerator StartFade(AudioSource audiosource, float duration, float targetVolume)
    {
        float currentTime = 0;
        float start = audiosource.volume;
        while(currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audiosource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        yield break;
    }

    void ReturnMainMenu()
    {
        AudioManagerController.instance.LoadMainMenuBGM();
        LevelManager.instance.OnRestart();
    }
}
