using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandleWalls : MonoBehaviour {

	public GameObject player;

	public float minimumDistance = 0.1F;
	public float lerpSpeed = 2.0F;

	private float maxDistance;
	private bool noTrigger = false;
	private float rayMaxDistance = 15.0F;

	// Use this for initialization
	void Start () {
		maxDistance = Vector3.Distance (player.transform.position, transform.position);
	}
	
	// Update is called once per frame
	void Update () {
		
		RaycastHit hit;
		if(Physics.Raycast(transform.parent.position, (transform.position - transform.parent.position), out hit, rayMaxDistance))
		{
			Debug.DrawLine (transform.parent.position, hit.point, Color.red);
			if(hit.distance < maxDistance)
				transform.position = hit.point;

		}


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
	}


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
	*/
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

}
