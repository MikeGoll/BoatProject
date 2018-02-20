using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*------------------------------------------------------------------------------------------------------------------
-- FUNCTION: Cannon
--
-- DATE: January 11, 2018
--
-- DESIGNER:   Michael Goll
--
-- PROGRAMMER: Michael Goll
--
-- NOTES:
-- Initializes and stores all relevant data for the cannon object.
----------------------------------------------------------------------------------------------------------------------*/
public class Cannon : MonoBehaviour {

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
-- Initializes the mass of the cannon.
----------------------------------------------------------------------------------------------------------------------*/
	void Start () {
		mass = 0;
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
-- Updates the X and Z position as well as the x and z dimension.
----------------------------------------------------------------------------------------------------------------------*/
	void Update () {
		x_position = transform.position.x;
		z_position = transform.position.z;
		x_dimension = 1;
		z_dimension = 2;
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
}
