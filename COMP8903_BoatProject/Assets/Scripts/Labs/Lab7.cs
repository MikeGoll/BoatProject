using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lab7 : MonoBehaviour {

	//---------------------- LAB #7 Additions ----------------------
	public bool dynamicControls;
	public float dragCoefficient;
	public float tau;
	public float vMax;
	public float accelerationWithDrag;

	public float testVel;

	//---------------------- LAB #6 Additions ----------------------
	public GameObject totalBoat;

	//User entered data through UI (force, angle of force, final displacement z, left angular displacement, right angular displacement)
	public float force, angularForce, angle, dinZ, lD, rD;

	//thrust in X and Z axis
	public Vector3 thrust, thrustAngle;
	public Vector3 rLeft, rRight, torqueL, torqueR;
	public Vector3 acceleration;

	public float accelerationL, accelerationR;

	//acceleration, initial velocity, final velocity, new intermediate velocity, old intermediate velocity
	public float v_initial, v_final, newVx, oldVx, newVz, oldVz;

	//total distance, new and old distance
	public float distance, newDx, oldDx, newDz, oldDz;
	public Text timeText, posText, angDText, disText;
	
	private bool moving, initial, rotLeft, rotRight;
	private float numUpdates;
	private float fixedTime;
	private float angularVelocity, oldAngularVelocity, angularDisplacement, oldAngularDisplacement;
	private float distanceZ, distanceX;
	private float temp1;

	//---------------------- LAB #1 Originals ----------------------
	//GameObject references for the boat, pilot and cannon
	public GameObject b, p, c;
	//masses of the objects
	public float mass_boat, mass_pilot, mass_cannon, mass_com;
	//moments of inertia of the objects
	public float moi_boat, moi_pilot, moi_cannon, moi_com;
	//rotation point of the objects
	public float mh_boat, mh_pilot, mh_cannon, mh_com;
	//inertia totals of the objects
	public float it_boat, it_pilot, it_cannon, it_com;
	public float comX, comZ;
	//h of the objects
	public float h_boat, h_pilot, h_cannon;
	//pilot object used to obtain pilot data
	private Pilot pilot;
	//pilot boat used to obtain boat data
	private Boat boat;
	//pilot boat used to obtain cannon data
	private Cannon cannon;
	//the size of the data arrays to update the HUD
	private int arraySize;
	//the material used to display the COM point
	public Material comPoint;

/*------------------------------------------------------------------------------------------------------------------
-- FUNCTION: Start
--
-- DATE: January 11, 2018
--
-- DESIGNER:   Michael Goll
--
-- PROGRAMMER: Michael Goll
--
-- NOTES:
-- Initializes the various objects to call member functions.
-- Initializes the canvas controller to update the HUD.
-- Initializes the data arrays.
----------------------------------------------------------------------------------------------------------------------*/
	void Start () {
		//------- LAB #6 Additions -------
		moving = false;
		initial = true;
		numUpdates = 0;
		fixedTime = Time.fixedDeltaTime;

		oldVz = v_initial;

		//calculate different acceleration directions
		thrust.x = PhysicsCalculator.calculateXThrust(force, angle);
		thrust.z = PhysicsCalculator.calculateZThrust(force, angle);

		thrustAngle.x = PhysicsCalculator.calculateXThrust(angularForce, angle);
		thrustAngle.z = PhysicsCalculator.calculateZThrust(angularForce, angle);
	}
	
/*------------------------------------------------------------------------------------------------------------------
-- FUNCTION: Update
--
-- DATE: January 11, 2018
--
-- DESIGNER:   Michael Goll
--
-- PROGRAMMER: Michael Goll
--
-- NOTES:
-- Calls helper functions that calculate various attributes.
-- Calls helper functions to load data arrays used to display the various attributes to the user.
-- Updates the COM Point material to move the com point shader around.
----------------------------------------------------------------------------------------------------------------------*/
	void Update () {
		//------- LAB #6 Additions -------
		// acceleration = PhysicsCalculator.calculateAccelerationFromThrust(force, mass_com);

		if (Input.GetKeyDown(KeyCode.Space)) {
			moving = !moving;
		}

		if (Input.GetKeyDown(KeyCode.W)) {
			force = 30000;
			moving = true;
		}

		if (Input.GetKeyUp(KeyCode.W) && dynamicControls) {
			force = 0;
		}

		if (Input.GetKeyDown(KeyCode.Q)) {
			rotLeft = true;
			rotRight = false;
		}

		if (Input.GetKeyDown(KeyCode.E)) {
			rotRight = true;
			rotLeft = false;
		}

		if (initial) {
			//------- LAB #1 Originals -------
			pilot = p.GetComponent<Pilot>();
			boat = b.GetComponent<Boat>();
			cannon = c.GetComponent<Cannon>();

			mass_boat = boat.getMass();
			mass_pilot = pilot.getMass();
			mass_cannon = cannon.getMass();
			mass_com = PhysicsCalculator.calculateCombined(mass_boat, mass_pilot, mass_cannon);
			
			calculateMomentOfInertia();
			calculateComValues();
			calculateH();
			calculateMH();
			calculateInertiaTotals();

			comPoint.SetVector("_COMPosition", new Vector3(comX, 0, comZ));

			//------- LAB #6 Additions -------
			acceleration.x = PhysicsCalculator.calculateAccelerationFromThrust(thrust.x, mass_com);
			acceleration.z = PhysicsCalculator.calculateAccelerationFromThrust(thrust.z, mass_com);

			rLeft.x = 2 - comX;
			rLeft.y = 0;
			rLeft.z = -4 - comZ; 

			rRight.x = -2 - comX;
			rRight.y = 0;
			rRight.z = -4 - comZ;

			torqueL = PhysicsCalculator.calculateCrossProd(new Vector3(thrustAngle.x, 0, thrustAngle.z), rLeft);
			torqueR = PhysicsCalculator.calculateCrossProd(new Vector3(thrustAngle.x, 0, thrustAngle.z), rRight);

			accelerationL = PhysicsCalculator.calculateAngularAcceleration(torqueL.y, it_com);
			accelerationR = PhysicsCalculator.calculateAngularAcceleration(torqueR.y, it_com);

			initial = false;
		}
	}


	void FixedUpdate() {
		if (moving) {

			if (numUpdates == 100 && !dynamicControls) {
				moving = false;
			}

			// if (Mathf.Abs(dinZ) - distanceZ <= 0.05) {
			// 	moving = false;
			// }

			// if (angularDisplacement >= rD && rotRight) {
			// 	moving = false;
			// }

			// if (angularDisplacement >= lD && rotLeft) {
			// 	moving = false;
			// }

			//calculate the new distances and velocities
			newDx = PhysicsCalculator.calculateDistance(oldDx, acceleration.x, oldVx, fixedTime);
			newVx = PhysicsCalculator.calculateVelocity(oldVx, acceleration.x, fixedTime);

			newDz = PhysicsCalculator.calculateDistance(oldDz, acceleration.z, oldVz, Time.fixedDeltaTime);
			newVz = PhysicsCalculator.calculateVelocity(oldVz, acceleration.z, Time.fixedDeltaTime);


			if (rotLeft) {
				temp1 = accelerationL;
			}

			if (rotRight) {
				temp1 = accelerationR;
			}

			angularVelocity = oldAngularVelocity + (temp1 * fixedTime);

			angularDisplacement += Mathf.Abs(angularVelocity * fixedTime);

			
			// //update the position of the boat
			// // totalBoat.transform.position = new Vector3(totalBoat.transform.position.x + newVx * fixedTime, totalBoat.transform.position.y, totalBoat.transform.position.z + newVz * fixedTime);
			// totalBoat.transform.Translate(newVx * fixedTime, 0, newVz * fixedTime);
			// totalBoat.transform.Rotate(0, temp, 0, Space.Self);

			distanceX += newVx * fixedTime;
			distanceZ += newVz * fixedTime;

			//------- LAB #7 Additions -------
			tau = PhysicsCalculator.calculateTau(mass_com, dragCoefficient);
			vMax = PhysicsCalculator.calculteTerminalVelocity(force, dragCoefficient);

			newVz = PhysicsCalculator.calculateVelocityWithDrag(vMax, dragCoefficient, oldVz, fixedTime, mass_com);
			newDz = PhysicsCalculator.calculatePositionWithDrag(oldDz, force, dragCoefficient, fixedTime, mass_com, oldVz);

			accelerationWithDrag = PhysicsCalculator.calculateAccelerationWithDrag(force, dragCoefficient, oldVz, mass_com);

			totalBoat.transform.Translate(newVx * fixedTime, 0, newVz * fixedTime);

		}

		if (moving) {

			numUpdates++;
			 
			//update UI
			timeText.text = "Time: " + ((numUpdates) * fixedTime) + " seconds, " + (numUpdates) + " updates";
			posText.text = "Velocity: " + newVz + "m/s";
			angDText.text = "Acceleration " + accelerationWithDrag + " m/s^2";
			disText.text = "Distance: " + newDz + " m";
			
			oldDx = newDx;
			oldVx = newVx;

			oldDz = newDz;
			oldVz = newVz;
			oldAngularVelocity = angularVelocity;

		} else {
			v_final = oldVz;
		}
	}

/*------------------------------------------------------------------------------------------------------------------
-- FUNCTION: calculateMH
--
-- DATE: January 11, 2018
--
-- DESIGNER:   Michael Goll
--
-- PROGRAMMER: Michael Goll
--
-- NOTES:
-- Calls the physics helper function that calculates the moment of inertia for all the objects and 
--     stores them in the appropriate data array.
----------------------------------------------------------------------------------------------------------------------*/
	private void calculateMomentOfInertia() {
		moi_boat = PhysicsCalculator.calculateMOI(boat.getMass(), boat.getXDim(), boat.getZDim());
		moi_pilot = PhysicsCalculator.calculateMOI(pilot.getMass(), pilot.getXDim(), pilot.getZDim());
		moi_cannon = PhysicsCalculator.calculateMOI(cannon.getMass(), cannon.getXDim(), cannon.getZDim());
		moi_com = PhysicsCalculator.calculateCombined(moi_boat, moi_pilot, moi_cannon);
	}

/*------------------------------------------------------------------------------------------------------------------
-- FUNCTION: calculateComValues
--
-- DATE: January 11, 2018
--
-- DESIGNER:   Michael Goll
--
-- PROGRAMMER: Michael Goll
--
-- NOTES:
-- Calls the physics helper function that calculates the center of mass the whole object and 
--     stores them in the appropriate data array.
----------------------------------------------------------------------------------------------------------------------*/
	private void calculateComValues() {
		comX = PhysicsCalculator.calculateComX(mass_boat, boat.getX(), mass_pilot, pilot.getX(), mass_cannon, cannon.getX(), mass_com);
		comZ = PhysicsCalculator.calculateComZ(mass_boat, boat.getZ(), mass_pilot, pilot.getZ(), mass_cannon, cannon.getZ(), mass_com);
	}

/*------------------------------------------------------------------------------------------------------------------
-- FUNCTION: calculateH
--
-- DATE: January 11, 2018
--
-- DESIGNER:   Michael Goll
--
-- PROGRAMMER: Michael Goll
--
-- NOTES:
-- Calls the physics helper function that calculates the H values for all the objects and 
--     stores them in the appropriate data array.
----------------------------------------------------------------------------------------------------------------------*/
	private void calculateH() {
		h_boat = PhysicsCalculator.calculateRotationPoint(boat.getX(), boat.getZ(), comX, comZ);
		h_pilot = PhysicsCalculator.calculateRotationPoint(pilot.getX(), pilot.getZ(), comX, comZ);
		h_cannon = PhysicsCalculator.calculateRotationPoint(cannon.getX(), cannon.getZ(), comX, comZ);
	}

/*------------------------------------------------------------------------------------------------------------------
-- FUNCTION: calculateMH
--
-- DATE: January 11, 2018
--
-- DESIGNER:   Michael Goll
--
-- PROGRAMMER: Michael Goll
--
-- NOTES:
-- Calls the physics helper function that calculates the MH values for all the objects and 
--     stores them in the appropriate data array.
----------------------------------------------------------------------------------------------------------------------*/
	private void calculateMH() {
		mh_boat = PhysicsCalculator.calculateMH(h_boat, mass_boat);
		mh_pilot = PhysicsCalculator.calculateMH(h_pilot, mass_pilot);
		mh_cannon = PhysicsCalculator.calculateMH(h_cannon, mass_cannon);
		mh_com = PhysicsCalculator.calculateCombined(mh_boat, mh_pilot, mh_cannon);
	}

/*------------------------------------------------------------------------------------------------------------------
-- FUNCTION: calculateInertiaTotals
--
-- DATE: January 11, 2018
--
-- DESIGNER:   Michael Goll
--
-- PROGRAMMER: Michael Goll
--
-- NOTES:
-- Calls the physics helper function that calculates the inertia totals for all the objects and 
--     stores them in the appropriate data array.
----------------------------------------------------------------------------------------------------------------------*/
	private void calculateInertiaTotals() {
		it_boat = PhysicsCalculator.calculateInertiaTotal(moi_boat, mh_boat);
		it_pilot = PhysicsCalculator.calculateInertiaTotal(moi_pilot, mh_pilot);
		it_cannon = PhysicsCalculator.calculateInertiaTotal(moi_cannon, mh_cannon);
		it_com = PhysicsCalculator.calculateCombined(it_boat, it_pilot, it_cannon);
	}
}