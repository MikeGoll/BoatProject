using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*------------------------------------------------------------------------------------------------------------------
-- FUNCTION: Spawner
--
-- DATE: January 22, 2018
--
-- DESIGNER:   Michael Goll
--
-- PROGRAMMER: Michael Goll
--
-- NOTES:
-- Spawns a prefab GameObject a set number of times.
-- Used to spawn the markers relative to position (0, 0, 0)
----------------------------------------------------------------------------------------------------------------------*/

public class Spawner : MonoBehaviour {

	public GameObject marker;
	public static GameObject gunball;
	private const int MARKERNUM = 38;
	private const int SPACER = -3;

	void Start () {
		for (int x = -4; x < MARKERNUM; x++) {
			GameObject temp = Object.Instantiate(marker, new Vector3(SPACER, 0, x), Quaternion.Euler(0, -90, 0));
			temp.GetComponentInChildren<TextMesh>().text = "" + x;
		}
	}
}
