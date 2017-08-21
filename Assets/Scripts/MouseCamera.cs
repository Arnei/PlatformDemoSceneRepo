using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCamera : MonoBehaviour {

	/**
	public GameObject target;
	public float rotateSpeed = 5;
	Vector3 offset;

	// Use this for initialization
	void Start () {
		Cursor.lockState = CursorLockMode.Locked;

		//offset = target.transform.position - transform.position;
		offset = transform.position - target.transform.position;
	}

	// Update is called once per frame
	void LateUpdate () {

		float horizontal = Input.GetAxis ("Mouse X") * rotateSpeed;
		float vertical = Input.GetAxis ("Mouse Y") * rotateSpeed;

		// Set Camera behind Player via offset
		transform.position = target.transform.position + offset;
		// Rotate Camera around Player
		transform.RotateAround(target.transform.position, target.transform.up, horizontal * rotateSpeed);
		//Rotate Camera around other axis
		transform.RotateAround(target.transform.position, -target.transform.right, vertical * rotateSpeed);
		// Get new offset from the rotation
		offset = transform.position - target.transform.position;



		//Rotate Camera around other axis
		//transform.RotateAround(transform.position, -transform.right, vertical * rotateSpeed);

		// If trying to go forward, ajust player forward to camera forward
		if ((Input.GetKey("w") || Input.GetMouseButtonDown(0)))
		{
			float desiredAngle = Camera.main.transform.eulerAngles.y;
			target.transform.rotation = Quaternion.Euler (0, desiredAngle, 0);
		}


	}
	*/

	public Transform CameraTarget;
	private float x = 0.0f;
	private float y = 0.0f;

	private int mouseXSpeedMod = 5;
	private int mouseYSpeedMod = 5;

	public float MaxViewDistance = 15f;
	public float MinViewDistance = 1f;
	public int ZoomRate = 20;
	private int lerpRate = 5;
	private float distance = 3f;
	private float desireDistance;
	private float correctedDistance;
	private float currentDistance;

	public float cameraTargetHeight = 1.0f;

	//checks if first person mode is on
	private bool click = false;
	//stores cameras distance from player
	private float curDist = 0;

	// Use this for initialization
	void Start () {
		x = transform.eulerAngles.x;
		y = transform.eulerAngles.y;
		currentDistance = distance;
		desireDistance = distance;
		correctedDistance = distance;
	}

	// Update is called once per frame
	void LateUpdate () {
		//if (Input.GetMouseButton (1)) {/*0 mouse btn left, 1 mouse btn right*/
			x += Input.GetAxis("Mouse X") * mouseXSpeedMod;
			y += Input.GetAxis("Mouse Y") * mouseYSpeedMod;
		//}
		/**
		// Blocks rotating around x axis while moving
		// Supposedly used with "else if" with the above if statement
		if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
		{
			float targetRotationAngle = CameraTarget.eulerAngles.y;
			float cameraRotationAngle = transform.eulerAngles.y;
			x = Mathf.LerpAngle(cameraRotationAngle,targetRotationAngle, lerpRate * Time.deltaTime);
		}
		*/
		y = ClampAngle (y, -15, 25);
		Quaternion rotation = Quaternion.Euler (y,x,0);

		desireDistance -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * ZoomRate * Mathf.Abs(desireDistance);
		desireDistance = Mathf.Clamp (desireDistance, MinViewDistance, MaxViewDistance);
		correctedDistance = desireDistance;

		Vector3 position = CameraTarget.position - (rotation * Vector3.forward * desireDistance);

		RaycastHit collisionHit;
		Vector3 cameraTargetPosition = new Vector3 (CameraTarget.position.x, CameraTarget.position.y + cameraTargetHeight, CameraTarget.position.z);

		bool isCorrected = false;
		if (Physics.Linecast (cameraTargetPosition, position, out collisionHit)) {
			position = collisionHit.point;
			correctedDistance = Vector3.Distance(cameraTargetPosition,position);
			isCorrected = true;
		}
			

		currentDistance = !isCorrected || correctedDistance > currentDistance ? Mathf.Lerp(currentDistance,correctedDistance,Time.deltaTime * ZoomRate) : correctedDistance;

		position = CameraTarget.position - (rotation * Vector3.forward * currentDistance + new Vector3 (0, -cameraTargetHeight, 0));

		transform.rotation = rotation;
		transform.position = position;

		//CameraTarget.rotation = rotation;

		float cameraX = transform.rotation.x;
		//checks if right mouse button is pushed
		//if(Input.GetMouseButton(1))
		//{
			//sets CHARACTERS x rotation to match cameras x rotation
			CameraTarget.eulerAngles = new Vector3(cameraX,transform.eulerAngles.y,transform.eulerAngles.z);
		//}
		//checks if middle mouse button is pushed down
		if(Input.GetMouseButtonDown(2))
		{
			//if middle mouse button is pressed 1st time set click to true and camera in front of player and save cameras position before mmb.
			//if mmb is pressed again set camera back to it's position before we clicked mmb 1st time and set click to false
			if(click == false)
			{
				click = true;
				curDist = distance;
				distance = distance - distance - 1;
			}
			else
			{
				distance = curDist;
				click = false;
			}
		}

	}

	private static float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360) {
			angle += 360;      
		}
		if (angle > 360) {
			angle -= 360;      
		}
		return Mathf.Clamp (angle,min,max);
	}
}
