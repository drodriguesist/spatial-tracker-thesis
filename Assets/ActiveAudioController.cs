using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveAudioController : MonoBehaviour
{

    #region PROPERTIES
    [SerializeField] AudioClip activeAC;
    AudioSource activeAS;
    GameObject GameManager;
    Transform target;
    float previousDistance;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
        GameManager = GameObject.FindGameObjectWithTag("GameController");
        target = GameManager.GetComponent<GameManagerController>().player.transform;
		previousDistance = float.MaxValue;
        activeAS = GetComponent<AudioSource>();
        activeAS.clip = activeAC;
        activeAS.volume = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayAudio()
    {
        if(!activeAS.isPlaying)
        {
            activeAS.Play();
        }
    }

    public void CheckDistanceToPlayer()
    {
        float newdistanceToTarget = Vector3.Distance(target.position, transform.parent.position);
        if (newdistanceToTarget < previousDistance)
        {
            activeAS.volume += 0.003f;
        }
    }
}