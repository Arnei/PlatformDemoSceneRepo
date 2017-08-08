using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBoxManager : MonoBehaviour {

	public GameObject textBox;

	public Text theText;

	public TextAsset textFile;
	public string[] textLines;

	public int currentLine = 0;
	public int endAtLine = -1;

	public my_character_controller player;

	public bool isActive;
	public bool stopPlayerMovement;

	// Use this for initialization
	void Start ()
	{
		player = FindObjectOfType<my_character_controller> ();

		if(textFile != null)
		{
			textLines = (textFile.text.Split ('\n'));
		}

		if(endAtLine == -1)
		{
			endAtLine = textLines.Length - 1;
		}

		if(isActive)
		{
			EnableTextBox ();
		} else {
			DisableTextBox ();
		}
	}

	// Update is called once per frame
	void Update () 
	{
		theText.text = textLines [currentLine];

		if(Input.GetKeyDown(KeyCode.Return))
		{
			currentLine += 1;
		}

		if(currentLine > endAtLine)
		{
			DisableTextBox ();
			currentLine -= 1;
		}
	}

	public void EnableTextBox()
	{
		textBox.SetActive (true);
		isActive = true;

		if(stopPlayerMovement)
		{
			player.canMoveText = false;
		}
	}

	public void DisableTextBox()
	{
		textBox.SetActive (false);
		isActive = false;

		player.canMoveText = true;
	}

	public void ReloadScript(TextAsset theText)
	{
		if(theText != null)
		{
			textLines = new string[1];
			textLines = (theText.text.Split ('\n'));

			if(endAtLine == -1)
			{
				endAtLine = textLines.Length - 1;
			}
		}
	}
}
