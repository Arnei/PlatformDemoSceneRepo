using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Used by moving/rotating platforms to "hold" player
// By making the player a child of itself. Has drawbacks
public class HoldCharacter : MonoBehaviour {

	void OnTriggerEnter(Collider col){
		col.transform.parent = gameObject.transform;
	}

	void OnTriggerExit(Collider col){
		col.transform.parent = null;
	}
}
