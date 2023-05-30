using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour {
	public Transform[] target;
	public float speed;

	private int _current;
	private int _zeroIndex; //move to utils class
	private bool _canMove;

	// Use this for initialization
	void Start () {
		_zeroIndex = 0;
		_canMove = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position != target[_current].position && _canMove)
		{
			Quaternion targetRotation = Quaternion.LookRotation(target[_current].position - transform.position);
			transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 20 * speed * Time.deltaTime);
			Vector3 pos = Vector3.MoveTowards(transform.position, target[_current].position, speed * Time.deltaTime);
			GetComponent<Rigidbody>().MovePosition(pos);
		}
		else
		{
			_current = (_current + 1) % target.Length;
			if(_zeroIndex == _current)
			{
				_canMove = false;
			}
		}
	}
}
