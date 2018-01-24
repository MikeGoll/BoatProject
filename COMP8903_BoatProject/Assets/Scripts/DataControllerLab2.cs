using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataControllerLab2 : MonoBehaviour {

	private int numUpdates;

	public float distance, velocity, acceleration, dragConstant, time;
	public bool useDrag;

	private bool moving;

	private float startTime, totalTime;
	private float newD, currentD, newV, oldV;

	public GameObject boat;

	private bool timeLogged;

	// Use this for initialization
	void Start () {
		numUpdates = 0;
		
		currentD = 0;
		oldV = velocity;

		startTime = Time.time;
		moving = true;
		timeLogged = false;

		if (useDrag) {
			dragConstant = PhysicsCalculator.calculateDragConstant(acceleration, velocity);
		}

		distance = PhysicsCalculator.calculateDistance(0, acceleration, velocity, time);
	}

	void FixedUpdate() {
		numUpdates++;

		if (moving) {
			if (useDrag) {
				//float si, float a, float init_velocity, float time, float k
				newD = PhysicsCalculator.calculateDistanceDrag(currentD, acceleration, oldV, Time.fixedDeltaTime, dragConstant);
				//float vi, float a, float time, float k
				newV = PhysicsCalculator.calculateVelocityDrag(oldV, acceleration, Time.fixedDeltaTime, dragConstant);
			} else {
				//float si, float a, float init_velocity, float time
				newD = PhysicsCalculator.calculateDistance(currentD, acceleration, oldV, Time.fixedDeltaTime);
				newV = PhysicsCalculator.calculateVelocity(oldV, acceleration, Time.fixedDeltaTime);
			}
		}

		if (oldV <= 0 || currentD >= distance) {
			moving = false;
		}

		currentD = newD;
		oldV = newV;

		if (moving) {
			boat.transform.Translate(Vector3.forward * oldV * Time.deltaTime);
		} else {
			if (!timeLogged) {
				timeLogged = true;
				totalTime = Time.timeSinceLevelLoad - startTime;
				Debug.Log("Total Time: " + totalTime + " seconds.");
				Debug.Log("Total Updates: " + numUpdates + " updates.");
			}
		}
	}
}
