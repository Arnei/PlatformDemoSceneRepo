using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Have a platform move between two positions
 * See PlatformManager object for details on implementation in the scene
 */
public class MovingPlatforms : MonoBehaviour {

	public Transform movingPlatform;
	public Transform position1;
	public Transform position2;
	private Vector3 newPosition;		// Current goal position (either position1 or position2)
	private string currentState;		// To which position platform is currently going
	public float smooth;			// How fast the platform will move
	public float resetTime;			// After how many seconds the platform will change directions


	// Use this for initialization
	void Start () {
		newPosition = new Vector3 (0, 0, 0);
		currentState = "";
		ChangeTarget ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		// Move to target position
		movingPlatform.position = Vector3.Lerp(movingPlatform.position, newPosition, smooth * Time.deltaTime);
	}

	// Change the direction the platform moves in.
	// Reinvokes itself after "resetTime" seconds
	void ChangeTarget(){
		if(currentState == "Moving To Position 1"){
			currentState = "Moving To Position 2";
			newPosition = position2.position;
		}
		else if(currentState == "Moving To Position 2"){
			currentState = "Moving To Position 1";
			newPosition = position1.position;
		}
		else if(currentState == ""){
			currentState = "Moving To Position 2";
			newPosition = position2.position;
		}
		Invoke ("ChangeTarget", resetTime);
	}

}
