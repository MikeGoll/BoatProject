using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lab4 : MonoBehaviour {

public GameObject boat, target, gunball, flightMarker;
	public float gunBallVelocity;
	public float range, xDifference, gamma;
	public float angle, angleAdjusted;
	public float newDx, newDy, newDz, newVx, newVy, newVz;
	public float currentDx, currentDy, oldDz, oldVx, oldVy, oldVz;
	// public Text initialVelText, updatesText, timeText, rangeText, angleText, gunBallText;

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
		xDifference = PhysicsCalculator.calculateXDifference(boat, target);

		//calculate the total time
		totalTime = PhysicsCalculator.calculateProjTime(range, gunBallVelocity, angle);

		if (xDifference != 0) {
			gamma = PhysicsCalculator.calculateGamma(xDifference, PhysicsCalculator.calculateRange(boat, target));

			angle = PhysicsCalculator.calculateAngle(ACCELERATION, xDifference, range, gunBallVelocity);
			angleAdjusted = (180 - PhysicsCalculator.toDegrees(angle)) / 2;

			angle = PhysicsCalculator.toDegrees(angle) / 2;
			// gamma = PhysicsCalculator.toDegrees(gamma);
			
			oldVz = PhysicsCalculator.calculateGammaVelocity(gunBallVelocity, Mathf.Cos(gamma), Mathf.Sin(angleAdjusted * Mathf.PI / 180));
			oldVy = PhysicsCalculator.calculateGammaYVelocity(gunBallVelocity, angleAdjusted);
			oldVx = PhysicsCalculator.calculateGammaVelocity(gunBallVelocity, Mathf.Sin(gamma), Mathf.Sin(angleAdjusted * Mathf.PI / 180));

			
		} else {
			//calculate theta
			angle = PhysicsCalculator.calculateTheta(range, gunBallVelocity);

			//change angle to degrees
			angle = angle * 180 / Mathf.PI;
			// angleText.text = "Angle: " + angle + " degrees";

			//calculate Vx
			oldVx = PhysicsCalculator.calculateXVelocity(gunBallVelocity, angle);
			//calculate Vy
			oldVy = PhysicsCalculator.calculateYVelocity(gunBallVelocity, angle);
		}		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space)) {
			if (!gunBallSpawned) {
				gunBallSpawned = true;
				//adjust the gun rotation

				//spawn the ball
				gunball = Object.Instantiate(gunball, new Vector3(0, 0, boat.transform.position.z), Quaternion.identity);
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

				if (xDifference == 0) {
					//rotates the cannon by the calculated angle
					boat.transform.rotation = Quaternion.Euler(-angle, 0, 0);
				} else {
					//gamma/alpha rotation of the cannon
					boat.transform.rotation = Quaternion.Euler(-angle, PhysicsCalculator.toDegrees(gamma), 0);
				}
			}

			

			if (xDifference == 0) {
				angle = PhysicsCalculator.calculateTheta(range, gunBallVelocity) * 180 / Mathf.PI;
			
				newVx = PhysicsCalculator.calculateVelocity(oldVx, 0, fixedTime);
				newVy = PhysicsCalculator.calculateVelocity(oldVy, ACCELERATION, fixedTime);

				//update positions accordingly
				newDx = PhysicsCalculator.calculateDistance(currentDx, 0, newVx, fixedTime);
				newDy = PhysicsCalculator.calculateDistance(currentDy, ACCELERATION, newVy, fixedTime);

				if (gunball.transform.position.y - target.transform.position.y <= 0.05 && numUpdates > 0) {
					moving = false;
					// timeText.text = "Time: " + totalTime + " seconds";
				} else {
					yDisplacement = newVy * fixedTime;
					zDisplacement = newVx * fixedTime;
					gunball.transform.position = new Vector3(gunball.transform.position.x, gunball.transform.position.y + yDisplacement, gunball.transform.position.z + zDisplacement);

					if (lastMarker + buffer < numUpdates) {
						lastMarker = numUpdates;
						Object.Instantiate(flightMarker, new Vector3(gunball.transform.position.x, gunball.transform.position.y + yDisplacement, gunball.transform.position.z + zDisplacement), Quaternion.identity);
					}
				}
			} else {
				if (gunball.transform.position.y - target.transform.position.y <= 0.05 && numUpdates > 0) {
					moving = false;
					// timeText.text = "Time: " + totalTime + " seconds";
					Debug.Log(numUpdates);
				} else {
					//run lab 4
					newVx = PhysicsCalculator.calculateVelocity(oldVx, 0, fixedTime);
					newVy = PhysicsCalculator.calculateYVelocityGamma(oldVy, -ACCELERATION, fixedTime);
					newVz = PhysicsCalculator.calculateVelocity(oldVz, 0, fixedTime);

					newDx = PhysicsCalculator.calculateXPosition(currentDx, oldVx, angleAdjusted * Mathf.PI / 180, gamma, fixedTime);
					newDy = PhysicsCalculator.calculateYPosition(currentDy, oldVy, -ACCELERATION, fixedTime);
					newDz = PhysicsCalculator.calculateZPosition(oldDz, oldVz, angleAdjusted * Mathf.PI / 180, PhysicsCalculator.toDegrees(gamma), fixedTime);

					gunball.transform.position = new Vector3(gunball.transform.position.x + newVx * fixedTime, gunball.transform.position.y + newVy * fixedTime, gunball.transform.position.z + newVz * fixedTime);
				}
			}
		}

		if (moving && gunBallSpawned) {
			//update UI
			// initialVelText.text = "Init. Velocity: " + gunBallVelocity + " m/s";
			// rangeText.text = "Range: " + range + " m";
			// timeText.text = "Time: " + (Time.time - timeBeg) + " seconds";
			// updatesText.text = "Updates: " + (numUpdates + 1) + " frames";
			// gunBallText.text = "Gunball Z: " + gunball.transform.position.z + " m";

			numUpdates++;
			oldVx = newVx;
			oldVy = newVy;
			oldVz = newVz;

			currentDx = newDx;
			currentDy = newDy;
			oldDz = newDz;
		}
	}
}
