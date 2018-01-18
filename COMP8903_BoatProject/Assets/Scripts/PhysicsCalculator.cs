using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*------------------------------------------------------------------------------------------------------------------
-- FUNCTION: PhysicsCalculator
--
-- DATE: January 11, 2018
--
-- DESIGNER:   Michael Goll
--
-- PROGRAMMER: Michael Goll
--
-- NOTES:
-- Handles all the physics calculations of the program.
----------------------------------------------------------------------------------------------------------------------*/
public class PhysicsCalculator : MonoBehaviour {

	/*
		Hull	500	4	8	0	0	3.33E+03	0.2653810836  1.33E+02	3.47E+03
		Gun	    100	1	2	0	-4	4.17E+01	12.14416896	  1.21E+03	1.26E+03
		Pilot	 60	1	1	0	1	1.00E+01	2.295684114	  1.38E+02	1.48E+02
		COM	    660			0	-0.5151515152	3.39E+03	  1.48E+03	4.87E+03
	 */

	 /*
	 	Calculate and Display the following:  
			1.Position of:  Hull, Pilot, Gun,  COM
			2.Mass of:  Hull, Pilot, Gun,  Total
			3.Moment  of Inertia, 2h, 2Mh for Hull, Pilot, Gun
			4.Total Moment  of inertia
	  */

/*------------------------------------------------------------------------------------------------------------------
-- FUNCTION: calculateCombined
--
-- DATE: January 11, 2018
--
-- DESIGNER:   Michael Goll
--
-- PROGRAMMER: Michael Goll
--
-- NOTES:
-- Sums three floats.
-- Used to calculate combined totals of all the objects.
----------------------------------------------------------------------------------------------------------------------*/
	  public static float calculateCombined(float hull, float pilot, float cannon) {
		  return hull + pilot + cannon;
	  }	  


/*------------------------------------------------------------------------------------------------------------------
-- FUNCTION: calculateMOI
--
-- DATE: January 11, 2018
--
-- DESIGNER:   Michael Goll
--
-- PROGRAMMER: Michael Goll
--
-- NOTES:
-- Calculates the moment of inertia of an object given its mass, and x and dimensions.
----------------------------------------------------------------------------------------------------------------------*/

	  public static float calculateMOI(float mass, float x, float z) {
		  const int divisor = 12;
		  return (mass * (Mathf.Pow(x, 2) + Mathf.Pow(z, 2))) / divisor;
	  }

/*------------------------------------------------------------------------------------------------------------------
-- FUNCTION: calculateRotationPoint
--
-- DATE: January 11, 2018
--
-- DESIGNER:   Michael Goll
--
-- PROGRAMMER: Michael Goll
--
-- NOTES:
-- Calculates the rotation point of an object
----------------------------------------------------------------------------------------------------------------------*/

	  public static float calculateRotationPoint(float x, float z, float comX, float comZ) {
		  return (Mathf.Pow(comX - x, 2) + Mathf.Pow(comZ - z, 2));
	  }

/*------------------------------------------------------------------------------------------------------------------
-- FUNCTION: calculateComX
--
-- DATE: January 11, 2018
--
-- DESIGNER:   Michael Goll
--
-- PROGRAMMER: Michael Goll
--
-- NOTES:
-- Calculates the combined center of mass for the x position.
----------------------------------------------------------------------------------------------------------------------*/

	  public static float calculateComX(float bMass, float bx, float pMass, float px, float cMass, float cx, float comMass) {
		  return (((bMass * bx) + (pMass * px) + (cMass * cx)) / comMass);
	  }

/*------------------------------------------------------------------------------------------------------------------
-- FUNCTION: calculateComZ
--
-- DATE: January 11, 2018
--
-- DESIGNER:   Michael Goll
--
-- PROGRAMMER: Michael Goll
--
-- NOTES:
-- Calculates the center of mass for the z position.
----------------------------------------------------------------------------------------------------------------------*/

	  public static float calculateComZ(float bMass, float bz, float pMass, float pz, float cMass, float cz, float comMass) {
		  return (((bMass * bz) + (pMass * pz) + (cMass * cz)) / comMass);
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
-- Calculates the MH value for an object.
----------------------------------------------------------------------------------------------------------------------*/

	  public static float calculateMH(float h, float mass) {
		  return h * mass;
	  }

/*------------------------------------------------------------------------------------------------------------------
-- FUNCTION: calculateInertiaTotal
--
-- DATE: January 11, 2018
--
-- DESIGNER:   Michael Goll
--
-- PROGRAMMER: Michael Goll
--
-- NOTES:
-- Sums two floats to calculate the total inertia for the object.
----------------------------------------------------------------------------------------------------------------------*/

	  public static float calculateInertiaTotal(float x, float y) {
		  return x + y;
	  }
}
