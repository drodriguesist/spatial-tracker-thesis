using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthTrackerController : MonoBehaviour
{
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
	Collider[] lstHealthkits;
	GameObject nearestEnemy;
	Transform _lastEnemyReturned;
	GameObject player;

	public float distanceToTarget;
	Vector3 lastHealthKitPos;

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
		player = GameObject.FindGameObjectWithTag("Player");
		audioSource.volume = 0.85f;
		audioSource.clip = sensorSlow;

		lastHealthKitPos = Vector3.zero;
	}

	private void Update()
	{
		lstHealthkits = Physics.OverlapSphere(player.transform.position, 45f);
		if(ScanForHealthkits(lstHealthkits))
        {
			UpdateAudioClip();
        }
		PlayAudio();
	}
	#endregion

	#region Custom Functions
	
	bool ScanForHealthkits(Collider[] lstHealthkits)
    {
		foreach (var item in lstHealthkits)
		{
			if (item.tag == "HealthKit")
			{
				return true;
			}
		}
		return false;
	}

	void UpdateAudioClip()
	{
		//GameObject localnearestEnemy = GetNearestEnemy();
		bool isHealthKitRange = false;
		foreach (var item in lstHealthkits)
		{
			if (item.tag == "HealthKit")
			{
				float distanceToKit = Vector3.Distance(player.transform.position, item.transform.position);
				if (distanceToKit < nearestEnemyDistance)
				{
					nearestEnemyDistance = distanceToKit;
					nearestEnemy = item.gameObject;
					isHealthKitRange = true;
				}
				else isHealthKitRange = false;
			}
		}
		if(isHealthKitRange)
        {
			if (HasNearestEnemyChanged(nearestEnemy))
			{
				distanceToTarget = Vector3.Distance(player.transform.position, nearestEnemy.transform.position);
				previousDistance = float.MaxValue;
			}
		}
		float newdistanceToTarget = Vector3.Distance(player.transform.position, nearestEnemy.transform.position);
		distancePercentage = newdistanceToTarget / distanceToTarget;
		if (newdistanceToTarget < previousDistance)
		{
			previousDistance = newdistanceToTarget;
            if (distancePercentage > 0.75f)
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
			//else if (newdistanceToTarget > previousDistance)
			//{
			//	//verify if enemy is not dead
			//	if (null != nearestEnemy)
			//	{
			//		var test = "ola";
			//		//enemiesList.Remove(nearestEnemy);
			//	}
			//	nearestEnemyDistance = float.MaxValue;
			//	previousDistance = float.MaxValue;
			//}
		//}
	}
	/// <summary>
	/// Plays audio file.
	/// </summary>
	private void PlayAudio()
	{
		//GameObject localnearestEnemy = GetNearestEnemy();
		//check if we have a enemy near
		//if (null != localnearestEnemy)
		//{
		//	if (HasNearestEnemyChanged(localnearestEnemy)) distanceToTarget = Vector3.Distance(player.transform.position, localnearestEnemy.transform.position);
		//	float newdistanceToTarget = Vector3.Distance(player.transform.position, localnearestEnemy.transform.position);
		//	distancePercentage = newdistanceToTarget / distanceToTarget;
		//	if (newdistanceToTarget < previousDistance)
		//	{
		//		previousDistance = newdistanceToTarget;
		//		if (distancePercentage > 7f)
		//		{
		//			audioSource.clip = sensorSlow;
		//		}
		//		else if (distancePercentage > 0.4f && distancePercentage <= 0.7f)
		//		{
		//			audioSource.clip = signal250;
		//		}
		//		//if (distancePercentage > 0.55f && distancePercentage <= 0.9f)
		//		//{
		//		//	audioSource.clip = signal250;
		//		//}
		//		else if (distancePercentage <= 0.4f)
		//		{
		//			audioSource.clip = signalContinuous;
		//		}
		//	}
		//	else if (newdistanceToTarget > previousDistance)
		//	{
		//		//verify if enemy is not dead
		//		if (null != nearestEnemy)
		//		{
		//			var test = "ola";
		//			//enemiesList.Remove(nearestEnemy);
		//		}
		//		nearestEnemyDistance = float.MaxValue;
		//		previousDistance = float.MaxValue;
		//	}
		if (!audioSource.isPlaying) audioSource.Play();
		//}
	}

	private bool HasNearestEnemyChanged(GameObject gObj)
	{
		if (gObj.transform.position != lastHealthKitPos)
		{
			lastHealthKitPos = gObj.transform.position;
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
		bool isHealthKitRange = false;
		foreach (var item in lstHealthkits)
		{
			if (item.tag == "HealthKit")
			{
				float distanceToKit = Vector3.Distance(player.transform.position, item.transform.position);
				if (distanceToKit < nearestEnemyDistance)
				{
					nearestEnemyDistance = distanceToKit;
					nearestEnemy = item.gameObject;
					isHealthKitRange = true;
				}
				else isHealthKitRange = false;
			}
		}
		if (isHealthKitRange) return nearestEnemy;
		else return null;
	}

	public void SetNearestEnemyDistance(float value)
    {
		nearestEnemyDistance = value;
    }

    //use for debug purpose
    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawWireSphere(player.transform.position, 45f);
    //}
    #endregion
}
