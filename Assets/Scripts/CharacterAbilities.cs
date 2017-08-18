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
	public Transform playerSmol;
	public Transform target;
	public CharacterSwitching characterSwitchingScript;

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
		if(Input.GetKeyDown(KeyCode.Alpha1) && controller.isGrounded)
		{
			growBranch ();
		}

		if(Input.GetKeyDown(KeyCode.Alpha2))
		{
			growIvy ();
		}

		if(Input.GetKeyDown(KeyCode.Alpha3) && characterSwitchingScript.together)
		{
			throwPlayerSmol ();
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

			if (hit.collider.gameObject.name == "GrowIvyPlane")
				return;
			Debug.Log ("Hitname: " + hit.collider.gameObject.name);

			Vector3 startPos = hit.point;
			Quaternion startRot = hit.collider.gameObject.transform.rotation;

			Quaternion tempRot = hit.collider.gameObject.transform.rotation;
			hit.collider.gameObject.transform.rotation = Quaternion.identity;
			Bounds hitBounds = hit.collider.gameObject.GetComponent<MeshRenderer> ().bounds;
			startPos.y = hit.collider.gameObject.transform.position.y - hitBounds.extents.y;

			Transform newIvy = Instantiate (ivy, startPos, startRot);

			float verySmallNumber = 0.01F;
			newIvy.transform.position = newIvy.transform.position + (newIvy.transform.forward * verySmallNumber);

			GrowIvy growIvyScript = newIvy.GetComponent<GrowIvy> ();


			growIvyScript.setRaycastHit (hitBounds);
			hit.collider.gameObject.transform.rotation = tempRot;
		}
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

}
