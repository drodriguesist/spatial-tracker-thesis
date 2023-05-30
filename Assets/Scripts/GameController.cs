using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
	public GameObject player;
	public GameObject enemyTracker;
	public GameObject[] targets;
	// Use this for initialization

	private bool shootl;
	private bool shootr;
	private bool canDestroy;
	public AudioClip pistolSound;
	public AudioClip enemyDeathSound;
	AudioSource audioSource;
	EnemyScript es;
	SonarBehaviour _enemyTrackerScript;
	bool rbTriggerShotted;
	bool lbTriggerShotted;
	string lastShoot;

	//private
	float _distanceToTarget;
	private GameObject target;
	int _current;


	void Start () {
		_current = 0;
		target = targets[_current];
		_distanceToTarget = Vector3.Distance(player.transform.position, target.transform.position);
		rbTriggerShotted = false;
		lbTriggerShotted = false;
		shootr = false;
		shootl = false;
		canDestroy = false;
		audioSource = GetComponent<AudioSource>();
		es = target.GetComponent<EnemyScript>();
		_enemyTrackerScript = enemyTracker.GetComponent<SonarBehaviour>();
	}
	
	// Update is called once per frame
	void Update () {
		CheckDistance();
		OnButtonLBTrigger();
		OnButtonRBTrigger();
		DestroyEnemy();
	}

	/// <summary>
	
	void CheckDistance()
	{
		var distancePercentage = 0f;
		if (target != null)
		{
			var newDistanceToTarget = Vector3.Distance(player.transform.position, target.transform.position);
			distancePercentage = newDistanceToTarget / _distanceToTarget;
		}
		if(distancePercentage < 0.475f && target != null)
		{
			canDestroy = true;
		}
	}

	/// </summary>
	bool OnButtonRBTrigger()
	{
		if (Input.GetButtonDown("RBTrigger")) { 
			audioSource.PlayOneShot(pistolSound);
			shootr = true;
		}
		else
			shootr = false;
		return shootr;
	}

	bool OnButtonLBTrigger()
	{
		if (Input.GetButtonDown("LBTrigger"))
		{
			audioSource.PlayOneShot(pistolSound);
			shootl = true;

		}
		else
			shootl = false;
		return shootl;
	}

	void DestroyEnemy()
	{
		if (canDestroy && (shootr || shootl))
		{
			if (shootr)
				lastShoot = "right";
			else
				lastShoot = "left";
			if (lastShoot == es.position)
			{
				audioSource.PlayOneShot(enemyDeathSound);
				StartCoroutine(Example());
				//foreach(var item in _enemyTrackerScript.enemiesList)
				//{
				//	if (item.name == target.name)
				//		_enemyTrackerScript.enemiesList.Remove(item);
				//}
				//_enemyTrackerScript.enemiesList.Remove(target);
				_enemyTrackerScript.previousDistance = float.MaxValue;
				_enemyTrackerScript.nearestEnemyDistance = float.MaxValue;
				Destroy(target);
				canDestroy = false;
				shootr = false;
				shootl = false;
			}
		}
	}

	string CheckLastShoot()
	{
		string lastShoot1 = "";
		if (rbTriggerShotted && lbTriggerShotted)
		{
			lastShoot1 = "center";
		}
		else if (lbTriggerShotted)
		{
			lastShoot1 = "left";
		}
		else if (rbTriggerShotted)
		{
			lastShoot1 = "right";
		}
		return lastShoot1;
	}

	IEnumerator Example()
    {
        yield return new WaitForSeconds(0.3f);
    }

	//public bool EnemyAlive(GameObject )
	//{

	//}
}
