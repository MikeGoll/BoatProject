using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*------------------------------------------------------------------------------------------------------------------
-- FUNCTION: Boat
--
-- DATE: January 11, 2018
--
-- DESIGNER:   Michael Goll
--
-- PROGRAMMER: Michael Goll
--
-- NOTES:
-- Initializes and stores all relevant data for the boat object.
----------------------------------------------------------------------------------------------------------------------*/
public class Boat : MonoBehaviour {

	private float mass;
	private float x_position;
	private float z_position;
	private float x_dimension, z_dimension;
	private Vector3 position;
	

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
-- Initializes the mass of the boat.
----------------------------------------------------------------------------------------------------------------------*/
	void Start () {
		mass = 15500;
	}
	
/*------------------------------------------------------------------------------------------------------------------
-- FUNCTION: getX
--
-- DATE: January 11, 2018
--
-- DESIGNER:   Michael Goll
--
-- PROGRAMMER: Michael Goll
--
-- NOTES:
-- Updates the X and Z positions as well as the X and Z dimensions.
----------------------------------------------------------------------------------------------------------------------*/
	void Update () {
		x_position = transform.position.x;
		z_position = transform.position.z;
		x_dimension = 4;
		z_dimension = 8;
	}

/*------------------------------------------------------------------------------------------------------------------
-- FUNCTION: getX
--
-- DATE: January 11, 2018
--
-- DESIGNER:   Michael Goll
--
-- PROGRAMMER: Michael Goll
--
-- NOTES:
-- Returns the X value.
----------------------------------------------------------------------------------------------------------------------*/
	public float getX() {
		return x_position;
	}

/*------------------------------------------------------------------------------------------------------------------
-- FUNCTION: getZ
--
-- DATE: January 11, 2018
--
-- DESIGNER:   Michael Goll
--
-- PROGRAMMER: Michael Goll
--
-- NOTES:
-- Returns the Z value.
----------------------------------------------------------------------------------------------------------------------*/
	public float getZ() {
		return z_position;
	}

/*------------------------------------------------------------------------------------------------------------------
-- FUNCTION: getXDim
--
-- DATE: January 11, 2018
--
-- DESIGNER:   Michael Goll
--
-- PROGRAMMER: Michael Goll
--
-- NOTES:
-- Returns the X dimension value.
----------------------------------------------------------------------------------------------------------------------*/
	public float getXDim() {
		return x_dimension;
	}

/*------------------------------------------------------------------------------------------------------------------
-- FUNCTION: getZDim
--
-- DATE: January 11, 2018
--
-- DESIGNER:   Michael Goll
--
-- PROGRAMMER: Michael Goll
--
-- NOTES:
-- Returns the Z dimension value.
----------------------------------------------------------------------------------------------------------------------*/
	public float getZDim() {
		return z_dimension;
	}

/*------------------------------------------------------------------------------------------------------------------
-- FUNCTION: getMass
--
-- DATE: January 11, 2018
--
-- DESIGNER:   Michael Goll
--
-- PROGRAMMER: Michael Goll
--
-- NOTES:
-- Returns the mass value.
----------------------------------------------------------------------------------------------------------------------*/
	public float getMass() {
		return mass;
	}

	public void setMass(float newMass) {
		mass = newMass;
	}
}
