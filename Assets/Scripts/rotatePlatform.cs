using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Simple script that rotates an object around itself
 */
public class rotatePlatform : MonoBehaviour {

	public float rotateSpeed = 1.0F;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(0, rotateSpeed, 0);
	}
}
