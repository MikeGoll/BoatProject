using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Final : MonoBehaviour {

	//---------------------- LAB #7 Additions ----------------------
	[Header("Lab 7 Attributes - Editable")]
	public float dragCoefficient;

	[Header("Lab 7 Calculated")]
	public float tau;
	public float vMax;
	public float accelerationWithDrag;

	[Header("Lab 6 Calculated")]
	public float force;

	[Header("Final Calculated")]
	public float oMax;
	public float oldOmega, omega;
	public float turnForce;

	[Header("Velocity Values")]
	//acceleration, initial velocity, final velocity, new intermediate velocity, old intermediate velocity
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

	//---------------------- LAB #1 Originals ----------------------
	[Header("Lab 1 Objects")]
	//GameObject references for the boat, pilot and cannon
	public GameObject b;
	public GameObject p;
	public GameObject c;

	//masses of the objects
	private float mass_boat;
	private float mass_pilot;
	private float mass_cannon;
	private float mass_com;

	//moments of inertia of the objects
	private float moi_boat;
	private float moi_pilot;
	private float moi_cannon;
	private float moi_com;

	//rotation point of the objects
	private float mh_boat; 
	private float mh_pilot;
	private float mh_cannon;
	private float mh_com;

	//inertia totals of the objects
	private float it_boat;
	private float it_pilot;
	private float it_cannon;
	private float it_com;

	[Space(20)]
	private float comX;
	private float comZ;

	//h of the objects
	private float h_boat, h_pilot, h_cannon;

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
		moving = true;
		initial = true;
		numUpdates = 0;
		fixedTime = Time.fixedDeltaTime;
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

		//Pause/unpause simulation
		if (Input.GetKeyDown(KeyCode.Space)) {
			moving = !moving;
		}

		//Forward Thrust
		if (Input.GetKeyDown(KeyCode.W)) {
			force = 4000;
		}

		//Release forward thrust
		if (Input.GetKeyUp(KeyCode.W)) {
			force = 0;
		}

		//Left torque
		if (Input.GetKeyDown(KeyCode.Q)) {
			rotLeft = true;
			rotRight = false;

			turnForce = -1000;
		}

		//Right torque
		if (Input.GetKeyDown(KeyCode.E)) {
			rotRight = true;
			rotLeft = false;

			turnForce = 1000;
		}

		//Release directional torque
		if (Input.GetKeyUp(KeyCode.E) || Input.GetKeyUp(KeyCode.Q)) {
			rotRight = false;
			rotLeft = false;

			turnForce = 0;
		}

		if (initial) {
			//Initialize objects to access attributes
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
			initial = false;
		}
	}


	void FixedUpdate() {
		if (moving) {

			//Calculate tau dynamically to account for mass changes
			tau = PhysicsCalculator.calculateTau(mass_com, dragCoefficient);

			//Calculate the boat's velocity for its forward motion
			vMax = PhysicsCalculator.calculteTerminalVelocity(force, dragCoefficient);
			newVz = PhysicsCalculator.calculateVelocityWithDrag(vMax, dragCoefficient, oldVz, fixedTime, mass_com);
			newDz = PhysicsCalculator.calculatePositionWithDrag(oldDz, force, dragCoefficient, fixedTime, mass_com, oldVz);

			//Calculate the linear movement acceleration with drag
			accelerationWithDrag = PhysicsCalculator.calculateAccelerationWithDrag(force, dragCoefficient, oldVz, mass_com);

			//Calculate the boat's velocity and acceleration with drag for its angular motion
			oMax = PhysicsCalculator.calculateTerminalAngularVelocity(turnForce, dragCoefficient);
			omega = PhysicsCalculator.calculateAngularVelocityWithDrag(oMax, dragCoefficient, oldOmega, fixedTime, mass_com);

			//Update the boat's position
			totalBoat.transform.Translate(newVx * fixedTime, 0, newVz * fixedTime);
			totalBoat.transform.Rotate(0, Mathf.Rad2Deg * (omega * fixedTime), 0, Space.Self);
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
			
			oldOmega = omega;
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