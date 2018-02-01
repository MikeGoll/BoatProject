using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataControllerLab3 : MonoBehaviour {

	
	public GameObject boat, target, gunball;
	public float gunBallVelocity;
	public float range;
	public float angle;

	private bool moving;
	public float totalTime, t;
	public float newDx, newDy, currentDx, currentDy, newVx, newVy, oldVx, oldVy;
	private float numUpdates;
	private bool gunBallSpawned;

	private const float ACCELERATION = -9.81f;

	// Use this for initialization
	void Start () {
		numUpdates = 0;
		range = PhysicsCalculator.calculateRange(boat, target);
		moving = true;
		gunBallSpawned = false;

		//calculate theta
		angle = PhysicsCalculator.calculateTheta(range, gunBallVelocity);
		//calculate the total time
		totalTime = PhysicsCalculator.calculateProjTime(range, gunBallVelocity, angle);
		//change angle to radians
		angle = angle * 180 / Mathf.PI;

		//calculate Vx
		oldVx = PhysicsCalculator.calculateXVelocity(gunBallVelocity, angle);
		//calculate Vy
		oldVy = PhysicsCalculator.calculateYVelocity(gunBallVelocity, angle);
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space)) {
			if (!gunBallSpawned) {
				gunBallSpawned = true;
				//adjust the gun rotation

				//spawn the ball
				gunball = Object.Instantiate(gunball, new Vector3(0, 0, -4), Quaternion.identity);
			} else {
				Debug.Log("Ball already spawned");
			}
		}
	}

	void FixedUpdate() {
		if (moving && gunBallSpawned) {
			//calculate what frame it is
			t = numUpdates * Time.fixedDeltaTime;

			angle = PhysicsCalculator.calculateTheta(range, gunBallVelocity) * 180 / Mathf.PI;
			
			newVx = PhysicsCalculator.calculateVelocity(oldVx, 0, Time.fixedDeltaTime);
			newVy = PhysicsCalculator.calculateVelocity(oldVy, ACCELERATION, Time.fixedDeltaTime);

			//update positions accordingly
			newDx = PhysicsCalculator.calculateDistance(currentDx, 0, newVx, Time.fixedDeltaTime);
			newDy = PhysicsCalculator.calculateDistance(currentDy, ACCELERATION, newVy, Time.fixedDeltaTime);

			Debug.Log("DisplacementX: " + newVx * Time.fixedDeltaTime);
			Debug.Log("DisplacementY: " + newVy * Time.fixedDeltaTime);
			

			//if gunball location < 0.05 to target
			//moving = false;

			if (gunball.transform.position.y - target.transform.position.y <= 0.05 && numUpdates > 0) {
				moving = false;
				Debug.Log("Delta: " + t);
			} else {
				gunball.transform.position = new Vector3(gunball.transform.position.x, gunball.transform.position.y + newVy * Time.fixedDeltaTime, gunball.transform.position.z + newVx * Time.fixedDeltaTime);
			}
		}

		if (moving && gunBallSpawned) {
			//update UI
			numUpdates++;
			oldVx = newVx;
			oldVy = newVy;
			currentDx = newDx;
			currentDy = newDy;
		}
	}
}
