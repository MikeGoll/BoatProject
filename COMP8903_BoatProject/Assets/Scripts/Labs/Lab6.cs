using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lab6 : MonoBehaviour {

	//---------------------- LAB #6 Additions ----------------------
	public GameObject totalBoat;
	//force applied and angle
	public float force, angle, dinZ, thrustX, thrustZ;

	//acceleration, initial velocity, final velocity, new intermediate velocity, old intermediate velocity
	public float accelerationX, accelerationZ, v_initial, v_final, newVx, oldVx, newVz, oldVz;

	//total distance, new and old distance
	public float distance, newDx, oldDx, newDz, oldDz;
	public Text timeText, posText;
	
	private bool moving;
	private float numUpdates;
	private float fixedTime;

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
		numUpdates = 0;
		fixedTime = Time.fixedDeltaTime;

		//calculate different acceleration directions
		thrustX = PhysicsCalculator.calculateXThrust(force, angle);
		thrustZ = PhysicsCalculator.calculateZThrust(force, angle);

		//------- LAB #1 Originals -------
		pilot = p.GetComponent<Pilot>();
		boat = b.GetComponent<Boat>();
		cannon = c.GetComponent<Cannon>();
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
			if (!moving)
				moving = true;
			else
				Debug.Log("Already moving");
		}
	}


	void FixedUpdate() {

		 //------- LAB #1 Originals -------
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

		if (moving) {

			if (Mathf.Abs(dinZ) - Mathf.Abs(totalBoat.transform.position.z) <= 0.05) {
				moving = false;
			}

			accelerationX = PhysicsCalculator.calculateAccelerationFromThrust(thrustX, mass_com);
			accelerationZ = PhysicsCalculator.calculateAccelerationFromThrust(thrustZ, mass_com);

			//calculate the new distances and velocities
			newDx = PhysicsCalculator.calculateDistance(oldDx, accelerationX, oldVx, fixedTime);
			newVx = PhysicsCalculator.calculateVelocity(oldVx, accelerationX, fixedTime);

			newDz = PhysicsCalculator.calculateDistance(oldDz, accelerationZ, oldVz, Time.fixedDeltaTime);
			newVz = PhysicsCalculator.calculateVelocity(oldVz, accelerationZ, Time.fixedDeltaTime);
		}		

		if (moving) {
			numUpdates++;

			//update the position of the boat
			totalBoat.transform.position = new Vector3(totalBoat.transform.position.x + newVx * fixedTime, totalBoat.transform.position.y, totalBoat.transform.position.z + newVz * fixedTime);

			//update UI
			timeText.text = "Time: " + ((numUpdates) * fixedTime) + " seconds, " + (numUpdates) + " updates";
			posText.text = "Position: " + totalBoat.transform.position.x  + ", " + totalBoat.transform.position.y + ", " + totalBoat.transform.position.z;
			
			oldDx = newDx;
			oldVx = newVx;

			oldDz = newDz;
			oldVz = newVz;

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