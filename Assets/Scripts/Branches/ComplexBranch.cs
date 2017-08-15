using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Attempt at growing a branch by instantiating multiple little branches, one after another. Way more difficult than i thought
 */
public class ComplexBranch : MonoBehaviour {

	/**
	//public float maxLength = 10.0F;
	//public float minLength = 1.0F;
	public float segmentMaxLength = 1.0F;
	public int segmentMaxAmount = 10;
	public float growSpeed = 0.1F;

	private bool doneGrowing;
	private int segmentCount;

	public GameObject branch;
	private GameObject currentlyGrowingBranch;

	// Use this for initialization
	void Start () {
		doneGrowing = false;
		segmentCount = 1;

		// Moves itself downwards so as not to appear IN the player
		Transform currentlyGrowingBranch = transform.Find("Single Branch");
		Transform branchObject = currentlyGrowingBranch.Find("BranchObject");
		MeshFilter mf = branchObject.GetComponent<MeshFilter>();
		Vector3 objSize = mf.sharedMesh.bounds.size;
		Vector3 objScale = transform.localScale;
		float objHeight = objSize.y * objScale.y; 
		transform.position = new Vector3(transform.position.x, transform.position.y - (objHeight / 4.0F), transform.position.z);	// Literally hacked the constant here
	}

	// Update is called once per frame
	void Update () {
		if (segmentCount >= segmentMaxAmount)
			return;
		if(currentlyGrowingBranch.transform.localScale.z < segmentMaxLength)
		{
			transform.localScale = new Vector3(currentlyGrowingBranch.transform.localScale.x, 
											   currentlyGrowingBranch.transform.localScale.y, 
											   currentlyGrowingBranch.transform.localScale.z + (growSpeed * Time.deltaTime));
		}
		else{
			currentlyGrowingBranch = Instantiate (branch, 
				currentlyGrowingBranch.transform.position + (currentlyGrowingBranch.transform.forward * segmentMaxLength),
				currentlyGrowingBranch.transform.rotation) as GameObject;
			segmentCount += 1;
		}
	}

	*/
}
