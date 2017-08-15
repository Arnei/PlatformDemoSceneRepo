using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Goes beyond a simple follower camera.
 * Script to be attached to an empty game object, with a camera as a child. 
 * The empty game object will be moved to player position, and follow the player position,
 * but will not follow the players rotation, allowing for more control of the rotation.
 */
public class AlternateCamera : MonoBehaviour {

	public GameObject player;						// Player the EGO will be following

	public float rotateSpeed = 3.0F;				// How fast the camera rotates by hand
	public float autoRotateSpeedDecrease = 10.0F;	// Decreases speed if automatic rotation (if >1)
	public float waitUntilAutoRotate = 1.0F;		// Time to wait until camera readjust itself

	private Camera myCamera;

	private bool justRotatedByHand = false;			// Used by the Timer
	private float tempWaitUntilAutoRotate;			// Used by the Timer

	// Use this for initialization
	void Awake () {
		myCamera = GetComponentInChildren<Camera> ();

		transform.position = player.transform.position;
		transform.rotation = player.transform.rotation;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		// Timer checking if the camera was recently rotated by hand
		if (justRotatedByHand && (Input.GetAxis ("Vertical") != 0.0F))
		{
			tempWaitUntilAutoRotate = tempWaitUntilAutoRotate - Time.deltaTime;
			if (tempWaitUntilAutoRotate <= 0.0F)
				justRotatedByHand = false;
		}

		// Follow Player
		transform.position = player.transform.position;

		// Get Rotation Input
		Vector3 cameraHorizontal;
		if (Input.GetKey (KeyCode.E))
			cameraHorizontal = Vector3.up; 
		else if (Input.GetKey (KeyCode.Q))
			cameraHorizontal = -Vector3.up;
		else
			cameraHorizontal = Vector3.zero;

		// Rotate by hand 
		if(cameraHorizontal != Vector3.zero)
		{
			transform.Rotate (cameraHorizontal * rotateSpeed);
			justRotatedByHand = true;
			tempWaitUntilAutoRotate = waitUntilAutoRotate;
		}
		// Automatically rotate to align rotation with player rotation
		// after a defined time and if the camera wasn't just manually manipulated
		else if(!justRotatedByHand)
		{
			// Calc the angle between the looking direction of the player and this object
			float forwardAngle = Vector3.SignedAngle (myCamera.transform.forward, player.transform.forward, Vector3.up);
			// Rotationspeed Modifier based on angle size (the remaining amount that still needs to be rotated)
			float autoRotateSpeed = Mathf.Abs(forwardAngle / autoRotateSpeedDecrease);
			// Rotate
			transform.Rotate (new Vector3(0, forwardAngle, 0) * Time.deltaTime * autoRotateSpeed);
		}






	}
}
