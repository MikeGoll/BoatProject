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
	private const int SPACER = -2;

	void Start () {
		for (int x = -4; x < MARKERNUM; x++) {
			GameObject temp = Object.Instantiate(marker, new Vector3(SPACER, 0, x), Quaternion.identity);
			temp.GetComponentInChildren<TextMesh>().text = "" + x;
		}
	}

	public static void spawnGunball(int x, int y, int z) {
		Object.Instantiate(gunball, new Vector3(x, y, z), Quaternion.identity);
	}
}
