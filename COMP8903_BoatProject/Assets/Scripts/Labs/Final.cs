using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Final : MonoBehaviour {

	//---------------------- LAB #7 Additions ----------------------
	[Header("Lab 7 Attributes - Editable")]
	[Tooltip("Enables/Disables the 2 second stop. Disable to allow for dynamic acceleration")]
	public bool dynamicControls;
	public float dragCoefficient;

	[Header("Lab 7 Calculated")]
	public float tau;
	public float vMax;
	public float accelerationWithDrag;

	//---------------------- LAB #6 Additions ----------------------
	[Header("Lab 6 Attributes - Editable")]
	//User entered data through UI (force, angle of force, angle, final displacement z, left angular displacement, right angular displacement)
	public float angularForce;
	public float angle;

	[Header("Lab 6 Calculated")]
	public float force;
	[Header("Final Calculated")]
	public float oMax;
	public float oldOmega, omega;
	public float turnForce;
	private int direction;
	// public float dinZ;
	// public float lD;
	// public float rD;

	//thrust in X and Z axis
	[Header("Thrust Values")]
	public Vector3 thrust;
	public Vector3 thrustAngle;
	public Vector3 rLeft;
	public Vector3 rRight;
	[Header("Torque & Acceleration Values")]
	public Vector3 torqueL;
	public Vector3 torqueR;
	public Vector3 acceleration;

	public float accelerationL, accelerationR;

	[Header("Velocity Values")]
	//acceleration, initial velocity, final velocity, new intermediate velocity, old intermediate velocity
	public float v_initial;
	public float v_final;
	public float newVx;
	public float oldVx;
	public float newVz;
	public float oldVz;

	//total distance, new and old distance
	[Header("Distance Values")]
	public float distance;
	public float newDx;
	public float oldDx;
	public float newDz;
	public float oldDz;

	[Header("Objects and Text")]
	public GameObject totalBoat;
	public Text timeText, posText, angDText, disText;
	
	private bool moving, initial, rotLeft, rotRight;
	private float numUpdates;
	private float fixedTime;
	private float angularVelocity, oldAngularVelocity, angularDisplacement, oldAngularDisplacement;
	private float distanceZ, distanceX;
	private float temp1;

	//---------------------- LAB #1 Originals ----------------------
	[Header("Lab 1 Objects")]
	//GameObject references for the boat, pilot and cannon
	public GameObject b;
	public GameObject p;
	public GameObject c;
	//masses of the objects
	[Header("Mass Values")]
	public float mass_boat;
	public float mass_pilot;
	public float mass_cannon;
	public float mass_com;
	//moments of inertia of the objects
	[Header("Moment of Inertia Values")]
	public float moi_boat;
	public float moi_pilot;
	public float moi_cannon;
	public float moi_com;
	//rotation point of the objects
	[Header("MH Values")]
	public float mh_boat; 
	public float mh_pilot;
	public float mh_cannon;
	public float mh_com;
	//inertia totals of the objects
	[Header("Combined Values")]
	public float it_boat;
	public float it_pilot;
	public float it_cannon;
	public float it_com;

	[Space(20)]
	public float comX;
	public float comZ;
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
		moving = true;
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
			force = 4000;
		}

		if (Input.GetKeyUp(KeyCode.W) && dynamicControls) {
			force = 0;
		}

		if (Input.GetKeyDown(KeyCode.Q)) {
			rotLeft = true;
			rotRight = false;

			turnForce = 40000;
			direction = -1;
		}

		if (Input.GetKeyDown(KeyCode.E)) {
			rotRight = true;
			rotLeft = false;

			turnForce = 40000;
			direction = 1;
		}

		if (Input.GetKeyUp(KeyCode.E) || Input.GetKeyUp(KeyCode.Q)) {
			rotRight = false;
			rotLeft = false;

			turnForce = 0;
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

			//calculate the new distances and velocities
			newDx = PhysicsCalculator.calculateDistance(oldDx, acceleration.x, oldVx, fixedTime);
			newVx = PhysicsCalculator.calculateVelocity(oldVx, acceleration.x, fixedTime);

			newDz = PhysicsCalculator.calculateDistance(oldDz, acceleration.z, oldVz, Time.fixedDeltaTime);
			newVz = PhysicsCalculator.calculateVelocity(oldVz, acceleration.z, Time.fixedDeltaTime);

			angularVelocity = oldAngularVelocity + (temp1 * fixedTime);

			angularDisplacement += Mathf.Abs(angularVelocity * fixedTime);

			distanceX += newVx * fixedTime;
			distanceZ += newVz * fixedTime;


			//------- LAB #7 Additions -------
			tau = PhysicsCalculator.calculateTau(mass_com, dragCoefficient);
			vMax = PhysicsCalculator.calculteTerminalVelocity(force, dragCoefficient);

			newVz = PhysicsCalculator.calculateVelocityWithDrag(vMax, dragCoefficient, oldVz, fixedTime, mass_com);
			newDz = PhysicsCalculator.calculatePositionWithDrag(oldDz, force, dragCoefficient, fixedTime, mass_com, oldVz);

			accelerationWithDrag = PhysicsCalculator.calculateAccelerationWithDrag(force, dragCoefficient, oldVz, mass_com);

			totalBoat.transform.Translate(newVx * fixedTime, 0, newVz * fixedTime);

			oMax = PhysicsCalculator.calculateTerminalAngularVelocity(turnForce, dragCoefficient);
			omega = PhysicsCalculator.calculateAngularVelocityWithDrag(oMax, dragCoefficient, oldOmega, fixedTime, mass_com);

			totalBoat.transform.Rotate(0, direction * omega * fixedTime, 0, Space.Self);
		}

		if (moving) {

			numUpdates++;
			 
			//update UI
			timeText.text = "Time: " + ((numUpdates) * fixedTime) + " seconds";
			posText.text = "Velocity: " + newVz + "m/s";
			angDText.text = "Acceleration " + accelerationWithDrag + " m/s^2";
			disText.text = "Distance: " + newDz + " m";
			
			oldDx = newDx;
			oldVx = newVx;

			oldDz = newDz;
			oldVz = newVz;
			oldAngularVelocity = angularVelocity;

			oldOmega = omega;

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