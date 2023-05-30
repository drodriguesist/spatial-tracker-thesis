using Player;
using UnityEngine;

public class AmmoController : MonoBehaviour
{
    #region PROPERTIES
    GameObject GameManager;
    GameObject Player;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        GameManager = GameObject.FindGameObjectWithTag("GameController");
        Player = GameManager.GetComponent<GameManagerController>().player;
    }

    public void ReloadWeapon()
    {
        Player.GetComponent<PlayerController>().ReloadWeapon();
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
