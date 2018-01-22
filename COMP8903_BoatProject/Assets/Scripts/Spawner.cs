using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour {

	public GameObject marker;
	private const int MARKERNUM = 38;

	// Use this for initialization
	void Start () {
		for (int x = -4; x < MARKERNUM; x++) {
			GameObject temp = Object.Instantiate(marker, new Vector3(-2, 0, x), Quaternion.identity);
			temp.GetComponentInChildren<TextMesh>().text = "" + x;
		}
	}
}
