using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lab11: MonoBehaviour {

	//---------------- Lab #9 Additions ----------------
	[Header("Editable Attributes")]
	[Header("Target Attributes")]
	public float targetMass;
	public float targetVelocity;
	public float targetVelocityFinal;
	[Header("Gunball Attributes")]
	public float projMass;
	public float gunBallVelocity;
	public float gunBallVelocityFinal;
	[Tooltip("Restitution coefficient - determines if the objects stick together or bounce upon impact.")]
	public float e;
	

	//---------------- Lab #11 Additions ----------------
	[Space(10)]
	[Header("Calculated Attributes")]
	[Header("Important Points")]
	[Tooltip("Point in which the two objects collide - forces act upon this point")]
	public Vector3 P;
	[Tooltip("Point of the gunball at the collision")]
	public Vector3 R1;
	[Tooltip("Point of the target at the collision")]
	public Vector3 R2;

	[Space(10)]
	public Vector3 lowCaseR1;
	public Vector3 lowCaseR2;

	[Space(10)]
	public Vector3 omegaBallI;
	public Vector3 omegaTargetI, omegaBallF, omegaTargetF;

	[Space(10)]
	[Header("Moment of Inertia Values")]
	[Tooltip("Moment of inertia of the gunball")]
	public float moi1;
	[Tooltip("Moment of inertia of the target")]
	public float moi2;

	[Space(10)]
	[Header("L Values")]
	public Vector3 LInitial;
	public Vector3 LFinal1, LFinal2;
	public Vector3 LFinalTotal;

	//---------------- Lab #10 Additions ----------------
	[Space(10)]
	public Vector3 normals;
	public Vector3 tangentials;

	[Space(10)]
	public float jNormal;
	public float jImpulse;

	[Space(10)]
	[Header("Energy Values")]
	public Vector3 energyInitialX;
	public Vector3 energyInitialZ;
    public Vector3 energyFinalX, energyFinalZ, rotEnergy;
	public float totalEnergy;

	[Space(10)]
	[Header("Momentum Values")]
	public Vector3 momentumInitialX;
	public Vector3 momentumInitialZ;
    public Vector3 momentumFinalX, momentumFinalZ;

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

	[Header("Game Objects and Text")]
	public GameObject boat;
	public GameObject target;
	public GameObject gunball;
	public GameObject flightMarker;
	public Text updatesText;
	public Text timeText;
	public Text posText;
	public Text angleText;

	private float fixedTime;
	private float numUpdates;
	private float lastMarker, buffer;

	//program state booleans - whether the simulation is in motion, whether it is the initial start
	//and whether or not the two objects have collided yet
	private static bool moving, initial, collided;
	
	//whether or not the gunball has been spawned yet
	private bool gunBallSpawned;


	void Start () {
		numUpdates = 0;

		moving = false;
		gunBallSpawned = false;
		initial = true;
		collided = false;

		lastMarker = numUpdates;
		buffer = 3;
		fixedTime = Time.fixedDeltaTime;

		//---------------- Lab #11 Additions ----------------
		moi1 = PhysicsCalculator.calculateMOI(projMass);
		moi2 = PhysicsCalculator.calculateMOI(targetMass);
	}
	
	
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space)) {
			if (!gunBallSpawned) {
				gunBallSpawned = true;

				//spawn the ball
				gunball = Object.Instantiate(gunball, new Vector3(0, 0, 0), Quaternion.identity);
				moving = true;
			} else {
				moving = !moving;
			}
		}
	}

	void FixedUpdate() {

		if (moving && gunBallSpawned) {

			if (Mathf.Abs(gunball.transform.position.z - target.transform.position.z) < 1 && numUpdates > 0 && !collided) {
				pause();
				timeText.text = "Time: " + ((numUpdates + 1) * fixedTime) + " seconds";

			} else {

				if (!collided) {
					gunball.transform.Translate(gunball.transform.position.x, gunball.transform.position.y, gunBallVelocity * fixedTime);
					target.transform.Translate(0, 0, targetVelocity * fixedTime);
				} else {
					gunball.transform.position = new Vector3(gunball.transform.position.x + bulletXFinal * fixedTime, gunball.transform.position.y, gunball.transform.position.z + gunBallVelocityFinal * fixedTime);
					target.transform.position = new Vector3(target.transform.position.x + targetXFinal * fixedTime, target.transform.position.y, target.transform.position.z + targetZfinal * fixedTime);

					gunball.transform.Rotate(-omegaBallF * Mathf.Rad2Deg * fixedTime);
					target.transform.Rotate(-omegaTargetF * Mathf.Rad2Deg * fixedTime);
				}

				if (lastMarker + buffer < numUpdates && moving) {
					lastMarker = numUpdates;
					Object.Instantiate(flightMarker, new Vector3(gunball.transform.position.x, gunball.transform.position.y, gunball.transform.position.z), Quaternion.identity);
				}
			}

			
		}

		if (moving && gunBallSpawned) {
			numUpdates++;

			//update UI
			posText.text = "Masses (Ball/Target): " + projMass + "kg, " + targetMass + "kg";
			timeText.text = "Time: " + ((numUpdates + 1) * fixedTime) + " seconds";
			updatesText.text = "Updates: " + (numUpdates + 1) + " frames";
		}
	}

	public void pause() {
		moving = !moving;
		collided = true;

		R1.x = gunball.transform.position.x;
		R1.y = gunball.transform.position.y;
		R1.z = gunball.transform.position.z;

		R2.x = target.transform.position.x;
		R2.y = target.transform.position.y;
		R2.z = target.transform.position.z;

		P.z = target.transform.position.z - 0.5f;
		P.x = gunball.transform.position.x + (target.transform.position.x - gunball.transform.position.x) / 2;

		//reposition the cube
		gunball.transform.position = new Vector3(gunball.transform.position.x, gunball.transform.position.y, target.transform.position.z - 1);

		//Lab #10
		normals.x = target.transform.position.x - gunball.transform.position.x;
		normals.z = target.transform.position.z - gunball.transform.position.z;

		//lab #11 additions
		lowCaseR1 = PhysicsCalculator.calculateSubtractVector(P, gunball.transform.position);
		lowCaseR2 = PhysicsCalculator.calculateSubtractVector(P, target.transform.position);

		//hacky fix :)
		normals.x = 0;
		tangentials.z = normals.x;
		//--------------------------------

		jImpulse = PhysicsCalculator.calculateJImpulse((gunBallVelocity - targetVelocity), e, projMass, targetMass, normals, lowCaseR1, lowCaseR2, moi1, moi2);

		omegaBallF = -(omegaBallI + Vector3.Cross(lowCaseR1, (normals * jImpulse)) / moi1);
		omegaTargetF = omegaTargetI + Vector3.Cross(lowCaseR2, (normals * jImpulse)) / moi2;

		//Lab #10
		jNormal = (jImpulse * normals.z) + (0 * normals.x);

		//react to the collision
		gunBallVelocityFinal = PhysicsCalculator.calculateRecoilVelocity(jNormal, projMass, gunBallVelocity);
		targetVelocityFinal  = PhysicsCalculator.calculateRecoilVelocity(-jNormal, targetMass, targetVelocity);

		tangentials.z = normals.x * -1;
		tangentials.x = normals.z;
 
		bulletinitialNormal = (gunBallVelocity * normals.z) + (0 * normals.x);
		bulletinitialTang = (gunBallVelocity * tangentials.z) + (0 * tangentials.x);

		targetinitialNormal = (targetVelocity * normals.z) + (0 * normals.x);
		targetinitialTang = (targetVelocity * tangentials.z) + (0 * tangentials.z);

		//final normals
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

		//rotational energy
		rotEnergy.x = (0.5f * moi1 * omegaBallF.y * omegaBallF.y);
		rotEnergy.y = (0.5f * moi2 * omegaTargetF.y * omegaTargetF.y);
		rotEnergy.z = rotEnergy.x + rotEnergy.y;

		totalEnergy = energyFinalZ.z + (0.5f * moi1 * omegaBallF.y * omegaBallF.y) + (0.5f * moi2 * omegaTargetF.y * omegaTargetF.y);

		//gunball momentum values
		Vector3 gunballInitialMomentum = new Vector3(momentumInitialX.x, 0.0f, momentumInitialZ.x);
		//nothing happening in x, hence why it's 0
		Vector3 gunballFinalMomentum = new Vector3(0.0f, 0.0f, momentumFinalZ.x);

		//target momentum values
		Vector3 targetInitialMomentum = new Vector3(momentumInitialX.y, 0.0f, momentumInitialZ.y);
		//nothing happening in x, hence why it's 0
		Vector3 targetFinalMomentum = new Vector3(0.0f, 0.0f, momentumFinalZ.y);

		LInitial = PhysicsCalculator.calculateAngularMomentum(lowCaseR1, lowCaseR2, gunballInitialMomentum, targetInitialMomentum);

		LFinal1 = Vector3.Cross(lowCaseR1, gunballFinalMomentum) + (moi1 * omegaBallF);
		LFinal2 = Vector3.Cross(lowCaseR2, targetFinalMomentum) + (moi2 * omegaTargetF);

		LFinalTotal = LFinal1 + LFinal2;
	}
}
