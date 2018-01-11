using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataController : MonoBehaviour {

	//GameObject references for the boat, pilot and cannon
	public GameObject b, p, c;
	public Canvas canvas;
	private CanvasController cController;
	private PhysicsCalculator physicsCalculator;
	//masses of the objects
	public float hullMass, pilotMass, cannonMass, commMass;
	public float boatMomentOfI, pilotMomentOfI, cannonMomentOfI, comMomentOfI;
	public float boatMH, pilotMH, cannonMH, comMH;
	public float boatIT, pilotIT, cannonIT, comIT;
	public float comX, comZ;
	public float boatH, pilotH, cannonH;
	private Pilot pilot;
	private Boat boat;
	private Cannon cannon;

	// Use this for initialization
	void Start () {
		pilot = p.GetComponent<Pilot>();
		boat = b.GetComponent<Boat>();
		cannon = c.GetComponent<Cannon>();

		cController = canvas.GetComponent<CanvasController>();
		physicsCalculator = GetComponent<PhysicsCalculator>();
	}
	
	// Update is called once per frame
	void Update () {
		hullMass = boat.getMass();
		pilotMass = pilot.getMass();
		cannonMass = cannon.getMass();
		commMass = physicsCalculator.calculateCombined(hullMass, pilotMass, cannonMass);

		cController.updateComText(commMass);
		
		calculateMomentOfInertia();
		calculateComValues();
		calculateH();
		calculateMH();
		calculateInertiaTotals();
	}

	private void calculateMomentOfInertia() {
		boatMomentOfI = physicsCalculator.calculateMOI(boat.getMass(), boat.getXDim(), boat.getZDim());
		pilotMomentOfI = physicsCalculator.calculateMOI(pilot.getMass(), pilot.getXDim(), pilot.getZDim());
		cannonMomentOfI = physicsCalculator.calculateMOI(cannon.getMass(), cannon.getXDim(), cannon.getZDim());
		comMomentOfI = physicsCalculator.calculateCombined(boatMomentOfI, pilotMomentOfI, cannonMomentOfI);
	}

	private void calculateComValues() {
		comX = physicsCalculator.calculateComX(hullMass, boat.getX(), pilotMass, pilot.getX(), cannonMass, cannon.getX(), commMass);
		comZ = physicsCalculator.calculateComZ(hullMass, boat.getZ(), pilotMass, pilot.getZ(), cannonMass, cannon.getZ(), commMass);
	}

	private void calculateH() {
		boatH = physicsCalculator.calculateRotationPoint(boat.getX(), boat.getZ(), comX, comZ);
		pilotH = physicsCalculator.calculateRotationPoint(pilot.getX(), pilot.getZ(), comX, comZ);
		cannonH = physicsCalculator.calculateRotationPoint(cannon.getX(), cannon.getZ(), comX, comZ);
	}

	private void calculateMH() {
		boatMH = physicsCalculator.calculateMH(boatH, hullMass);
		pilotMH = physicsCalculator.calculateMH(pilotH, pilotMass);
		cannonMH = physicsCalculator.calculateMH(cannonH, cannonMass);
	}

	private void calculateInertiaTotals() {
		boatIT = physicsCalculator.calculateInertiaTotal(boatMomentOfI, boatMH);
		pilotIT = physicsCalculator.calculateInertiaTotal(pilotMomentOfI, pilotMH);
		cannonIT = physicsCalculator.calculateInertiaTotal(cannonMomentOfI, cannonMH);
		comIT = physicsCalculator.calculateCombined(boatIT, pilotIT, cannonIT);
	}
}
