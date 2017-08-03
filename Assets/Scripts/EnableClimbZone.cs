using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableClimbZone : MonoBehaviour {

	public my_character_controller player;

	void OnTriggerEnter(Collider col){
		if (col.gameObject.tag == "Player")
			player.movementType = "ClimbMovement";
	}

	/**
	void OnTriggerStay(Collider col){
		if (col.gameObject.tag == "Player"  && Input.GetButton("Jump")){
			Vector3 dir_vector = new Vector3(transform.position.x - col.transform.position.x, 0, transform.position.z - col.transform.position.z);
			print (dir_vector);
			player.controller.Move ( dir_vector * Time.deltaTime);
		}
	}
	*/

	void OnTriggerExit(Collider col){
		if (col.gameObject.tag == "Player")
			player.movementType = "NormalMovement";
	}
}
