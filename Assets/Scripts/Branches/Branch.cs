using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Initializes a branch object growing in a direction
 * To be attached to the Parent of BranchObject (See Branch prefab)
 */
public class Branch : MonoBehaviour {

	public float maxLength = 10.0F;		// Maximum Length the branch can have (in unity units)
	public float growSpeed = 0.1F;		// Manipulates the speed at which the branch scales		

	private Vector3 rescale;			// Holds the final scale we want to achieve

	// Use this for initialization
	void Start () {
		// Moves the branch downwards, so that it does not appear IN the player, but under it.
		// WIP. Needs optimization, as the calculations are not precise. Might be better to use something other than mesh.sharedMesh (maybe MeshRenderer)
		Transform branchObject = transform.Find("BranchObject");
		MeshFilter mf = branchObject.GetComponent<MeshFilter>();
		Vector3 objSize = mf.sharedMesh.bounds.size;
		Vector3 objScale = transform.localScale;
		float objHeight = objSize.y * objScale.y; 
		transform.position = new Vector3(transform.position.x, transform.position.y - (objHeight / 3.0F), transform.position.z);	// Literally hacked the constant here

		// Check if we wanna grow shorter than maxLength
		Debug.Log ("Old MaxLength: " + maxLength);
		maxLength = checkForGoal (maxLength, objHeight);
		Debug.Log ("New MaxLength: " + maxLength);

		// MeshRenderer.bounds returns the size of the axis-aligned bounding box.
		// This is then used to calculate the corresponding scale value to maxLength.
		// Unfortunately, the AABB changes its size with rotation so
		// in order to get the standard size, we briefly rotate to identity (0,0,0)
		Quaternion tempRot = transform.rotation;
		transform.rotation = Quaternion.identity;
		float size = branchObject.GetComponent<MeshRenderer> ().bounds.size.z;
		transform.rotation = tempRot;
		Debug.Log ("Size: " + size);
		rescale = transform.localScale;
		Debug.Log ("Rescale:" + rescale);
		rescale.z = maxLength * rescale.z / size;		// The goal scale
	}

	// Update is called once per frame
	void Update () {
		if(transform.localScale.z < maxLength)
		{
			// Gradually changes the scale each frame
			transform.localScale = Vector3.MoveTowards (transform.localScale, rescale, (growSpeed * Time.deltaTime));
		}
	}

	// Do 3 Raycasts forward (middle, low, high), find the closest object they hit that is not a branch
	// maxAcceptableDistance: The biggest distance an object can be away before it is ignored
	// objHeight: Height of the branch. Used to calc starting position for low/high raycasts
	// Return: Distance to closest object that is not a branch
	float checkForGoal(float maxAcceptableDistance, float objHeight)
	{
		RaycastHit[] hits_middle, hits_low, hits_high;

		hits_middle = Physics.RaycastAll (transform.position, transform.forward, maxAcceptableDistance);
		//Debug.DrawLine (transform.position, (transform.position + transform.forward * maxLength), Color.red);
		maxAcceptableDistance = shortestDistanceInHits(hits_middle, maxAcceptableDistance);

		Vector3 low = new Vector3 (transform.position.x, transform.position.y - (objHeight / 4.0F), transform.position.z);
		hits_low = Physics.RaycastAll(low, transform.forward, maxAcceptableDistance);
		//Debug.DrawLine (low, (low + transform.forward * maxLength), Color.red);
		maxAcceptableDistance = shortestDistanceInHits(hits_low, maxAcceptableDistance);


		Vector3 high = new Vector3 (transform.position.x, transform.position.y + (objHeight / 4.0F), transform.position.z);
		hits_high = Physics.RaycastAll(high, transform.forward, maxAcceptableDistance);
		//Debug.DrawLine (high, (high + transform.forward * maxLength), Color.red);
		maxAcceptableDistance = shortestDistanceInHits(hits_high, maxAcceptableDistance);


		return maxAcceptableDistance;
	}

	// Helper function to checkForGoal. Iterates over the hit results.
	// hits: The hits from the respective raycast.
	// maxAcceptableDistance: Current maximum distance
	// Return: New maximum distance
	float shortestDistanceInHits(RaycastHit[] hits, float maxAcceptableDistance)
	{
		for (int i=0; i < hits.Length; i++)
		{
			//Debug.Log ("Hits: " + hits[i].collider.gameObject.name);
			// Ignore other Branches. Because they are child objects, the raycasts will collide with them,
			// effectivly stopping growth.
			if (hits [i].collider.gameObject.name == "BranchObject")
				continue;
			if (hits [i].distance < maxAcceptableDistance)
			{
				maxAcceptableDistance = hits [i].distance;
				//Debug.Log ("New shortest distance: " + hits [i].collider.gameObject.name);
			}
				
		}
		return maxAcceptableDistance;
	}

}
