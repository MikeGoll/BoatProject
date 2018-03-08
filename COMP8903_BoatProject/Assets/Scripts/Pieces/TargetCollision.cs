using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCollision : MonoBehaviour {

	private Vector3 ballPos;
	private Lab9 lab9;
	private bool initial;

	// Use this for initialization
	void Start () {
		lab9 = GameObject.FindObjectOfType(typeof(Lab9)) as Lab9;
		Debug.Log(lab9);
		initial = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other)
	{
		Debug.Log("HIT");
		if (initial) {
			initial = false;

			ballPos = other.gameObject.transform.position;
			this.GetComponent<Rigidbody>().detectCollisions = false;

			if (Mathf.Abs(ballPos.z - transform.position.z) > 0) {
				other.gameObject.transform.position = new Vector3(ballPos.x, ballPos.y, Mathf.Floor(ballPos.z));
			}

			lab9.pause();
		}
	}
}
