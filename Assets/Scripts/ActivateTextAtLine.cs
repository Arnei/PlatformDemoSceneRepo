using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateTextAtLine : MonoBehaviour {

	public TextAsset theText;

	public int startLine = 0;
	public int endLine = -1;

	public TextBoxManager theTextBox;

	public bool requireButtonPress;
	private bool waitForPress;

	public bool destroyWhenActivated;

	private bool oneFrameLater;

	// Use this for initialization
	void Start () {
		theTextBox = FindObjectOfType<TextBoxManager> ();
		oneFrameLater = false;
	}
	
	// Update is called once per frame
	void Update () {

		// To avoid skipping line 1 if both Start and Skip command is Return
		if(oneFrameLater)
		{
			startTextBox ();
			oneFrameLater = false;
		}

		if(waitForPress && 
			Input.GetKeyDown(KeyCode.Return) && 
			!theTextBox.isActive)
		{
			oneFrameLater = true;
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player")
		{
			if(requireButtonPress)
			{
				waitForPress = true;
				return;
			}

			startTextBox ();
		}
	}

	void OnTriggerExit(Collider other)
	{
		if(other.tag == "Player")
		{
			waitForPress = false;
		}
	}

	public void startTextBox()
	{
		theTextBox.currentLine = startLine;
		theTextBox.endAtLine = endLine;
		theTextBox.ReloadScript (theText);
		theTextBox.EnableTextBox ();

		if(destroyWhenActivated)
		{
			Destroy (gameObject);
		}
	}
}
