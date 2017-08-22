using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Possible types:
 * -"branch"
 * -"ivy"
 */
public class PickUpScript : MonoBehaviour {
	
	public string type = "branch";
	public int amount = 1;
	public float rotateSpeed = 5.0F;

	private GlobalVariablesScript gvs;
	private int tbIncreased;

	// Use this for initialization
	void Start () {
		GameObject gvsGameObj = GameObject.FindGameObjectWithTag ("GlobalVariables");
		if (gvsGameObj != null)
		{
			gvs = gvsGameObj.GetComponent<GlobalVariablesScript>();
		}
		else
		{
			Debug.LogError ("GVS NOT FOUND");
			this.enabled = false;
		}
		//gvs = (GlobalVariablesScript)GameObject.FindGameObjectWithTag ("GlobalVariables");
	}
	
	// Update is called once per frame
	void Update () {
		transform.RotateAround (transform.position, Vector3.up, rotateSpeed);
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.name == "MyPlayer" && type == "branch")
			gvs.bigBranchSeeds += amount;
		else if (other.gameObject.name == "MyPlayer" && type == "ivy")
			gvs.bigIvySeeds += amount;
		else if (other.gameObject.name == "MyMiniPlayer" && type == "branch")
			gvs.smolBranchSeeds += amount;
		else if (other.gameObject.name == "MyMiniPlayer" && type == "ivy")
			gvs.smolIvySeeds += amount;


		if(other.gameObject.name == "MyPlayer" || other.gameObject.name == "MyMiniPlayer")
			Destroy (this.gameObject);
	}
}
