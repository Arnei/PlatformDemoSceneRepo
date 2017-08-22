using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalVariablesScript : MonoBehaviour {


	public CharacterSwitching characterSwitchingScript;

	public Text branchSeedsText;
	public Text ivySeedsText;

	public int bigBranchSeeds = 0;
	public int bigIvySeeds = 0;
	public int smolBranchSeeds = 0;
	public int smolIvySeeds = 0;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if(characterSwitchingScript.focusOnSmol)
		{
			branchSeedsText.text = smolBranchSeeds.ToString();
			ivySeedsText.text = smolIvySeeds.ToString();
		}
		else
		{
			branchSeedsText.text = bigBranchSeeds.ToString();
			ivySeedsText.text = bigIvySeeds.ToString();
		}
	}

	public void handOver()
	{
		for(; 0 > smolBranchSeeds; smolBranchSeeds--)
			bigBranchSeeds++;
		for(; 0 > smolIvySeeds; smolIvySeeds--)
			bigIvySeeds++;	
	}
}
