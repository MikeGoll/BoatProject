using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lab5 : MonoBehaviour {

	// Use this for initialization
	public GameObject boat, target, gunball, flightMarker;
	public float gunBallVelocity;
	public float omegaI, omegaF, alphaI, theta, angularDisplacement, totalAngleDisplacement;
	public float range, xDifference;
	public Text updatesText, timeText, gammaText, angleText;

	private bool moving, initial;
	private float fixedTime;
	private float numUpdates;
	private float lastMarker, buffer;
	private bool gunBallSpawned;
	private float yDisplacement, zDisplacement;
	private float newDx, newDy, newDz, newVx, newVy, newVz;
	private float currentDx, currentDy, oldDz, oldVx, oldVy, oldVz;
	private float angle, angleAdjusted;
	private float gamma, gammaDegrees;

	private GameObject gunballPoint;

	private const float ACCELERATION = -9.81f;

	// Use this for initialization
	void Start () {
		numUpdates = 0;
		range = PhysicsCalculator.calculateRange(boat, target);
		moving = true;
		gunBallSpawned = false;
		initial = true;
		lastMarker = numUpdates;
		buffer = 0;
		fixedTime = Time.fixedDeltaTime;
		xDifference = PhysicsCalculator.calculateXDifference(boat, target);

		gunballPoint = gunball.transform.GetChild(0).gameObject;

		if (xDifference != 0) {
			gamma = PhysicsCalculator.calculateGamma(xDifference, PhysicsCalculator.calculateRange(boat, target));
			gammaDegrees = PhysicsCalculator.toDegrees(gamma);

			angle = PhysicsCalculator.calculateAngle(ACCELERATION, xDifference, range, gunBallVelocity);
			angleAdjusted = (180 - PhysicsCalculator.toDegrees(angle)) / 2;

			angle = PhysicsCalculator.toDegrees(angle) / 2;
			
			oldVz = PhysicsCalculator.calculateGammaVelocity(gunBallVelocity, Mathf.Cos(gamma), Mathf.Sin(angleAdjusted * Mathf.PI / 180));
			oldVy = PhysicsCalculator.calculateGammaYVelocity(gunBallVelocity, angleAdjusted);
			oldVx = PhysicsCalculator.calculateGammaVelocity(gunBallVelocity, Mathf.Sin(gamma), Mathf.Sin(angleAdjusted * Mathf.PI / 180));
			
		} else {
			//calculate theta
			angle = PhysicsCalculator.calculateTheta(range, gunBallVelocity);

			//change angle to degrees
			angle = angle * 180 / Mathf.PI;

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
					timeText.text = "Time: " + ((numUpdates + 1) * fixedTime) + " seconds";
				} else {
					yDisplacement = newVy * fixedTime;
					zDisplacement = newVx * fixedTime;

					if (target.transform.position.z < boat.transform.position.z) {
						gunball.transform.position = new Vector3(gunball.transform.position.x, gunball.transform.position.y + yDisplacement, gunball.transform.position.z - zDisplacement);
					} else {
						gunball.transform.position = new Vector3(gunball.transform.position.x, gunball.transform.position.y + yDisplacement, gunball.transform.position.z + zDisplacement);
					}

					// if (lastMarker + buffer < numUpdates) {
					// 	lastMarker = numUpdates;
					// 	Object.Instantiate(flightMarker, new Vector3(gunball.transform.position.x, gunball.transform.position.y + yDisplacement, gunball.transform.position.z + zDisplacement), Quaternion.identity);
					// }
				}
			} else {
				if (gunball.transform.position.y - target.transform.position.y <= 0.05 && numUpdates > 0) {
					moving = false;
					timeText.text = "Time: " + ((numUpdates + 1) * fixedTime) + " seconds";
				} else {
					//run lab 4
					newVx = PhysicsCalculator.calculateVelocity(oldVx, 0, fixedTime);
					newVy = PhysicsCalculator.calculateYVelocityGamma(oldVy, -ACCELERATION, fixedTime);
					newVz = PhysicsCalculator.calculateVelocity(oldVz, 0, fixedTime);

					newDx = PhysicsCalculator.calculateXPosition(currentDx, oldVx, angle, gamma, fixedTime);
					newDy = PhysicsCalculator.calculateYPosition(currentDy, oldVy, -ACCELERATION, fixedTime);
					newDz = PhysicsCalculator.calculateZPosition(oldDz, oldVz, angle, PhysicsCalculator.toDegrees(gamma), fixedTime);

					if (target.transform.position.x < boat.transform.position.x && target.transform.position.z < boat.transform.position.z) {
						 gunball.transform.position = new Vector3(gunball.transform.position.x - newVx * fixedTime, gunball.transform.position.y + newVy * fixedTime, gunball.transform.position.z - newVz * fixedTime);
					} else if (target.transform.position.x < boat.transform.position.x) {
						gunball.transform.position = new Vector3(gunball.transform.position.x - newVx * fixedTime, gunball.transform.position.y + newVy * fixedTime, gunball.transform.position.z + newVz * fixedTime);
					} else if (target.transform.position.z < boat.transform.position.z) {
						gunball.transform.position = new Vector3(gunball.transform.position.x + newVx * fixedTime, gunball.transform.position.y + newVy * fixedTime, gunball.transform.position.z - newVz * fixedTime);
					} else {
						gunball.transform.position = new Vector3(gunball.transform.position.x + newVx * fixedTime, gunball.transform.position.y + newVy * fixedTime, gunball.transform.position.z + newVz * fixedTime);
					}

					// if (lastMarker + buffer < numUpdates) {
					// 	lastMarker = numUpdates;
					// 	Object.Instantiate(flightMarker, new Vector3(gunball.transform.position.x, gunball.transform.position.y + yDisplacement, gunball.transform.position.z + zDisplacement), Quaternion.identity);
					// }
				}
			}

			omegaF = PhysicsCalculator.calculateOmegaFinal(omegaI, alphaI, (numUpdates + 1) * fixedTime);
			theta = PhysicsCalculator.calculateThetaFinal(omegaI, alphaI, (numUpdates + 1) * fixedTime);
			angularDisplacement = PhysicsCalculator.calculateAngularDisplacement(theta, omegaF, alphaI, fixedTime);
			totalAngleDisplacement += angularDisplacement;

			//rotate the ball
			gunball.transform.Rotate(new Vector3(-PhysicsCalculator.toDegrees(angularDisplacement), 0, 0));
		}

		if (moving && gunBallSpawned) {
			numUpdates++;

			//update UI
			gammaText.text = "Angular Velocity: " + omegaF;
			angleText.text = "Acceleration: " + theta;
			timeText.text = "Time: " + ((numUpdates + 1) * fixedTime) + " seconds";
			updatesText.text = "Updates: " + (numUpdates + 1) + " frames";
			
			
			oldVx = newVx;
			oldVy = newVy;
			oldVz = newVz;

			currentDx = newDx;
			currentDy = newDy;
			oldDz = newDz;

		}
	}
}
