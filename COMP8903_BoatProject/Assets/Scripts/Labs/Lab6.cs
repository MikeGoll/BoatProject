using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lab6 : MonoBehaviour {

	//---------------------- LAB #6 Additions ----------------------
	//force applied and angle
	public float force, angle;

	//final distance z
	public float dinZ;

	//acceleration, initial velocity, final velocity, new intermediate velocity, old intermediate velocity
	public float acceleration, v_initial, v_final, newV, oldV;

	//total distance, new displacement and old displacement
	public float distance, newD, oldD;
	public Text timeText, posText;
	
	private bool moving;
	private float numUpdates;
	private float fixedTime;

	//---------------------- LAB #1 Originals ----------------------
	//GameObject references for the boat, pilot and cannon
	public GameObject b, p, c, totalBoat;
	//canvas object used to grab the CanvasController class
	public Canvas canvas;
	//data arrays that hold calculated values for various objects.
	private float[] boatFloats, pilotFloats, cannonFloats, comFloats;
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

		//------- LAB #1 Originals -------
		pilot = p.GetComponent<Pilot>();
		boat = b.GetComponent<Boat>();
		cannon = c.GetComponent<Cannon>();
		arraySize = 9;

		boatFloats = new float[arraySize];
		pilotFloats = new float[arraySize];
		cannonFloats = new float[arraySize];
		comFloats = new float[arraySize];
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
		 //------- LAB #1 Originals -------
		mass_boat = boat.getMass();
		mass_pilot = pilot.getMass();
		mass_cannon = cannon.getMass();
		mass_com = PhysicsCalculator.calculateCombined(mass_boat, mass_pilot, mass_cannon);

		boatFloats[0] = mass_boat;
		pilotFloats[0] = mass_pilot;
		cannonFloats[0] = mass_cannon;
		comFloats[0] = mass_com;

		loadBoatFloats();
		loadPilotFloats();
		loadCannonFloats();
		
		calculateMomentOfInertia();
		calculateComValues();
		calculateH();
		calculateMH();
		calculateInertiaTotals();

		comPoint.SetVector("_COMPosition", new Vector3(comX, 0, comZ));

		//------- LAB #6 Additions -------
		acceleration = PhysicsCalculator.calculateAccelerationFromThrust(force, mass_com);

		if (Input.GetKeyDown(KeyCode.Space)) {
			if (!moving)
				moving = true;
			else
				Debug.Log("Already moving");
		}
	}


	void FixedUpdate() {
		if (moving) {

			if (dinZ - totalBoat.transform.position.z <= 0.05) {
				moving = false;
			}

			//update the calculations
			newD = PhysicsCalculator.calculateDistance(oldD, acceleration, oldV, Time.fixedDeltaTime);
			newV = PhysicsCalculator.calculateVelocity(oldV, acceleration, Time.fixedDeltaTime);
		}		

		if (moving) {
			//update the position of the boat
			totalBoat.transform.Translate(Vector3.forward * oldV * Time.deltaTime);

			//update UI
			// gammaText.text = "Angular Velocity: " + omegaF;
			// angleText.text = "Acceleration: " + theta;
			// updatesText.text = "Updates: " + (numUpdates + 1) + " frames";
			timeText.text = "Time: " + ((numUpdates) * fixedTime) + " seconds, " + (numUpdates) + " updates";
			posText.text = "Position: " + totalBoat.transform.position.z + " m";
			
			oldD = newD;
			oldV = newV;

			numUpdates++;
		} else {
			v_final = oldV;
		}
	}

/*------------------------------------------------------------------------------------------------------------------
-- FUNCTION: loadBoatFloats
--
-- DATE: January 11, 2018
--
-- DESIGNER:   Michael Goll
--
-- PROGRAMMER: Michael Goll
--
-- NOTES:
-- Loads the data array for all the boat attributes that can change on the fly.
-- Only updates the position of the boat and dimensions of the boat.
----------------------------------------------------------------------------------------------------------------------*/
	private void loadBoatFloats() {
		boatFloats[4] = boat.getX();
		boatFloats[5] = boat.getZ();
		boatFloats[6] = boat.getXDim();
		boatFloats[7] = boat.getZDim();
	}

