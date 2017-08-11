using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlternateCamera : MonoBehaviour {

	public GameObject player;

	// Camera Things
	private Camera myCamera;
	public float rotateSpeed = 3.0F;
	public float autoRotateSpeedDecrease = 10.0F;
	public float waitUntilAutoRotate = 1.0F;

	private bool justRotatedByHand = false;
	private float tempWaitUntilAutoRotate;

	// Use this for initialization
	void Awake () {
		myCamera = GetComponentInChildren<Camera> ();

		transform.position = player.transform.position;
		transform.rotation = player.transform.rotation;
	}
	
	// Update is called once per frame
	void LateUpdate () {
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


		if(cameraHorizontal != Vector3.zero)
		{
			transform.Rotate (cameraHorizontal * rotateSpeed);
			justRotatedByHand = true;
			tempWaitUntilAutoRotate = waitUntilAutoRotate;
		}
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
