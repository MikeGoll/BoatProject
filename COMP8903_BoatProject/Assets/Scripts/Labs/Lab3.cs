using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lab3 : MonoBehaviour {

	
	[Header("Editable Attributes")]
	public float gunBallVelocity;

	[Header("Calculated Attributes")]
	public float range;
	public float angle;
	public float newDx, newDy, currentDx, currentDy, newVx, newVy, oldVx, oldVy;

	[Header("Object References")]
	public GameObject boat;
	public GameObject target, gunball, flightMarker;
	public Text initialVelText, updatesText, timeText, rangeText, angleText, gunBallText;

	private bool moving, initial;
	private float totalTime, fixedTime;
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
		fixedTime = Time.fixedDeltaTime;

		//calculate theta
		angle = PhysicsCalculator.calculateTheta(range, gunBallVelocity);

		//calculate the total time
		totalTime = PhysicsCalculator.calculateProjTime(range, gunBallVelocity, angle);

		//change angle to degrees
		angle = angle * 180 / Mathf.PI;
		angleText.text = "Angle: " + angle + " degrees";

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
				//rotates the cannon by the calculated angle
				boat.transform.rotation = Quaternion.Euler(-angle, 0, 0);
			}

			angle = PhysicsCalculator.calculateTheta(range, gunBallVelocity) * 180 / Mathf.PI;
			
			newVx = PhysicsCalculator.calculateVelocity(oldVx, 0, fixedTime);
			newVy = PhysicsCalculator.calculateVelocity(oldVy, ACCELERATION, fixedTime);

			//update positions accordingly
			newDx = PhysicsCalculator.calculateDistance(currentDx, 0, newVx, fixedTime);
			newDy = PhysicsCalculator.calculateDistance(currentDy, ACCELERATION, newVy, fixedTime);

			if (gunball.transform.position.y - target.transform.position.y <= 0.05 && numUpdates > 0) {
				moving = false;
				timeText.text = "Time: " + totalTime + " seconds";
			} else {
				yDisplacement = newVy * fixedTime;
				zDisplacement = newVx * fixedTime;
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
			gunBallText.text = "Gunball Z: " + gunball.transform.position.z + " m";

			numUpdates++;
			oldVx = newVx;
			oldVy = newVy;
			currentDx = newDx;
			currentDy = newDy;
		}
	}
}
