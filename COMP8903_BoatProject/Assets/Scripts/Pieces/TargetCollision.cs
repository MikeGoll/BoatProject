using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCollision : MonoBehaviour {

	private Vector3 ballPos;
	private Lab9 lab9;
	private Lab10 lab10;
	private bool initial, l9, l10;

	// Use this for initialization
	void Start () {
		lab9 = GameObject.FindObjectOfType(typeof(Lab9)) as Lab9;
		lab10 = GameObject.FindObjectOfType(typeof(Lab10)) as Lab10;

		l9 = true;
		l10 = false;

		initial = true;
	}

	void OnTriggerEnter(Collider other)
	{
		if (initial) {
			initial = false;

			ballPos = other.gameObject.transform.position;
			this.GetComponent<Rigidbody>().detectCollisions = false;

			if (l9) {
				if (Mathf.Abs(ballPos.z - transform.position.z) > 0) {
					other.gameObject.transform.position = new Vector3(ballPos.x, ballPos.y, transform.position.z - 1);
				}

				lab9.pause();
			} else if (l10) {
				if (Mathf.Abs(ballPos.z - transform.position.z) < 1) {
					float z = Mathf.Sqrt(1 - Mathf.Pow(ballPos.x, 2));
					float x = transform.position.x - z;
					
					other.gameObject.transform.position = new Vector3(x, ballPos.y, z);
				}

				lab10.pause();
			}
			
		}
	}
}
