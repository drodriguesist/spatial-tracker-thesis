using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour {
	//public
	public static EnemyScript instance;
	AudioSource enemyAS;
	bool keepPlaying = true;
	public AudioClip playerOnSightSound;
	public int carwait = 10;
	public string position; // TODO change to ENUM

	private void Awake()
	{
		instance = this;
	}

	// Use this for initialization
	void Start () {
		enemyAS = GetComponent<AudioSource>();
		StartCoroutine(SoundOut());
	}

	// Update is called once per frame
	void Update () {
	}

	IEnumerator SoundOut()
	{
		while (keepPlaying)
		{
			enemyAS.PlayOneShot(playerOnSightSound);
			yield return new WaitForSeconds(carwait);
		}
	}

}
