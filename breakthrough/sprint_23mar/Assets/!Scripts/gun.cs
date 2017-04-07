using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gun : MonoBehaviour {

	public GameObject canvas;
	public GameObject bullet;
	public Transform spawnObject;

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {

		//shooting
		if (Input.GetMouseButtonDown (0)) {
			GameObject go = Instantiate (bullet, spawnObject.position, spawnObject.rotation) as GameObject;
			go.GetComponent<Rigidbody> ().AddForce (transform.forward * 100, ForceMode.VelocityChange);
			canvas.GetComponent<canvas_text> ().update_canvas ("bullet");
			GetComponent<AudioSource> ().Play ();
		}

//		To check collilders in gun's target
//		RaycastHit hit;
//		float theDistance;
//		Vector3 forward = transform.TransformDirection (Vector3.forward) * 10;
//		Debug.DrawRay (transform.position, forward, Color.green);
//		if (Physics.Raycast (transform.position, (forward), out hit)) {
//			theDistance = hit.distance;
//			print (theDistance + " " + hit.collider.gameObject.name);
//		}
	
	}
}