/*------------------------------------------------------------------------------------------------------------------
-- FUNCTION: loadCannonFloats
--
-- DATE: January 11, 2018
--
-- DESIGNER:   Michael Goll
--
-- PROGRAMMER: Michael Goll
--
-- NOTES:
-- Loads the data array for all the cannon attributes that can change on the fly.
-- Only updates the position of the cannon and dimensions of the boat.
----------------------------------------------------------------------------------------------------------------------*/
	private void loadCannonFloats() {
		cannonFloats[4] = cannon.getX();
		cannonFloats[5] = cannon.getZ();
		cannonFloats[6] = cannon.getXDim();
		cannonFloats[7] = cannon.getZDim();
	}

/*------------------------------------------------------------------------------------------------------------------
-- FUNCTION: loadPilotFloats
--
-- DATE: January 11, 2018
--
-- DESIGNER:   Michael Goll
--
-- PROGRAMMER: Michael Goll
--
-- NOTES:
-- Loads the data array for all the pilot attributes that can change on the fly.
-- Only updates the position of the pilot and dimensions of the boat.
----------------------------------------------------------------------------------------------------------------------*/
	private void loadPilotFloats() {
		pilotFloats[4] = pilot.getX();
		pilotFloats[5] = pilot.getZ();
		pilotFloats[6] = pilot.getXDim();
		pilotFloats[7] = pilot.getZDim();
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

		boatFloats[1] = moi_boat;
		pilotFloats[1] = moi_pilot;
		cannonFloats[1] = moi_cannon;
		comFloats[1] = moi_com;
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

		comFloats[4] = comX;
		comFloats[5] = comZ;
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

		boatFloats[2] = h_boat;
		pilotFloats[2] = h_pilot;
		cannonFloats[2] = h_cannon;
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

		boatFloats[3] = mh_boat;
		pilotFloats[3] = mh_pilot;
		cannonFloats[3] = mh_cannon;
		comFloats[3] = mh_com;
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

		boatFloats[8] = it_boat;
		pilotFloats[8] = it_pilot;
		cannonFloats[8] = it_cannon;
		comFloats[8] = it_com;
	}

/*------------------------------------------------------------------------------------------------------------------
-- FUNCTION: getPilotFloats
--
-- DATE: January 11, 2018
--
-- DESIGNER:   Michael Goll
--
-- PROGRAMMER: Michael Goll
--
-- NOTES:
-- Returns the boat float values in an array.
-- Used by the CanvasController to retrieve updated values
----------------------------------------------------------------------------------------------------------------------*/
	public float[] getBoatFloats() {
		return boatFloats;
	}

/*------------------------------------------------------------------------------------------------------------------
-- FUNCTION: getPilotFloats
--
-- DATE: January 11, 2018
--
-- DESIGNER:   Michael Goll
--
-- PROGRAMMER: Michael Goll
--
-- NOTES:
-- Returns the pilot float values in an array.
-- Used by the CanvasController to retrieve updated values
----------------------------------------------------------------------------------------------------------------------*/
	public float[] getPilotFloats() {
		return pilotFloats;
	}

/*------------------------------------------------------------------------------------------------------------------
-- FUNCTION: getCannonFloats
--
-- DATE: January 11, 2018
--
-- DESIGNER:   Michael Goll
--
-- PROGRAMMER: Michael Goll
--
-- NOTES:
-- Returns the cannon float values in an array.
-- Used by the CanvasController to retrieve updated values
----------------------------------------------------------------------------------------------------------------------*/
	public float[] getCannonFloats() {
		return cannonFloats;
	}

/*------------------------------------------------------------------------------------------------------------------
-- FUNCTION: getComFloats
--
-- DATE: January 11, 2018
--
-- DESIGNER:   Michael Goll
--
-- PROGRAMMER: Michael Goll
--
-- NOTES:
-- Returns the com float values in an array.
-- Used by the CanvasController to retrieve updated values
----------------------------------------------------------------------------------------------------------------------*/
	public float[] getComFloats() {
		return comFloats;
	}
}
