using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonarBehaviour : MonoBehaviour {

	#region Properties
	// public vars
	public static SonarBehaviour instance;

	public AudioClip sensorSlow;
	public AudioClip signal250;
	public AudioClip signalContinuous;
	public float nearestEnemyDistance;
	public float previousDistance;
	public float distancePercentage;

	// private vars
	AudioSource audioSource;
	Collider[] lstEnemies;
	GameObject nearestEnemy;
	Transform _lastEnemyReturned;


	float distanceToTarget;
	string lastEnemyName;

	int _currentIndex;
	#endregion

	#region Unity Functions

	private void Start()
	{
		//initialization
		_currentIndex = 0;
		nearestEnemyDistance = float.MaxValue;
		previousDistance = float.MaxValue;
        audioSource = GetComponent<AudioSource>();
		audioSource.volume = 0.85f;
		audioSource.clip = sensorSlow;
		lastEnemyName = string.Empty;
	}

	private void Update()
    {
		lstEnemies = Physics.OverlapSphere(transform.position, 30f);
		PlayAudio();
    }
    #endregion

    #region Custom Functions
    /// <summary>
    /// Plays audio file.
    /// </summary>
    private void PlayAudio()
    {
		GameObject localnearestEnemy = GetNearestEnemy();
		//check if we have a enemy near
		if (null != localnearestEnemy)
		{
			if (HasNearestEnemyChanged(localnearestEnemy)) distanceToTarget = Vector3.Distance(transform.position, localnearestEnemy.transform.position);
			if (!audioSource.isPlaying)
			{
				audioSource.Play();
			}
			float newdistanceToTarget = Vector3.Distance(transform.position, localnearestEnemy.transform.position);
			distancePercentage = newdistanceToTarget / distanceToTarget;
			if (newdistanceToTarget < previousDistance)
			{
				previousDistance = newdistanceToTarget;
				if (distancePercentage > 0.75f && distancePercentage <= 1.0f)
				{
					audioSource.clip = sensorSlow;
				}
				else if (distancePercentage > 0.45f && distancePercentage <= 0.75f)
				{
					audioSource.clip = signal250;
				}
				else if (distancePercentage <= 0.45f)
				{
					audioSource.clip = signalContinuous;
				}
			}
			else if (newdistanceToTarget > previousDistance)
			{
				//verify if enemy is not dead
				if(null != nearestEnemy)
				{
				//enemiesList.Remove(nearestEnemy);
				}
				nearestEnemyDistance = float.MaxValue;
				previousDistance = float.MaxValue;
			}
		}
		else
		{
			audioSource.Stop();
		}
	}

	private bool HasNearestEnemyChanged(GameObject gObj)
    {
		if (gObj.name != lastEnemyName)
		{
			lastEnemyName = gObj.name;
			return true;
		}
		else
			return false;
    }

	/// <summary>
	/// Fetch the closest enemy
	/// </summary>
	/// <returns></returns>
	private GameObject GetNearestEnemy()
	{
		foreach (var enemy in lstEnemies)
        {
			if(enemy.tag == "Enemy")
            {
				float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
				if (distanceToEnemy <= nearestEnemyDistance)
                {
                    nearestEnemyDistance = distanceToEnemy;
                    nearestEnemy = enemy.gameObject;
                }
            }
		}
		return nearestEnemy;
	}

    private void OnDrawGizmos()
    {
		Gizmos.DrawWireSphere(transform.position, 30f);
    }

    #endregion
}