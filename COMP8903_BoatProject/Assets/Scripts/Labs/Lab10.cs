using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lab10: MonoBehaviour {

	//---------------- Lab #10 Additions ----------------
	public Vector3 normals, tangentials;
	public float jNormal;

	[Space(10)]
	public Vector3 energyInitialX;
	public Vector3 energyInitialZ;
    public Vector3 energyFinalX, energyFinalZ;
	public float totalEnergy;
	[Space(10)]
	public Vector3 momentumInitialX;
	public Vector3 momentumInitialZ;
    public Vector3 momentumFinalX, momentumFinalZ;
	[Space(10)]
	public float targetVelocityFinal;

	[Space(10)]
	public float bulletinitialNormal;
	public float bulletinitialTang;
	public float targetinitialNormal, targetinitialTang;

	//Z
	[Space(10)]
	public float bulletZfinalNormal;
	public float bulletZfinalTang;
	public float targetZfinalNormal, targetZfinalTang;

	//FINALS
	[Space(10)]
	public float bulletZfinal;
	public float targetZfinal;
	public float bulletXFinal;
	public float targetXFinal;

	//X
	[Space(10)]
	public float bulletXfinalNormal;
	public float bulletXfinalTang;
	public float targetXfinalNormal, targetXfinalTang;

	private bool collided;


	//---------------- Lab #9 Additions ----------------
	[Header("Target Attributes")]
	public float targetMass;
	public float targetVelocity;
	[Header("Gunball Attributes")]
	public float projMass;
	public float gunBallVelocity;
	public float gunBallVelocityFinal;

	[Space(10)]
	[Tooltip("Restitution coefficient - determines if the objects stick together or bounce upon impact.")]
	public float e;
	public float jImpulse;
	public Vector3 momentumInitial;
	public Vector3 momentumFinal;


	//---------------- Lab #8 Additions ----------------
	[Header("Lab 8 Attributes - Editable")]
	public float dragCoefficient;
	
	[Header("Lab 8 Attributes")]
	public float tau;
	public float windVelocity;
	public float windCoefficient;

	[Header("Projectile Calculations")]
	public float gamma;
	public float gammaDegrees;
	public float angle;
	public float angleAdjusted;

	[Header("Distance Calculations")]
	public float newDx;
	public float newDy;
	public float newDz;

	[Header("Velocity Calculations")]
	public float newVx;
	public float newVy;
	public float newVz;
	
	[Header("Game Objects and Text")]
	public GameObject boat;
	public GameObject target;
	public GameObject gunball;
	public GameObject flightMarker;
	public Text updatesText;
	public Text timeText;
	public Text posText;
	public Text angleText;

	private float range, xDifference;
	private float currentDx, currentDy, oldDz, oldVx, oldVy, oldVz;
	private float fixedTime;
	private float numUpdates;
	private float lastMarker, buffer;
	private float yDisplacement, zDisplacement;

	private static bool moving, initial;
	private bool gunBallSpawned;

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

		oldVz = gunBallVelocity;

		range = PhysicsCalculator.calculateRange(boat, target);
		xDifference = PhysicsCalculator.calculateXDifference(boat, target);

		collided = false;

		if (xDifference != 0) {

			// gamma = PhysicsCalculator.calculateGamma(xDifference, PhysicsCalculator.calculateRange(boat, target));
			// gammaDegrees = PhysicsCalculator.toDegrees(gamma);

			// angle = PhysicsCalculator.calculateAngle(ACCELERATION, xDifference, range, gunBallVelocity);
			// angleAdjusted = (180 - PhysicsCalculator.toDegrees(angle)) / 2;

			// angle = PhysicsCalculator.toDegrees(angle) / 2;
			
			// oldVz = PhysicsCalculator.calculateGammaVelocity(gunBallVelocity, Mathf.Cos(gamma), Mathf.Sin(angleAdjusted * Mathf.PI / 180));
			// oldVy = PhysicsCalculator.calculateGammaYVelocity(gunBallVelocity, angleAdjusted);
			// oldVx = PhysicsCalculator.calculateGammaVelocity(gunBallVelocity, Mathf.Sin(gamma), Mathf.Sin(angleAdjusted * Mathf.PI / 180));
			
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
		tau = PhysicsCalculator.calculateTau(projMass, dragCoefficient);
		windVelocity = 2.0f;
		windCoefficient = 0.1f;

		//---------------- Lab #9 Additions ----------------
		jImpulse = PhysicsCalculator.calculateJImpulse((gunBallVelocity - targetVelocity), e, projMass, targetMass);
		momentumInitial.x = PhysicsCalculator.calculateMomentum(projMass, gunBallVelocity);
		momentumInitial.y = PhysicsCalculator.calculateMomentum(targetMass, targetVelocity);
		momentumInitial.z = momentumInitial.x + momentumInitial.y;

		//have this here to satisfy requirement of needing to show initial target velocity
		Debug.Log("Target Initial Velocity: " + targetVelocity);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space)) {
			if (!gunBallSpawned) {
				gunBallSpawned = true;
				//adjust the gun rotation

				//spawn the ball
				gunball = Object.Instantiate(gunball, new Vector3(0, 0, boat.transform.position.z), Quaternion.identity);
				moving = true;
			} else {
				moving = !moving;
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

			//run lab 4
			// newVx = PhysicsCalculator.calculateVelocity(oldVx, 0, fixedTime);
			// newVy = PhysicsCalculator.calculateYVelocityGamma(oldVy, -ACCELERATION, fixedTime);
			newVz = PhysicsCalculator.calculateVelocity(oldVz, 0, fixedTime);

			// newDx = PhysicsCalculator.calculateXPosition(currentDx, oldVx, angle, gamma, fixedTime);
			// newDy = PhysicsCalculator.calculateYPosition(currentDy, oldVy, -ACCELERATION, fixedTime);
			// newDz = PhysicsCalculator.calculateZPosition(oldDz, oldVz, angle, PhysicsCalculator.toDegrees(gamma), fixedTime);

			if (Mathf.Abs(gunball.transform.position.z - target.transform.position.z) < 1 && numUpdates > 0 && !collided) {
				pause();
				timeText.text = "Time: " + ((numUpdates + 1) * fixedTime) + " seconds";
			} else {

				if (xDifference == 0) {
					gunball.transform.Translate(gunball.transform.position.x, gunball.transform.position.y, newVx * fixedTime);
				} else {

					// //handles any quadrant case
					// if (target.transform.position.x < boat.transform.position.x && target.transform.position.z < boat.transform.position.z) {
					// 	gunball.transform.Translate(-newVx * fixedTime, newVy * fixedTime, -newVz * fixedTime);
					// } else if (target.transform.position.x < boat.transform.position.x) {
					// 	gunball.transform.Translate(-newVx * fixedTime, newVy * fixedTime, newVz * fixedTime);
					// } else if (target.transform.position.z < boat.transform.position.z) {
					// 	gunball.transform.Translate(newVx * fixedTime, newVy * fixedTime, -newVz * fixedTime);
					// } else {
					// 	gunball.transform.Translate(newVx * fixedTime, newVy * fixedTime, newVz * fixedTime);
					// }

					if (!collided) {
						gunball.transform.Translate(gunball.transform.position.x, gunball.transform.position.y, gunBallVelocity * fixedTime);
						target.transform.Translate(0, 0, targetVelocity * fixedTime);
					} else {
						gunball.transform.Translate(bulletXFinal * fixedTime, gunball.transform.position.y, bulletZfinal * fixedTime);
						target.transform.Translate(targetXFinal * fixedTime, target.transform.position.y, targetZfinal * fixedTime);
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
			posText.text = "Masses (Ball/Target): " + projMass + "kg, " + targetMass + "kg";
			timeText.text = "Time: " + ((numUpdates + 1) * fixedTime) + " seconds";
			updatesText.text = "Updates: " + (numUpdates + 1) + " frames";
			
			oldVx = newVx;
			oldVz = newVz;

			currentDx = newDx;
			currentDy = newDy;
			oldDz = newDz;
		}
	}

	public void pause() {
		moving = !moving;
		collided = true;

		float velR = gunBallVelocity - targetVelocity;
		jImpulse = -velR * (e + 1) * projMass * targetMass / (projMass + targetMass);

		gunBallVelocityFinal = jImpulse / projMass + gunBallVelocity;
		targetVelocityFinal = -jImpulse / targetMass + targetVelocity;

		//react to the collision
		// oldVx = PhysicsCalculator.calculateRecoilVelocity(jImpulse, projMass, gunBallVelocity);
		// gunBallVelocityFinal = oldVx;

		// targetVelocity = PhysicsCalculator.calculateRecoilVelocity(-jImpulse, targetMass, targetVelocity);

		//Lab #10
		normals.x = target.transform.position.x - gunball.transform.position.x;
		normals.z = target.transform.position.z - gunball.transform.position.z;

		jNormal = (jImpulse * normals.z) + (0 * normals.x);

		tangentials.z = normals.x * -1;
		tangentials.x = normals.z;

		bulletinitialNormal = (gunBallVelocity * normals.z) + (0 * normals.x);
		bulletinitialTang = (gunBallVelocity * tangentials.z) + (0 * tangentials.x);

		targetinitialNormal = (targetVelocity * normals.z) + (0 * normals.x);
		targetinitialTang = (targetVelocity * tangentials.z) + (0 * tangentials.z);

		bulletZfinalNormal = (jNormal / projMass + bulletinitialNormal) * normals.z;
		bulletZfinalTang = (bulletinitialTang * tangentials.z);

		targetZfinalNormal = ((-jNormal) / targetMass + targetinitialNormal) * normals.z;
		targetZfinalTang = (targetinitialTang * tangentials.z);

		//z velocities
		bulletZfinal = bulletZfinalNormal + bulletZfinalTang;
		targetZfinal = targetZfinalNormal + targetZfinalTang;

		bulletXfinalNormal = (jNormal / projMass + bulletinitialNormal) * normals.x;
		bulletXfinalTang = (bulletinitialTang * tangentials.x);

		targetXfinalNormal = ((-jNormal) / targetMass + targetinitialNormal) * normals.x;
		targetXfinalTang = targetinitialTang * tangentials.x;

		//x velocities
		bulletXFinal = bulletXfinalNormal + bulletXfinalTang;
		targetXFinal = targetXfinalNormal + targetXfinalTang;

		//bullet, target, final
		//momentum
		momentumInitialZ.x = projMass * gunBallVelocity;
		momentumInitialZ.y = targetMass * targetVelocity;
		momentumInitialZ.z = momentumInitialZ.x + momentumInitialZ.y;

		momentumFinalZ.x = projMass * bulletZfinal;
		momentumFinalZ.y = targetMass * targetZfinal;
		momentumFinalZ.z = momentumFinalZ.x + momentumFinalZ.y;

		momentumInitialX.x = 0f;
		momentumInitialX.y = 0f;
		momentumInitialX.z = momentumInitialX.x + momentumInitialX.y;

		momentumFinalX.x = projMass * bulletXFinal;
		momentumFinalX.y = targetMass * targetXFinal;
		momentumFinalX.z = momentumFinalX.x + momentumFinalX.y;

		//energy
		energyInitialZ.x = 0.5f * projMass * gunBallVelocity * gunBallVelocity;
		energyInitialZ.y = 0.5f * targetMass * targetVelocity * targetVelocity;
		energyInitialZ.z = energyInitialZ.x + energyInitialZ.y;

		energyFinalZ.x = 0.5f * projMass * bulletZfinal * bulletZfinal;
		energyFinalZ.y = 0.5f * targetMass * targetZfinal * targetZfinal;
		energyFinalZ.z = energyFinalZ.x + energyFinalZ.y;

		energyInitialX.x = 0.5f * projMass * 0.0f * 0.0f;
		energyInitialX.y = 0.5f * targetMass * 0.0f * 0.0f;
		energyInitialX.z = energyInitialX.x + energyInitialX.y;

		energyFinalX.x = 0.5f * projMass * bulletXFinal * bulletXFinal;
		energyFinalX.y = 0.5f * targetMass * targetXFinal * targetXFinal;
		energyFinalX.z = energyFinalX.x + energyFinalX.y;

		totalEnergy = energyFinalX.z + energyFinalZ.z;

	}
}
