using Player;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    #region properties

    #region pubblic
    public static LevelManager instance;
    #endregion

    #region private
    int currentScene;
    int nextScene;
    #endregion

    #endregion
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    // called first
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // called second
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name);
        currentScene = GetCurrentScene();
        if(currentScene != 0) IOLevelObj(currentScene); //ignore main_menu
    }

    // called when the game is terminated
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void IOLevelObj(int currentScene)
    {
        string name = "L" + currentScene.ToString() + "Controller";
        Transform currentLevelTrf = transform.Find(name);
        switch (currentScene)
        {
            case 1:
                GameManagerController.instance.InitGameManager();
                PlayerController.instance.InitPlayer();
                currentLevelTrf.gameObject.GetComponent<L1Controller>().SetUpLevel();
                break;
            case 2:
                GameManagerController.instance.InitGameManager();
                PlayerController.instance.InitPlayer();
                currentLevelTrf.gameObject.GetComponent<L2Controller>().SetUpLevel();
                break;
            case 3:
                GameManagerController.instance.InitGameManager();
                PlayerController.instance.InitPlayer();
                currentLevelTrf.gameObject.GetComponent<L3Controller>().SetUpLevel();
                break;
            case 4:
                GameManagerController.instance.InitGameManager();
                PlayerController.instance.InitPlayer();
                currentLevelTrf.gameObject.GetComponent<L4Controller>().SetUpLevel();
                break;
            case 5:
                GameManagerController.instance.InitGameManager();
                currentLevelTrf.gameObject.GetComponent<L5Controller>().SetUpLevel();
                break;
        }
    }

    public int GetCurrentScene()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }

    public void LoadNextScene()
    {
        AudioManagerController.instance.RemoveAllSfx();
        currentScene = GetCurrentScene();
        nextScene = currentScene + 1;
        SceneManager.LoadScene(nextScene);
    }

    public void LoadTrainingScene()
    {
        AudioManagerController.instance.RemoveAllSfx();
        SceneManager.LoadScene(5);
    }

    public void StartPressed()
    {
        var component = transform.Find("SFX").Find("StartGame").GetComponent<StartGameController>();
        component.OnKeyDown();
        Invoke("LoadNextScene", component.keydownAC.length);
    }

    public void TrainingPressed()
    {
        var component = transform.Find("SFX").Find("StartGame").GetComponent<StartGameController>();
        component.OnKeyDown();
        Invoke("LoadTrainingScene", component.keydownAC.length);
    }

    /// <summary>
    /// Restarts the game by loading the main menu
    /// </summary>
    public void OnRestart()
    {
        Destroy(PlayerController.instance.gameObject);
        Destroy(GameManagerController.instance.gameObject);
        AudioManagerController.instance.RemoveAllSfx();
        SceneManager.LoadScene("MainMenu");
    }
}
