using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UtilsController;

public class HealthkitController : MonoBehaviour
{
    #region PROPERTIES
    GameObject GameManager;
    GameObject Player;
    bool picked;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        GameManager = GameObject.FindGameObjectWithTag("GameController");
        Player = GameManager.GetComponent<GameManagerController>().player;
    }

    public void GainLife()
    {
        int currPlayerHealth = Player.GetComponent<PlayerController>().GetPlayerHealth();
        if (currPlayerHealth < 100)
        {
            Player.GetComponent<PlayerController>().SetPlayerHealth(currPlayerHealth + 20);
            Debug.Log("Player gained 20 pts of Life");
        }
        Player.GetComponent<PlayerController>().SetHealthStateChanged(true);
        Player.GetComponent<PlayerController>().SetHealthAudioState(HealthAudioState.Recover);
        Destroy(gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name.StartsWith("Lane"))
        {
            GameManager.GetComponent<GameManagerController>().SetEnemiesCounter(0);
            Destroy(gameObject);
        }
    }
}
