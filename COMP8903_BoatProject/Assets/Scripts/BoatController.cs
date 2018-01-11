using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatController : MonoBehaviour {

	private Vector3 movementVector;
	private CharacterController controller;

	//temporary speed variable
	public float movementSpeed;

	// Use this for initialization
	void Start () {
		movementVector = new Vector3(0, 0, 0);
		controller = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {
		movementVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
		movementVector = transform.TransformDirection(movementVector);

		movementVector.x *= movementSpeed;
		movementVector.z *= movementSpeed;

		controller.Move(movementVector * Time.deltaTime);
	}
}
