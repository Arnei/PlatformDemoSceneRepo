using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Stops the camera from going through walls,
 * by raycasting from player to camera, and adjusting the camera position on this trajectory if necessary.
 * Script to be attached to the camera
 */
public class CameraHandleWalls : MonoBehaviour {

	public GameObject player;

	public float lerpBackSpeed = 5.0F;		// Speed at which the camera moves back to normal

	private float maxDistance;				// Maximum possible distance from player. Based on position in scene.
	private float rayMaxDistance;			// Maximum possible distance the ray can go

	private Vector3 playerPos;
	private Vector3 cameraPos;
	private Vector3 goalPos;


	// Use this for initialization
	void Start () {
		playerPos = player.transform.position;
		cameraPos = transform.position;

		maxDistance = Vector3.Distance (playerPos, cameraPos);
		rayMaxDistance = maxDistance; //+ 1.0F;


	}

	// Update is called once per frame
	void LateUpdate () {
		playerPos = player.transform.position;
		cameraPos = transform.position;
		goalPos = transform.position;

		// Send a raycast in camera direction. If there is a hit, position the camera at the hit point, so that it does not go through walls etc.
		Ray ray = new Ray(playerPos, (cameraPos - playerPos));
		RaycastHit hit;
		if(Physics.Raycast(ray, out hit, rayMaxDistance))
		{
			Debug.DrawLine (cameraPos, hit.point, Color.red);
			if(hit.distance < maxDistance)
				goalPos = hit.point;

		}
		// Reset to maxDistance if there is no obstacle
		else if(Vector3.Distance (playerPos, cameraPos) < maxDistance)
		{
			Vector3 reset = ray.GetPoint (maxDistance);
			goalPos = Vector3.MoveTowards (cameraPos, reset, lerpBackSpeed * Time.deltaTime);
		}

		transform.position = goalPos;
	}

	/**
	// Update is called once per frame
	void Update () {

		// Send a raycast in camera direction. If there is a hit, position the camera at the hit point, so that it does not go through walls etc.
		Ray ray = new Ray(transform.parent.position, (transform.position - transform.parent.position));
		RaycastHit hit;
		if(Physics.Raycast(ray, out hit, rayMaxDistance))
		{
			Debug.DrawLine (transform.parent.position, hit.point, Color.red);
			if(hit.distance < maxDistance)
				transform.position = hit.point;

		}
		// Reset to maxDistance if there is no obstacle
		else if(Vector3.Distance (player.transform.position, transform.position) < maxDistance)
		{
			Vector3 reset = ray.GetPoint (maxDistance);
			transform.position = Vector3.MoveTowards (transform.position, reset, lerpBackSpeed * Time.deltaTime);
		}
	}
	*/


}
