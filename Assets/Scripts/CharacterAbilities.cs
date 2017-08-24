using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * Handles character abilities
 * Would make sense to include this in CharacterController Script, 
 * but seperate for clarity.
 */
public class CharacterAbilities : MonoBehaviour {
	
	public Transform branch;
	public Transform ivy;
	public Transform playerSmol;
	public Transform target;
	public CharacterSwitching characterSwitchingScript;
	public GlobalVariablesScript globalVariablesScript;
	public Image redXImage;

	public float maxLength = 10.0F;		// Maximum Length the branch can have (in unity units)

	public float ivyRaycastDistance = 5.0F;

	public float firingAngle = 45.0f;
	public float gravity = 9.8f;

	private CharacterController controller;		

	// Use this for initialization
	void Start () {
		controller = GetComponent<CharacterController>();	

		characterSwitchingScript = characterSwitchingScript.GetComponent<CharacterSwitching> ();
	}
	
	// Update is called once per frame
	void Update () {
		// Create branch on "1"
		// Make sure branch starts in ground by having the player be grounded
		if(Input.GetKeyDown(KeyCode.Alpha1) && controller.isGrounded && globalVariablesScript.bigBranchSeeds > 0)
		{
			if(growBranch ())
				globalVariablesScript.bigBranchSeeds -= 1;
			else
				StartCoroutine (showX());
		}
		else if(Input.GetKeyDown(KeyCode.Alpha1) && (!controller.isGrounded || globalVariablesScript.bigBranchSeeds <= 0))
		{
			StartCoroutine (showX());
		}

		// Create Ivy on "2"
		if(Input.GetKeyDown(KeyCode.Alpha2) && globalVariablesScript.bigIvySeeds > 0)
		{
			if(growIvy ())
				globalVariablesScript.bigIvySeeds -= 1;
			else
				StartCoroutine (showX());
		}
		else if(Input.GetKeyDown(KeyCode.Alpha2) && globalVariablesScript.bigIvySeeds <= 0)
		{
			StartCoroutine (showX());
		}

		// Throw Smol on "3"
		if(Input.GetKeyDown(KeyCode.Alpha3) && characterSwitchingScript.together)
		{
			throwPlayerSmol ();
		}
	}




	// Instantiate a branch below the player
	bool growBranch()
	{
		Vector3 startPosition = transform.position;

		// Move startPosition to the ground the player is standing on
		RaycastHit hit;
		if(Physics.Raycast(transform.position, Vector3.down, out hit))
		{
			startPosition.y = hit.point.y;
		}
		// If we don't anything something is srsly wrong
		else
		{
			Debug.LogError ("NO GROUND");
			return false;
		}


		// Goal here is to have player and branch have the same forward
		Quaternion startRotation = transform.rotation;

		Transform newBranch = Instantiate (branch, startPosition, startRotation);

		// Moves the branch downwards, so that it does not appear IN the player, but under it.
		// WIP. Needs optimization, as the calculations are not precise. Might be better to use something other than mesh.sharedMesh (maybe MeshRenderer)
		Transform branchObject = newBranch.transform.Find("BranchObject");
		MeshFilter bO_mf = branchObject.GetComponent<MeshFilter>();
		Vector3 bO_objSize = bO_mf.sharedMesh.bounds.size;
		Vector3 bO_objScale = newBranch.transform.localScale;
		float bO_objHeight = bO_objSize.y * bO_objScale.y; 
		newBranch.transform.position = new Vector3(newBranch.transform.position.x, newBranch.transform.position.y - (bO_objHeight / 4.0F), newBranch.transform.position.z);	// Literally hacked the constant here

		// Check if we wanna grow shorter than maxLength
		float newMaxLength = checkForGoal (newBranch.transform, maxLength, bO_objHeight);
		// Can we even grow here?
		if (newMaxLength >= maxLength)
		{
			Debug.Log ("MaxLength reached, overshot by: " + newMaxLength);
			Destroy (newBranch.gameObject);
			return false;
		}

		// Notifiy the branch of how long it should be
		Branch branchScript = newBranch.GetComponent<Branch> ();
		branchScript.setMaxLength (newMaxLength);

		return true;
	}




