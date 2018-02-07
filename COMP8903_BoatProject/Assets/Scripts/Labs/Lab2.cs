using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataControllerLab2 : MonoBehaviour {

	public int numUpdates;

	public float distance, velocity, acceleration, dragConstant, time;
	public bool useDrag;

	private bool moving;

	private float totalTime, t;
	private float newD, currentD, newV, oldV, dt;

	public GameObject boat;

	public Text timeText, frameText, velocityText, distanceText;

	// Use this for initialization
	void Start () {
		numUpdates = 0;
		
		currentD = 0;
		oldV = velocity;
		moving = true;

		if (useDrag) {
			dragConstant = PhysicsCalculator.calculateDragConstant(acceleration, velocity);
		}

		distance = PhysicsCalculator.calculateDistance(0, acceleration, velocity, time);
		dt = PhysicsCalculator.calculateDragTime(dragConstant, distance, velocity);
	}

	void FixedUpdate() {
		
		//calculate what frame it is
		t = numUpdates * Time.deltaTime;

		if (moving) {
			if (useDrag) {
				newD = PhysicsCalculator.calculateDistanceDrag(currentD, oldV, Time.fixedDeltaTime, dragConstant);
				newV = PhysicsCalculator.calculateVelocityDrag(oldV, acceleration, Time.fixedDeltaTime, dragConstant);

				if (t >= dt) {
					moving = false;
				}

			} else {
				newD = PhysicsCalculator.calculateDistance(currentD, acceleration, oldV, Time.fixedDeltaTime);
				newV = PhysicsCalculator.calculateVelocity(oldV, acceleration, Time.fixedDeltaTime);

				if (t >= time) {
					moving = false;
				}
			}

			timeText.text = "Time: " + t + " seconds";
			frameText.text = "Frame: " + numUpdates + " frames";
			velocityText.text = "Velocity: " + oldV + " m/s";
			distanceText.text = "Distance: " + currentD + " m";
		}

		//need to recheck if moving before going to next frame.
		//moving could be changed to false before it reaches here so need to recheck.
		if (moving) {
			boat.transform.Translate(Vector3.forward * oldV * Time.deltaTime);
			currentD = newD;
			oldV = newV;
			numUpdates++;
		}
	}
}
