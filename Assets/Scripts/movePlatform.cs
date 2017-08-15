using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Moves a platform along x and z axis from starting position
 * Moves evenly along one axis, meaning that if it goes 5 units in x direction, it will go -5 units as well
 */
public class movePlatform : MonoBehaviour {

	public float xSpeed = 0.1F;
	public float zSpeed = 0.0F;
	public float xDirectionMax = 5.0F;
	public float zDirectionMax = 0.0F;

	private Vector3 startPosition;

	// Use this for initialization
	void Start () {
		startPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if(Mathf.Abs(startPosition.x - transform.position.x) > xDirectionMax)
		{
			xSpeed = xSpeed * (-1);
		} 
		if(Mathf.Abs(startPosition.z - transform.position.z) > zDirectionMax)
		{
			zSpeed = zSpeed * (-1);
		} 

		transform.Translate (new Vector3 (xSpeed, 0, zSpeed));
	}
}
