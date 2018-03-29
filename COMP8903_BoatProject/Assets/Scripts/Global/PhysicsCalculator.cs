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

	public static float calculateXThrust(float force, float angle) {
		return force * Mathf.Sin(toRads(angle));
	}

	public static float calculateZThrust(float force, float angle) {
		return force * Mathf.Cos(toRads(angle));
	}

	public static float toRads(float degrees) {
		return ((degrees * Mathf.PI) / 180);
	}

	public static Vector3 calculateCrossProd(Vector3 v1, Vector3 v2) {
		Vector3 resultV;

		resultV.x = v1.y * v2.z - v1.z * v2.y;
		resultV.y = v1.x * v2.z - v1.z * v2.x;
		resultV.z = v1.x * v2.y - v1.y * v2.x;

		return resultV;
	}

	public static float calculateAngularAcceleration(float torque, float comMass) {
		return torque / comMass;
	}


// ------------------------------------------------- LAB #7 -------------------------------------------------

	public static float calculateTau(float totalMass, float dragCoefficient) {
		return (totalMass / dragCoefficient);  
	}

	public static float calculteTerminalVelocity(float thrust, float dragCoefficient) {
		return (thrust / dragCoefficient);
	}

	public static float calculateAccelerationWithDrag(float thrust, float dragCoefficient, float vnext, float mass) {
		return ((thrust - dragCoefficient * vnext) / mass);
	}

	public static float calculateVelocityWithDrag(float vmax, float dragCoefficient, float vi, float time, float mass) {
		return vmax - Mathf.Pow(EXP, (-dragCoefficient) * time / mass) * (vmax - vi);
	}

	public static float calculatePositionWithDrag(float prex, float force, float dragConst, float time, float com, float vi) {
		return prex + (force / dragConst * time) + ((force - dragConst * vi) / dragConst * com / dragConst * (Mathf.Pow(EXP, (-dragConst) * time / com) - 1) );
	}

// ------------------------------------------------- LAB #8 -------------------------------------------------

	public static float calculateZPositionWithWind(float zi, float vi, float tau, float time, float windVel, float gamma, float windCoefficient, float dragCoefficient) {
		float emp = Mathf.Pow(EXP, -time / tau);
		float temp = windCoefficient * windVel * Mathf.Cos(gamma) / dragCoefficient;
		return zi + vi * tau * (1 - emp) + temp * tau * (1 - emp) - temp * time;
	}

	public static float calculateZVelocityWithWind(float time, float tau, float vi, float windCoefficient, float windVel, float gamma, float dragCoefficient) {
		float emp = Mathf.Pow(EXP, -time / tau);
		return emp * vi + (emp - 1) * windCoefficient * windVel * Mathf.Cos(gamma) / dragCoefficient;
	}

	public static float calculateYPositionWithWind(float yi, float vi, float tau, float time) {
		float emp = Mathf.Pow(EXP, -time / tau);
		return yi + vi * tau * (1 - emp) + GRAVITY * Mathf.Pow(tau, 2) * (1 - emp) - GRAVITY * tau * time;
	}

	public static float calculateYVelocityWithWind(float dragCoefficient, float time, float mass, float vi) {
		float mg = mass * GRAVITY;
		return 1 / dragCoefficient * (Mathf.Pow(EXP, -dragCoefficient * time / mass) * (dragCoefficient * vi + mg) - mg);
	}

	public static float calculateXPositionWithWind(float xi, float vi, float tau, float time, float windCoefficient, float windVel, float dragCoefficient, float gamma) {
		float emp = Mathf.Pow(EXP, -time / tau);
		float temp = windCoefficient * windVel * Mathf.Sin(gamma) / dragCoefficient;
		return xi + vi * tau * (1 - emp) + temp * tau * (1 - emp) - temp * time;
	}

	public static float calculateXVelocityWithWind(float time, float tau, float vi, float windCoefficient, float windVel, float gamma, float dragCoefficient) {
		float emp = Mathf.Pow(EXP, -time / tau);
		return emp * vi + (emp - 1) * windCoefficient * windVel * Mathf.Sin(gamma) / dragCoefficient;
	}

// ------------------------------------------------- LAB #9 -------------------------------------------------

	public static float calculateJImpulse(float vr, float e, float gunballMass, float targetMass) {
		return -vr * (e + 1) * targetMass * gunballMass / (targetMass + gunballMass);
	}

	public static float calculateRecoilVelocity(float jImpulse, float mass, float vi) {
		return jImpulse / mass + vi;
	}

	public static float calculateMomentum(float mass, float vi) {
		return mass * vi;
	}

// ------------------------------------------------- LAB #10 -------------------------------------------------

// ------------------------------------------------- LAB #11 -------------------------------------------------

	public static float calculateLowerCaseR(float P, float R) {
		return P - R;
	}

	public static float calculateMOI(float mass) {
		return (1 / (6 * mass) * (mass * mass));
	}

	public static float calculateOmegaFinal(float omegaI, Vector3 r, Vector3 jN, float I) {
		// return omegaI + calculateCrossProd(r, jN) / I;
		return 0.0f;
	}

	public static float calculateJImpulseWithRot(float vr, float e, float m1, float m2, float lCR1, float lCR2) {
		return -vr * (e + 1) * (1 / (1 / m1 + 1 / m2 + lCR1 + lCR2));
	}

	public static Vector3 calculateSubtractVector(Vector3 v1, Vector3 v2) {
		Vector3 temp;

		temp.x = v1.x - v2.x;
		temp.y = v1.y - v2.y;
		temp.z = v1.z - v2.z;

		return temp;
	}

	public static float calculateJImpulse(float vr, float e, float mass1, float mass2, Vector3 n, Vector3 r1, Vector3 r2, float moi1, float moi2) {
		float jMass1 = Vector3.Dot(n, Vector3.Cross((Vector3.Cross(r1, n) / moi1), r1));
		float jMass2 = Vector3.Dot(n, Vector3.Cross((Vector3.Cross(r2, n) / moi2), r2));

		Debug.Log("Jmass1, Jmass2: " + jMass1 + ", " + jMass2);

		return -vr * (e + 1) * (1 / ((1 / mass1) + (1 / mass2) + jMass1 + jMass2));
	}

	public static Vector3 calculateAngularMomentum(Vector3 r1, Vector3 r2, Vector3 p1, Vector3 p2) {
		return Vector3.Cross(r1, p1) + Vector3.Cross(r2, p2);
	}
}
