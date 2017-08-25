using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour {

	public string sceneName;

	public float colorLerpTime = 3.0F;
	public float colorLerpSmoothness = 0.2F;

	public bool playerBigHasEntered;
	public bool playerSmolHasEntered;
	private bool flowersGrey;

	private Renderer myRenderer;
	private Color startColor;
	private Color endColor;

	// Use this for initialization
	void Start () {
		playerBigHasEntered = false;
		playerSmolHasEntered = false;
		flowersGrey = true;
		myRenderer = transform.GetComponent<Renderer> ();
		startColor = myRenderer.material.color;
		endColor = new Color (2.0F, 2.0F, 2.0F);
	}
	
	// Update is called once per frame
	void Update () {
		if(playerBigHasEntered && playerSmolHasEntered)
		{
			if (flowersGrey)
				StartCoroutine (LerpColor (myRenderer, startColor, endColor));
			if (myRenderer.material.color == endColor)
				flowersGrey = false;
			if (Input.GetKeyDown (KeyCode.Return))
				SceneManager.LoadScene (sceneName);
		}
	}


	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.name == "MyPlayer")
			playerBigHasEntered = true;
		if (other.gameObject.name == "MyMiniPlayer")
			playerSmolHasEntered = true;
	}

	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.name == "MyPlayer")
			playerBigHasEntered = false;
		if (other.gameObject.name == "MyMiniPlayer")
			playerSmolHasEntered = false;
	}



	IEnumerator LerpColor(Renderer rend, Color start, Color end)
	{
		float progress = 0; //This float will serve as the 3rd parameter of the lerp function.
		float increment = colorLerpSmoothness/colorLerpTime; //The amount of change to apply.
		while(progress < 1)
		{
			rend.material.color = Color.Lerp(start, end, progress);
			progress += increment;
			yield return new WaitForSeconds(colorLerpSmoothness);
		}
		if (progress >= 1)
			flowersGrey = false;
		//return true;
	}

}
