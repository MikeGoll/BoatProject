using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataControllerLab3 : MonoBehaviour {

	
	public GameObject boat, target, gunball, flightMarker;
	public float gunBallVelocity;
	public float range;
	public float angle;
	public float newDx, newDy, currentDx, currentDy, newVx, newVy, oldVx, oldVy;
	public Text initialVelText, updatesText, timeText, rangeText;

	private bool moving, initial;
	private float totalTime, t;
	private float numUpdates;
	private float lastMarker, buffer;
	private bool gunBallSpawned;
	private float yDisplacement, zDisplacement;
	private float timeBeg;

	private const float ACCELERATION = -9.81f;

	// Use this for initialization
	void Start () {
		numUpdates = 0;
		range = PhysicsCalculator.calculateRange(boat, target);
		moving = true;
		gunBallSpawned = false;
		initial = true;
		lastMarker = numUpdates;
		buffer = 3;

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

			if (initial) {
				initial = false;
				timeBeg = Time.time;
			}

			//calculate what frame it is
			t = numUpdates * Time.fixedDeltaTime;

			angle = PhysicsCalculator.calculateTheta(range, gunBallVelocity) * 180 / Mathf.PI;
			
			newVx = PhysicsCalculator.calculateVelocity(oldVx, 0, Time.fixedDeltaTime);
			newVy = PhysicsCalculator.calculateVelocity(oldVy, ACCELERATION, Time.fixedDeltaTime);

			//update positions accordingly
			newDx = PhysicsCalculator.calculateDistance(currentDx, 0, newVx, Time.fixedDeltaTime);
			newDy = PhysicsCalculator.calculateDistance(currentDy, ACCELERATION, newVy, Time.fixedDeltaTime);

			if (gunball.transform.position.y - target.transform.position.y <= 0.05 && numUpdates > 0) {
				moving = false;
				timeText.text = "Time: " + totalTime + " seconds";
			} else {
				yDisplacement = newVy * Time.fixedDeltaTime;
				zDisplacement = newVx * Time.fixedDeltaTime;
				gunball.transform.position = new Vector3(gunball.transform.position.x, gunball.transform.position.y + yDisplacement, gunball.transform.position.z + zDisplacement);

				if (lastMarker + buffer < numUpdates) {
					lastMarker = numUpdates;
					Object.Instantiate(flightMarker, new Vector3(gunball.transform.position.x, gunball.transform.position.y + yDisplacement, gunball.transform.position.z + zDisplacement), Quaternion.identity);
				}
			}
		}

		if (moving && gunBallSpawned) {
			//update UI
			initialVelText.text = "Init. Velocity: " + gunBallVelocity + " m/s";
			rangeText.text = "Range: " + range + " m";
			timeText.text = "Time: " + (Time.time - timeBeg) + " seconds";
			updatesText.text = "Updates: " + (numUpdates + 1) + " frames";

			numUpdates++;
			oldVx = newVx;
			oldVy = newVy;
			currentDx = newDx;
			currentDy = newDy;
		}
	}
}
