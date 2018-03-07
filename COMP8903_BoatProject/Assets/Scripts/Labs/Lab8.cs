using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lab8 : MonoBehaviour {

	//---------------- Lab #8 Additions ----------------
	public float projMass, dragCoefficient, tau, windVelocity, windCoefficient;

	public GameObject boat, target, gunball, flightMarker;
	public float gunBallVelocity;
	public float range, xDifference, gamma, gammaDegrees;
	public float angle, angleAdjusted;
	public float newDx, newDy, newDz, newVx, newVy, newVz;
	public float currentDx, currentDy, oldDz, oldVx, oldVy, oldVz;
	public Text updatesText, timeText, gammaText, angleText;

	private bool moving, initial;
	private float fixedTime;
	private float numUpdates;
	private float lastMarker, buffer;
	private bool gunBallSpawned;
	private float yDisplacement, zDisplacement;

	private const float ACCELERATION = -9.81f;

	// Use this for initialization
	void Start () {
		numUpdates = 0;
		moving = true;
		gunBallSpawned = false;
		initial = true;
		lastMarker = numUpdates;
		buffer = 3;
		fixedTime = Time.fixedDeltaTime;

		range = PhysicsCalculator.calculateRange(boat, target);
		xDifference = PhysicsCalculator.calculateXDifference(boat, target);

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
			angle = PhysicsCalculator.calculateAngle(ACCELERATION, xDifference, range, gunBallVelocity);

			//change angle to degrees
			angle = PhysicsCalculator.toDegrees(angle) / 2;

			//calculate Vx
			oldVx = PhysicsCalculator.calculateXVelocity(gunBallVelocity, angle);
			//calculate Vy
			oldVy = PhysicsCalculator.calculateYVelocity(gunBallVelocity, angle);
		}


		//---------------- Lab #8 Additions ----------------
		tau = PhysicsCalculator.calculateTau(1, dragCoefficient);
		projMass = 1.0f;
		windVelocity = 2.0f;
		windCoefficient = 0.1f;
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

			angle = PhysicsCalculator.calculateTheta(range, gunBallVelocity) * 180 / Mathf.PI;
		
			newVx = PhysicsCalculator.calculateXVelocityWithWind(fixedTime, tau, oldVx, windCoefficient, windVelocity, gamma, dragCoefficient);
			newVy = PhysicsCalculator.calculateYVelocityWithWind(dragCoefficient, fixedTime, projMass, oldVy);
			newVz = PhysicsCalculator.calculateZVelocityWithWind(fixedTime, tau, oldVz, windCoefficient, windVelocity, gamma, dragCoefficient);


			newDx = PhysicsCalculator.calculateXPositionWithWind(currentDx, oldVx, tau, fixedTime, windCoefficient, windVelocity, dragCoefficient, gamma);
			newDy = PhysicsCalculator.calculateYPositionWithWind(currentDy, oldVy, tau, fixedTime);
			newDz = PhysicsCalculator.calculateZPositionWithWind(oldDz, oldVz, tau, fixedTime, windVelocity, gamma, windCoefficient, dragCoefficient);


			if (gunball.transform.position.y - target.transform.position.y <= 0.05 && numUpdates > 0) {
				moving = false;
				timeText.text = "Time: " + ((numUpdates + 1) * fixedTime) + " seconds";
			} else {

				if (xDifference == 0) {
					if (target.transform.position.z > boat.transform.position.z) {
						gunball.transform.Translate(gunball.transform.position.x, newVy * fixedTime, newVx * fixedTime);
					} else {
						gunball.transform.Translate(gunball.transform.position.x, newVy * fixedTime, -newVx * fixedTime);
					}
					
				} else { 
					if (target.transform.position.x < boat.transform.position.x && target.transform.position.z < boat.transform.position.z) {
						gunball.transform.Translate(-newVx * fixedTime, newVy * fixedTime, -newVz * fixedTime);
					} else if (target.transform.position.x < boat.transform.position.x) {
						gunball.transform.Translate(-newVx * fixedTime, newVy * fixedTime, newVz * fixedTime);
					} else if (target.transform.position.z < boat.transform.position.z) {
						gunball.transform.Translate(newVx * fixedTime, newVy * fixedTime, -newVz * fixedTime);
					} else {
						gunball.transform.Translate(newVx * fixedTime, newVy * fixedTime, newVz * fixedTime);
					}
				}

				if (lastMarker + buffer < numUpdates) {
					lastMarker = numUpdates;
					Object.Instantiate(flightMarker, new Vector3(gunball.transform.position.x, gunball.transform.position.y + yDisplacement, gunball.transform.position.z + zDisplacement), Quaternion.identity);
				}
			}
		}

		if (moving && gunBallSpawned) {
			numUpdates++;

			//update UI
			gammaText.text = "Gamma: " + gamma;
			angleText.text = "Alpha: " + angle;
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
