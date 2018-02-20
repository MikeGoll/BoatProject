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

	//EXP constant for lab #2
	private const float EXP = 2.71828182845904f;
	
	//Acceleration due to gravity for lab #3
	private const float GRAVITY = 9.81f;

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

// ------------------------------------------------- LAB #1 -------------------------------------------------

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

// ------------------------------------------------- LAB #2 -------------------------------------------------

/*------------------------------------------------------------------------------------------------------------------
-- FUNCTION: calculateDistance
--
-- DATE: January 23, 2018
--
-- DESIGNER:   Michael Goll
--
-- PROGRAMMER: Michael Goll
--
-- NOTES:
-- Calculates the distance for the uniform-acceleration case.
----------------------------------------------------------------------------------------------------------------------*/
	  public static float calculateDistance(float si, float a, float init_velocity, float time) {
		  return si + (init_velocity * time) + (0.5f * a * Mathf.Pow(time, 2));
	  }

/*------------------------------------------------------------------------------------------------------------------
-- FUNCTION: calculateVelocity
--
-- DATE: January 23, 2018
--
-- DESIGNER:   Michael Goll
--
-- PROGRAMMER: Michael Goll
--
-- NOTES:
-- Calculates the velocity for the uniform-acceleration case.
----------------------------------------------------------------------------------------------------------------------*/
	  public static float calculateVelocity(float vi, float a, float time) {
		  return (vi + (a * time));
	  }

/*------------------------------------------------------------------------------------------------------------------
-- FUNCTION: calculateDistanceDrag
--
-- DATE: January 23, 2018
--
-- DESIGNER:   Michael Goll
--
-- PROGRAMMER: Michael Goll
--
-- NOTES:
-- Calculates the velocity for the non-uniform acceleration case.
----------------------------------------------------------------------------------------------------------------------*/
	  public static float calculateDistanceDrag(float si, float vi, float time, float k) {
		  return si + ((Mathf.Log(1f + k * vi * time)) / k);
	  }

/*------------------------------------------------------------------------------------------------------------------
-- FUNCTION: calculateVelocityDrag
--
-- DATE: January 23, 2018
--
-- DESIGNER:   Michael Goll
--
-- PROGRAMMER: Michael Goll
--
-- NOTES:
-- Calculates the drag velocity for the non-uniform acceleration case.
----------------------------------------------------------------------------------------------------------------------*/
	  public static float calculateVelocityDrag(float vi, float a, float time, float k) {
		  return vi / (1f + k * vi * time);
	  }

/*------------------------------------------------------------------------------------------------------------------
-- FUNCTION: calculateDragConstant
--
-- DATE: January 23, 2018
--
-- DESIGNER:   Michael Goll
--
-- PROGRAMMER: Michael Goll
--
-- NOTES:
-- Calculates the drag constant for the non-uniform acceleration case.
----------------------------------------------------------------------------------------------------------------------*/
	  public static float calculateDragConstant(float a, float v) {
		  return -a / Mathf.Pow(v, 2);
	  }

/*------------------------------------------------------------------------------------------------------------------
-- FUNCTION: calculateDragTime
--
-- DATE: January 24, 2018
--
-- DESIGNER:   Michael Goll
--
-- PROGRAMMER: Michael Goll
--
-- NOTES:
-- Calculates the drag time for the non-uniform acceleration case.
----------------------------------------------------------------------------------------------------------------------*/
	  public static float calculateDragTime(float damped, float distance, float velocity) {
		  return ((Mathf.Pow(EXP, damped * distance) - 1f) / damped) / velocity;
	  }


// ------------------------------------------------- LAB #3 -------------------------------------------------

/*------------------------------------------------------------------------------------------------------------------
-- FUNCTION: calculateRange
--
-- DATE: January 31, 2018
--
-- DESIGNER:   Michael Goll
--
-- PROGRAMMER: Michael Goll
--
-- NOTES:
-- Calculates the range between the middle of the cannon and the middle of the target.
----------------------------------------------------------------------------------------------------------------------*/
	public static float calculateRange(GameObject boat, GameObject target) {
		return Mathf.Abs((target.transform.position.z - boat.transform.position.z));
	}

/*------------------------------------------------------------------------------------------------------------------
-- FUNCTION: calculateProjTime
--
-- DATE: January 31, 2018
--
-- DESIGNER:   Michael Goll
--
-- PROGRAMMER: Michael Goll
--
-- NOTES:
-- Calculates the projectile's total time to hit the target.
----------------------------------------------------------------------------------------------------------------------*/
	public static float calculateProjTime(float distance, float v, float cos) {
		return distance / (v * Mathf.Cos(cos));
	}

