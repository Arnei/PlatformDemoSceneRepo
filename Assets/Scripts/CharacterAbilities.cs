using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Handles character abilities
 * Would make sense to include this in CharacterController Script, 
 * but seperate for clarity.
 */
public class CharacterAbilities : MonoBehaviour {
	
	public Transform branch;
	public Transform ivy;

	public float ivyRaycastDistance = 5.0F;

	private CharacterController controller;		

	// Use this for initialization
	void Start () {
		controller = GetComponent<CharacterController>();	
	}
	
	// Update is called once per frame
	void Update () {
		// Create branch on "1"
		// Make sure branch starts in ground by having the player be grounded
		if(Input.GetKeyDown(KeyCode.Alpha1) && controller.isGrounded)
		{
			growBranch ();
		}

		if(Input.GetKeyDown(KeyCode.Alpha2))
		{
			growIvy ();
		}
	}




	// Instantiate a branch below the player
	void growBranch()
	{
		Vector3 startPosition = transform.position;

		// Alter startPosition to start at the bottom of the player ('s mesh)
		MeshFilter mf = GetComponent<MeshFilter>();
		Vector3 objSize = mf.sharedMesh.bounds.size;
		Vector3 objScale = transform.localScale;
		float objHeight = objSize.y * objScale.y; //(*anyparentobject.transform.localScale.y);
		//float objWidth = objSize.x * objScale.x; //(*parent.transform.localScale.x);
		startPosition.y = startPosition.y - (objHeight / 2.0F);

		// Goal here is to have player and branch have the same forward
		Quaternion startRotation = transform.rotation;

		Instantiate (branch, startPosition, startRotation);
	}

	void growIvy()
	{
		RaycastHit hit;
		if(Physics.Raycast(transform.position, transform.forward, out hit, ivyRaycastDistance)){


			Vector3 startPos = hit.point;
			Quaternion startRot = hit.collider.gameObject.transform.rotation;

			Vector3 endPos = hit.point + new Vector3 (0, 2.0F, 0);

			Transform newIvy = Instantiate (ivy, startPos, startRot);

			float verySmallNumber = 0.01F;
			newIvy.transform.position = newIvy.transform.position + (newIvy.transform.forward * verySmallNumber);

			GrowIvy growIvyScript = newIvy.GetComponent<GrowIvy> ();
			growIvyScript.setEndPos (endPos);
			growIvyScript.setRaycastHit (hit.collider.gameObject.GetComponent<MeshFilter>().sharedMesh.bounds);

		}
	}


}
