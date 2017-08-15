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


	// Use this for initialization
	void Start () {
		maxDistance = Vector3.Distance (player.transform.position, transform.position);
		rayMaxDistance = maxDistance; //+ 1.0F;
	}
	
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



}

// DEAD CODE. To be removed eventually


/**
		if(noTrigger)
		{
			bool done = moveAway ();
			if (done)
			{
				noTrigger = false;
			}
		}
		*/

/**
	void OnTriggerEnter(Collider other)
	{
		moveCloser ();
	}

	void OnTriggerStay(Collider other)
	{
		moveCloser ();
	}

	void OnTriggerExit(Collider other)
	{
		noTrigger = true;
	}

	void moveCloser()
	{
		// Direction from player to camera
		Vector3 direction = transform.position - player.transform.position;
		Vector3 minDistPos = player.transform.position + (direction.normalized * minimumDistance);

		transform.position = Vector3.Lerp (transform.position, minDistPos, lerpSpeed * Time.deltaTime);
	}

	bool moveAway()
	{
		Vector3 direction = transform.position - player.transform.position;
		Vector3 maxDistPos = player.transform.position + (direction.normalized * maxDistance);

		transform.position = Vector3.Lerp (transform.position, maxDistPos, lerpSpeed * Time.deltaTime);

		if (maxDistance >= Vector3.Distance (player.transform.position, transform.position))
			return false;
		else
			return true;
	}
	*/