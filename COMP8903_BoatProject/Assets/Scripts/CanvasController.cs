using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour {

	public Text textTitle, textMoi, textMass, textH, textMH, posXText, posYText, dimText, totInertia;
	public Canvas canvas;
	public GameObject dco;
	private DataController dc;
	private float[] values;

	private bool showCom;

	// Use this for initialization
	void Start () {
		textTitle.text = "Hull";
		dc = dco.GetComponent<DataController>();
		values = dc.getBoatFloats();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Alpha1)) {
			showCom = false;
			textTitle.text = "Hull";
			values = dc.getBoatFloats();
		}

		if (Input.GetKeyDown(KeyCode.Alpha2)) {
			showCom = false;
			textTitle.text = "Pilot";
			values = dc.getPilotFloats();
		}

		if (Input.GetKeyDown(KeyCode.Alpha3)) {
			showCom = false;
			textTitle.text = "Cannon";
			values = dc.getCannonFloats();
		}

		if (Input.GetKeyDown(KeyCode.Alpha4)) {
			showCom = true;
			textTitle.text = "COM";
			values = dc.getComFloats();
		}

		if (Input.GetKeyDown(KeyCode.Space)) {
			canvas.enabled = !canvas.enabled;
		}
		
		setText(values);
	}

	private void setText(float[] temp) {
		textMass.text = "Mass: " + temp[0] + " kg";
		textMoi.text = "MoI: " + temp[1].ToString();
		textH.text = "H: " + temp[2].ToString();
		textMH.text = "MH: " + temp[3].ToString();
		posXText.text = "X: " + temp[4];
		posYText.text = "Y: " + temp[5];
		dimText.text = temp[6] + "m , " + temp[7] + "m";
		totInertia.text = "TI: " + temp[8];
		
	}
}
