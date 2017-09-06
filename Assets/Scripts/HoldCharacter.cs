using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Used by moving/rotating platforms to "hold" player
// By making the player a child of itself. 
public class HoldCharacter : MonoBehaviour {

	private List<Collider> attachedColliders;

	void Start(){
		attachedColliders = new List<Collider> ();
	}

	// Only Top Level Objects can become attached
	// This is supposed to prevent hiearchy breaking
	void OnTriggerEnter(Collider col){
		if(col.transform.parent == null)
		{
			col.transform.parent = gameObject.transform;
			attachedColliders.Add (col);
		}

	}

	// To know if an object should be dettached
	// Look through the list
	void OnTriggerExit(Collider col){
		foreach(Collider collider in attachedColliders)
		{
			if(collider == col)
				col.transform.parent = null;
		}

	}
}
