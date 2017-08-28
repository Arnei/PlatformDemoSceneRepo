using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadScene : MonoBehaviour {

	void Update()
	{
		if(Input.GetKey(KeyCode.R))
			SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}

}
