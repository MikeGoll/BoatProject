using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataController : MonoBehaviour {

	//GameObject references for the boat, pilot and cannon
	public GameObject b, p, c;
	public Canvas canvas;
	private CanvasController cController;

	private float[] boatFloats, pilotFloats, cannonFloats, comFloats;
	//masses of the objects
	public float mass_boat, mass_pilot, mass_cannon, mass_com;
	//moments of inertia of the objects
	public float moi_boat, moi_pilot, moi_cannon, moi_com;
	//rotation point of the objects
	public float mh_boat, mh_pilot, mh_cannon, mh_com;
	//inertia totals of the objects
	public float it_boat, it_pilot, it_cannon, it_com;
	public float comX, comZ;
	//h of the objects
	public float h_boat, h_pilot, h_cannon;
	private Pilot pilot;
	private Boat boat;
	private Cannon cannon;

	private int arraySize;

	public Material comPoint;

	// Use this for initialization
	void Start () {
		pilot = p.GetComponent<Pilot>();
		boat = b.GetComponent<Boat>();
		cannon = c.GetComponent<Cannon>();
		cController = canvas.GetComponent<CanvasController>();
		arraySize = 9;

		boatFloats = new float[arraySize];
		pilotFloats = new float[arraySize];
		cannonFloats = new float[arraySize];
		comFloats = new float[arraySize];
	}
	
	// Update is called once per frame
	void Update () {
		mass_boat = boat.getMass();
		mass_pilot = pilot.getMass();
		mass_cannon = cannon.getMass();
		mass_com = PhysicsCalculator.calculateCombined(mass_boat, mass_pilot, mass_cannon);

		boatFloats[0] = mass_boat;
		pilotFloats[0] = mass_pilot;
		cannonFloats[0] = mass_cannon;
		comFloats[0] = mass_com;

		loadBoatFloats();
		loadPilotFloats();
		loadCannonFloats();
		
		calculateMomentOfInertia();
		calculateComValues();
		calculateH();
		calculateMH();
		calculateInertiaTotals();

		comPoint.SetVector("_COMPosition", new Vector3(comX, 0, comZ));
	}

	private void loadBoatFloats() {
		boatFloats[4] = boat.getX();
		boatFloats[5] = boat.getZ();
		boatFloats[6] = boat.getXDim();
		boatFloats[7] = boat.getZDim();
	}

	private void loadCannonFloats() {
		cannonFloats[4] = cannon.getX();
		cannonFloats[5] = cannon.getZ();
		cannonFloats[6] = cannon.getXDim();
		cannonFloats[7] = cannon.getZDim();
	}

	private void loadPilotFloats() {
		pilotFloats[4] = pilot.getX();
		pilotFloats[5] = pilot.getZ();
		pilotFloats[6] = pilot.getXDim();
		pilotFloats[7] = pilot.getZDim();
	}

	private void calculateMomentOfInertia() {
		moi_boat = PhysicsCalculator.calculateMOI(boat.getMass(), boat.getXDim(), boat.getZDim());
		moi_pilot = PhysicsCalculator.calculateMOI(pilot.getMass(), pilot.getXDim(), pilot.getZDim());
		moi_cannon = PhysicsCalculator.calculateMOI(cannon.getMass(), cannon.getXDim(), cannon.getZDim());
		moi_com = PhysicsCalculator.calculateCombined(moi_boat, moi_pilot, moi_cannon);

		boatFloats[1] = moi_boat;
		pilotFloats[1] = moi_pilot;
		cannonFloats[1] = moi_cannon;
		comFloats[1] = moi_com;
	}

	private void calculateComValues() {
		comX = PhysicsCalculator.calculateComX(mass_boat, boat.getX(), mass_pilot, pilot.getX(), mass_cannon, cannon.getX(), mass_com);
		comZ = PhysicsCalculator.calculateComZ(mass_boat, boat.getZ(), mass_pilot, pilot.getZ(), mass_cannon, cannon.getZ(), mass_com);

		comFloats[4] = comX;
		comFloats[5] = comZ;
	}

	private void calculateH() {
		h_boat = PhysicsCalculator.calculateRotationPoint(boat.getX(), boat.getZ(), comX, comZ);
		h_pilot = PhysicsCalculator.calculateRotationPoint(pilot.getX(), pilot.getZ(), comX, comZ);
		h_cannon = PhysicsCalculator.calculateRotationPoint(cannon.getX(), cannon.getZ(), comX, comZ);

		boatFloats[2] = h_boat;
		pilotFloats[2] = h_pilot;
		cannonFloats[2] = h_cannon;
	}

	private void calculateMH() {
		mh_boat = PhysicsCalculator.calculateMH(h_boat, mass_boat);
		mh_pilot = PhysicsCalculator.calculateMH(h_pilot, mass_pilot);
		mh_cannon = PhysicsCalculator.calculateMH(h_cannon, mass_cannon);
		mh_com = PhysicsCalculator.calculateCombined(mh_boat, mh_pilot, mh_cannon);

		boatFloats[3] = mh_boat;
		pilotFloats[3] = mh_pilot;
		cannonFloats[3] = mh_cannon;
		comFloats[3] = mh_com;
	}

	private void calculateInertiaTotals() {
		it_boat = PhysicsCalculator.calculateInertiaTotal(moi_boat, mh_boat);
		it_pilot = PhysicsCalculator.calculateInertiaTotal(moi_pilot, mh_pilot);
		it_cannon = PhysicsCalculator.calculateInertiaTotal(moi_cannon, mh_cannon);
		it_com = PhysicsCalculator.calculateCombined(it_boat, it_pilot, it_cannon);

		boatFloats[8] = it_boat;
		pilotFloats[8] = it_pilot;
		cannonFloats[8] = it_cannon;
		comFloats[8] = it_com;
	}

	public float[] getBoatFloats() {
		return boatFloats;
	}

	public float[] getPilotFloats() {
		return pilotFloats;
	}

	public float[] getCannonFloats() {
		return cannonFloats;
	}

	public float[] getComFloats() {
		return comFloats;
	}
}
