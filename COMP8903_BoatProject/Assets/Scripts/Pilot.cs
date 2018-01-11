using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pilot : MonoBehaviour {

	private float mass;
	private float x_position;
	private float z_position;
	private float x_dimension, z_dimension;
	
	private Vector3 position;
	

	// Use this for initialization
	void Start () {
		mass = 60;
	}
	
	// Update is called once per frame
	void Update () {
		x_position = transform.position.x;
		z_position = transform.position.z;
		x_dimension = 1;
		z_dimension = 1;
	}

	public float getX() {
		return x_position;
	}

	public float getZ() {
		return z_position;
	}

	public float getXDim() {
		return x_dimension;
	}

	public float getZDim() {
		return z_dimension;
	}

	public float getMass() {
		return mass;
	}
}