	bool growIvy()
	{
		RaycastHit hit;
		if (Physics.Raycast (transform.position, transform.forward, out hit, ivyRaycastDistance)) {

			if (hit.collider.gameObject.name == "GrowIvyPlane")
				return false;
			Debug.Log ("Hitname: " + hit.collider.gameObject.name);

			Vector3 startPos = hit.point;
			Quaternion startRot = hit.collider.gameObject.transform.rotation;

			Quaternion tempRot = hit.collider.gameObject.transform.rotation;
			hit.collider.gameObject.transform.rotation = Quaternion.identity;
			Bounds hitBounds = hit.collider.gameObject.GetComponent<MeshRenderer> ().bounds;
			startPos.y = hit.collider.gameObject.transform.position.y - hitBounds.extents.y;

			Transform newIvy = Instantiate (ivy, startPos, startRot);

			// Face in correct direction
			newIvy.transform.forward = hit.normal;

			// Put a little in fron so as not to overlap with the object it is "growing" on
			float verySmallNumber = 0.01F;
			newIvy.transform.position = newIvy.transform.position + (newIvy.transform.forward * verySmallNumber);


			GrowIvy growIvyScript = newIvy.GetComponent<GrowIvy> ();

			growIvyScript.setRaycastHit (hitBounds);
			hit.collider.gameObject.transform.rotation = tempRot;
			return true;
		} else
			return false;
	}

	void throwPlayerSmol()
	{
		// Detach
		characterSwitchingScript.detach();
		characterSwitchingScript.together = false;
		StartCoroutine(SimulateProjectile());
	}


	// I steal dis
	// https://forum.unity3d.com/threads/throw-an-object-along-a-parabola.158855/
	IEnumerator SimulateProjectile()
	{
		// Short delay added before Projectile is thrown
		//yield return new WaitForSeconds(1.5f);

		// Move projectile to the position of throwing object + add some offset if needed.
		//playerSmol.position = transform.position + new Vector3(0, 1.0f, 0);

		// Calculate distance to target
		float target_Distance = Vector3.Distance(playerSmol.position, target.position);

		// Calculate the velocity needed to throw the object to the target at specified angle.
		float projectile_Velocity = target_Distance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity);

		// Extract the X  Y componenent of the velocity
		float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
		float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);

		// Calculate flight time.
		float flightDuration = target_Distance / Vx;

		// Rotate projectile to face the target.
		//Quaternion lookingAt = Quaternion.LookRotation(target.position - playerSmol.position);
		//Vector3 eulerAngles = new Vector3 (playerSmol.rotation.eulerAngles.x, lookingAt.eulerAngles.y, playerSmol.rotation.eulerAngles.z);
		//playerSmol.rotation = Quaternion.Euler(eulerAngles.x, eulerAngles.y, eulerAngles.z);


		Quaternion originalRot = playerSmol.rotation;
		Quaternion toTargetRot = Quaternion.LookRotation(target.position - playerSmol.position);

		float elapse_time = 0;

		while (elapse_time < flightDuration)
		{
			playerSmol.rotation = toTargetRot;
			playerSmol.Translate(0, (Vy - (gravity * elapse_time)) * Time.deltaTime, Vx * Time.deltaTime);
			playerSmol.rotation = originalRot;

			elapse_time += Time.deltaTime;

			yield return null;
		}
	}   

	// Display Red X when an action is not possible (For whatever reason)
	IEnumerator showX()
	{
		Color c;
		for (int i = 1; i > 0; i--)
		{
			redXImage.enabled = true;
			yield return new WaitForSeconds (0.5F);
		}
		for (float f = 1.0F; f > 0.0F; f -= 0.1F)
		{
			c = redXImage.color;
			c.a = f;
			redXImage.color = c;
			yield return new WaitForSeconds (0.1F);
		}
		c = redXImage.color;
		c.a = 1.0F;
		redXImage.color = c;
		redXImage.enabled = false;
	}

	// Do 3 Raycasts forward (middle, low, high), find the closest object they hit that is not a branch
	// maxAcceptableDistance: The biggest distance an object can be away before it is ignored
	// objHeight: Height of the branch. Used to calc starting position for low/high raycasts
	// Return: Distance to closest object that is not a branch
	float checkForGoal(Transform startTransform, float maxAcceptableDistance, float objHeight)
	{
		
		RaycastHit[] hits_middle, hits_low, hits_high;

		hits_middle = Physics.RaycastAll (startTransform.position, startTransform.forward, maxAcceptableDistance);
		//Debug.DrawLine (transform.position, (transform.position + transform.forward * maxLength), Color.red);
		maxAcceptableDistance = shortestDistanceInHits(hits_middle, maxAcceptableDistance);

		Vector3 low = new Vector3 (startTransform.position.x, startTransform.position.y - (objHeight / 4.0F) + 0.01F, startTransform.position.z);
		hits_low = Physics.RaycastAll(low, startTransform.forward, maxAcceptableDistance);
		//Debug.DrawLine (low, (low + transform.forward * maxLength), Color.red);
		maxAcceptableDistance = shortestDistanceInHits(hits_low, maxAcceptableDistance);


		Vector3 high = new Vector3 (startTransform.position.x, startTransform.position.y + (objHeight / 4.0F) - 0.01F, startTransform.position.z);	// Small correction so as to not miss the ground
		hits_high = Physics.RaycastAll(high, startTransform.forward, maxAcceptableDistance);
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
