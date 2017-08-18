using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowIvy : MonoBehaviour {

	public float growSpeed = 1.0F;


	private Bounds hitWallBounds;
	private Vector3 rescale;

	// Use this for initialization
	void Start () {
		Transform growIvyPlane = transform.Find("GrowIvyPlane");


		float distanceTopBottom = hitWallBounds.size.y;
		Debug.Log ("HitWallBoundssize: " + hitWallBounds.size.x + " " + hitWallBounds.size.y + " " + hitWallBounds.size.z);
		//float sizeY = growIvyPlane.GetComponent<MeshFilter> ().sharedMesh.bounds.size.z;
		//Debug.Log ("PLaneSizes: " + growIvyPlane.GetComponent<MeshFilter> ().sharedMesh.bounds.size.x + " " + growIvyPlane.GetComponent<MeshFilter> ().sharedMesh.bounds.size.y + " " + growIvyPlane.GetComponent<MeshFilter> ().sharedMesh.bounds.size.z);

		Quaternion tempRot = transform.rotation;
		transform.rotation = Quaternion.identity;
		float sizeY = growIvyPlane.GetComponent<MeshRenderer> ().bounds.size.y;
		Debug.Log ("PLaneSizes: " + growIvyPlane.GetComponent<MeshRenderer> ().bounds.size.x + " " + growIvyPlane.GetComponent<MeshRenderer> ().bounds.size.y + " " + growIvyPlane.GetComponent<MeshRenderer> ().bounds.size.z);
		transform.rotation = tempRot;

		rescale = transform.localScale;
		Debug.Log ("Rescale before: " + rescale);
		Debug.Log ("Distance: " + distanceTopBottom + " localsacleY: " + transform.localScale.y + " SizeY: " + sizeY );
		rescale.y = distanceTopBottom * transform.localScale.y / sizeY;
		float wtf = (float)distanceTopBottom * (float)transform.localScale.y / (float)sizeY;
		print ("WTIF: " + wtf);
		Debug.Log("Rescale after: " + rescale);

	}
	
	// Update is called once per frame
	void Update () {


		// Gradually changes the scale each frame
		transform.localScale = Vector3.MoveTowards (transform.localScale, rescale, (growSpeed * Time.deltaTime));
	}


	public void setRaycastHit(Bounds hit)
	{
		hitWallBounds = hit;
	}
}