/*------------------------------------------------------------------------------------------------------------------
-- FUNCTION: calculateTheta
--
-- DATE: January 31, 2018
--
-- DESIGNER:   Michael Goll
--
-- PROGRAMMER: Michael Goll
--
-- NOTES:
-- Calculates the angle that the cannon will have to fire at to hit the target.
-- Returns the answer in radians.
----------------------------------------------------------------------------------------------------------------------*/
	//returns the theta value in radians
	public static float calculateTheta(float distance, float v) {
		return (Mathf.Asin((GRAVITY * distance) / Mathf.Pow(v, 2))) / 2;
	}

/*------------------------------------------------------------------------------------------------------------------
-- FUNCTION: calculateXVelocity
--
-- DATE: January 31, 2018
--
-- DESIGNER:   Michael Goll
--
-- PROGRAMMER: Michael Goll
--
-- NOTES:
-- Calculates the X velocity component given the total velocity and angle.
----------------------------------------------------------------------------------------------------------------------*/
	public static float calculateXVelocity(float velocity, float theta) {
		return Mathf.Cos((theta * Mathf.PI) / 180) * velocity;
	}

/*------------------------------------------------------------------------------------------------------------------
-- FUNCTION: calculateYVelocity
--
-- DATE: January 31, 2018
--
-- DESIGNER:   Michael Goll
--
-- PROGRAMMER: Michael Goll
--
-- NOTES:
-- Calculates the Y velocity component given the total velocity and angle.
----------------------------------------------------------------------------------------------------------------------*/
	public static float calculateYVelocity(float velocity, float theta) {
		return Mathf.Sin((theta * Mathf.PI) / 180) * velocity;
	}

// ------------------------------------------------- LAB #4 -------------------------------------------------

	public static float toDegrees(float rads) {
		return (rads * 180) / Mathf.PI;
	}

	public static float calculateXDifference(GameObject boat, GameObject target) {
		return Mathf.Abs((target.transform.position.x - boat.transform.position.x));
	}

	public static float calculateAngle(float g, float xDifference, float zDifference, float vi) {
		return (Mathf.Asin(Mathf.Abs(g) * (Mathf.Sqrt(Mathf.Pow(xDifference, 2) + Mathf.Pow(zDifference, 2))) / Mathf.Pow(vi, 2)));
	}

	public static float calculateGamma(float xDifference, float zDifference) {
		return (Mathf.Asin(xDifference / Mathf.Sqrt((Mathf.Pow(zDifference, 2) + Mathf.Pow(xDifference, 2)))));
	}

	public static float calculateXPosition(float xi, float xvi, float alpha, float gamma, float t) {
		return (xi + (xvi * Mathf.Sin(alpha) * Mathf.Sin(gamma) * t));
	}

	public static float calculateZPosition(float zi, float zvi, float alpha, float gamma, float t) {
		return (zi + (zvi * Mathf.Sin(alpha) * Mathf.Cos(gamma) * t));
	}

	public static float calculateGammaVelocity(float speed, float gamma, float alpha) {
		return (speed * gamma * alpha);
	}

	public static float calculateGammaYVelocity(float speed, float alpha) {
		return (speed * Mathf.Cos(alpha * Mathf.PI / 180));
	}

	public static float calculateYVelocityGamma(float vi, float g, float t) {
		return (vi - g * t);
	}

	public static float calculateYPosition(float yi, float vi, float g, float t) {
		return (yi + (vi * t) - (0.5f * g * Mathf.Pow(t, 2)));
	}


// ------------------------------------------------- LAB #5 -------------------------------------------------

	public static float calculateOmegaFinal(float omegaI, float alpha, float t) {
		return (omegaI + alpha * t);
	}

	public static float calculateThetaFinal(float omegaI, float alpha, float t) {
		return ((omegaI * t) + (alpha * Mathf.Pow(t, 2) / 2));
	}

	public static float calculateAngularDisplacement(float theta, float omega, float alpha, float t) {
		return ((omega * t) + (alpha * Mathf.Pow(t, 2) / 2));
	}

// ------------------------------------------------- LAB #6 -------------------------------------------------

	public static float calculateAccelerationFromThrust(float thrust, float comMass) {
		return thrust / comMass;
	}
}
