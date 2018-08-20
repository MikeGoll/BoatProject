using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*------------------------------------------------------------------------------------------------------------------
-- FUNCTION: CanvasController
--
-- DATE: January 11, 2018
--
-- DESIGNER:   Michael Goll
--
-- PROGRAMMER: Michael Goll
--
-- NOTES:
-- Controls the initializing, updating and input handling for the HUD's values.
----------------------------------------------------------------------------------------------------------------------*/
public class CanvasController : MonoBehaviour {

	//Text references for updating
	public Text textTitle, textMoi, textMass, textH, textMH, posXText, posYText, dimText, totInertia;
	//canvas reference
	public Canvas canvas;
	//GameObject reference to the DataController object. Used to grab a reference to the DataController class.
	public GameObject dco;
	//DataController class reference. Used to obtain values to display.
	private Lab1 dc;
	//Float array of calculated values to display to the user.
	private float[] values;
	//boolean flag to represent whether or not the "COM" section is displayed and change the UI accordingly.
	private bool showCom;

/*------------------------------------------------------------------------------------------------------------------
-- FUNCTION: Start
--
-- DATE: January 11, 2018
--
-- DESIGNER:   Michael Goll
--
-- PROGRAMMER: Michael Goll
--
-- NOTES:
-- Initializes the default text of the UI.
-- Initializes the array of values to default calculated values.
----------------------------------------------------------------------------------------------------------------------*/
	void Start () {
		textTitle.text = "Hull";
		dc = dco.GetComponent<Lab1>();
		values = dc.getBoatFloats();
	}
	
/*------------------------------------------------------------------------------------------------------------------
-- FUNCTION: Update
--
-- DATE: January 11, 2018
--
-- DESIGNER:   Michael Goll
--
-- PROGRAMMER: Michael Goll
--
-- NOTES:
-- Handles the user input and calls the UI to refresh its text.
----------------------------------------------------------------------------------------------------------------------*/
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

/*------------------------------------------------------------------------------------------------------------------
-- FUNCTION: setText
--
-- DATE: January 11, 2018
--
-- DESIGNER:   Michael Goll
--
-- PROGRAMMER: Michael Goll
--
-- NOTES:
-- Updates the various text values with their appropriate values to display.
----------------------------------------------------------------------------------------------------------------------*/
	private void setText(float[] temp) {
		textMass.text = "Mass: " + temp[0] + " kg";
		textMoi.text = "MoI: " + temp[1].ToString();
		textH.text = "H: " + temp[2].ToString();
		textMH.text = "MH: " + temp[3].ToString();
		posXText.text = "X: " + temp[4];
		posYText.text = "Z: " + temp[5];
		dimText.text = temp[6] + "m , " + temp[7] + "m";
		totInertia.text = "TI: " + temp[8];
	}
}
