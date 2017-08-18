using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSwitching : MonoBehaviour {

	public GameObject PlayerBig;
	public GameObject PlayerSmol;
	public Camera PlayerBigCamera;
	public Camera PlayerSmolCamera;

	public float distanceFromBig = 5.0F;

	public bool together = true;
	private bool focusOnSmol = false;

	private my_character_controller PlayerBigScript;
	private my_character_controller PlayerSmolScript;

	private CharacterController PlayerBigController;
	private CharacterController PlayerSmolController;

	private Vector3 anchorPos;

	// Use this for initialization
	void Start () {
		PlayerBigScript = PlayerBig.GetComponent<my_character_controller> ();
		PlayerSmolScript = PlayerSmol.GetComponent<my_character_controller> ();
		PlayerBigController = PlayerBig.GetComponent<CharacterController> ();
		PlayerSmolController = PlayerSmol.GetComponent<CharacterController> ();
		anchorPos = PlayerBig.transform.position - PlayerSmol.transform.position;
	}
	
	// Update is called once per frame
	void Update () {


		if(Input.GetKeyDown(KeyCode.Y) && together)
		{
			// Detach
			detach();
			together = false;
			// Switch to Smol
			switchToSmol();
			focusOnSmol = true;
		}
		else if(Input.GetKeyDown(KeyCode.Y) && !together)
		{
			// If close reattach and switch to Big
			if(PlayersAreClose())
			{
				retach();
				together = true;
				switchToBig ();
				focusOnSmol = false;
			}
			// Else nothing
		}

		if(Input.GetKeyDown(KeyCode.X) && !together)
		{			
			// If Smol, switch to Big
			if(focusOnSmol)
			{
				switchToBig ();
				focusOnSmol = false;
			}
			// If Big, switch to Smol
			else 
			{
				switchToSmol ();
				focusOnSmol = true;
			}


		}
		else if(Input.GetKeyDown(KeyCode.X) && together)
		{
			// Detach
			detach();
			together = false;
			// Switch to Smol
			switchToSmol();
			focusOnSmol = true;
		}
	}

	bool PlayersAreClose()
	{
		float PSmolX = PlayerSmol.transform.position.x;
		float PSmolY = PlayerSmol.transform.position.y;
		float PSmolZ = PlayerSmol.transform.position.z;
		float PBigX = PlayerBig.transform.position.x;
		float PBigY = PlayerBig.transform.position.y;
		float PBigZ = PlayerBig.transform.position.z;

		if(    PSmolX < PBigX + PlayerBig.transform.localScale.x + distanceFromBig
			&& PSmolX > PBigX - PlayerBig.transform.localScale.x - distanceFromBig
			&& PSmolY < PBigY + PlayerBig.transform.localScale.y + distanceFromBig
			&& PSmolY > PBigY - PlayerBig.transform.localScale.y - distanceFromBig
			&& PSmolZ < PBigZ + PlayerBig.transform.localScale.z + distanceFromBig
			&& PSmolZ > PBigZ - PlayerBig.transform.localScale.z - distanceFromBig)
		{
			return true;
		}
		return false;
	}

	public void detach()
	{
		PlayerSmol.transform.parent = null;
	}

	void retach()
	{
		PlayerSmol.transform.parent = PlayerBig.transform;
		PlayerSmol.transform.position = PlayerBig.transform.position - anchorPos;
	}

	// Make necessary enables and disables
	// CharacterController need to be disabled, else bugs can happen (like invisible colliders remaining in the scense)
	void switchToSmol()
	{
		PlayerBigCamera.enabled = false;
		PlayerBigScript.canMoveStart = false;
		PlayerBigController.enabled = false;

		PlayerSmolCamera.enabled = true;
		PlayerSmolScript.canMoveStart = true;
		PlayerSmolController.enabled = true;
	}

	void switchToBig()
	{
		PlayerSmolCamera.enabled = false;
		PlayerSmolScript.canMoveStart = false;
		PlayerSmolController.enabled = false;

		PlayerBigCamera.enabled = true;
		PlayerBigScript.canMoveStart = true;
		PlayerBigController.enabled = true;
	}

}
