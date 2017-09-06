using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FallOutOfWorld : MonoBehaviour {

	public bool resetPlayer = true;
	public bool resetLevel = false;

	public Vector3 playerResetPosition = new Vector3(0, 0, 0);

	void OnTriggerEnter(Collider col)
	{
		// Teleport back to start
		if(col.gameObject.tag == "Player")
		{
			if(resetPlayer)
				col.transform.position = playerResetPosition;
			if(resetLevel)
				SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
		}
	}

}
