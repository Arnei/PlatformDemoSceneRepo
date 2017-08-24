using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowIvy : MonoBehaviour {

	public float growSpeed = 1.0F;

	private Transform growIvyPlane;
	private Bounds hitWallBounds;
	private Vector3 rescale;
	private float distanceTopBottom;

	// Use this for initialization
	void Start () {
		growIvyPlane = transform.Find("GrowIvyPlane");


		distanceTopBottom = hitWallBounds.size.y;
		//Debug.Log ("HitWallBoundssize: " + hitWallBounds.size.x + " " + hitWallBounds.size.y + " " + hitWallBounds.size.z);
		//float sizeY = growIvyPlane.GetComponent<MeshFilter> ().sharedMesh.bounds.size.z;
		//Debug.Log ("PLaneSizes: " + growIvyPlane.GetComponent<MeshFilter> ().sharedMesh.bounds.size.x + " " + growIvyPlane.GetComponent<MeshFilter> ().sharedMesh.bounds.size.y + " " + growIvyPlane.GetComponent<MeshFilter> ().sharedMesh.bounds.size.z);

		Quaternion tempRot = transform.rotation;
		transform.rotation = Quaternion.identity;
		float sizeY = growIvyPlane.GetComponent<MeshRenderer> ().bounds.size.y;
		transform.rotation = tempRot;

		rescale = transform.localScale;
		rescale.y = distanceTopBottom * transform.localScale.y / sizeY;

	}
	
	// Update is called once per frame
	void Update () {
		// Gradually changes the scale each frame
		transform.localScale = Vector3.MoveTowards (transform.localScale, rescale, (growSpeed * Time.deltaTime));

		Quaternion tempRot = transform.rotation;
		transform.rotation = Quaternion.identity;
		float sizeX = growIvyPlane.GetComponent<MeshRenderer> ().bounds.size.x;
		float sizeY = growIvyPlane.GetComponent<MeshRenderer> ().bounds.size.y;
		transform.rotation = tempRot;

		//Debug.Log ("SizeY: " + sizeY);
		growIvyPlane.GetComponent<Renderer>().material.mainTextureScale = new Vector2(sizeX / 2, sizeY / 2);		// The /2 are just magic numbers for styling
	}


	public void setRaycastHit(Bounds hit)
	{
		hitWallBounds = hit;
	}
}
