using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class gun : MonoBehaviour {

	public GameObject bulletPrefab;
	public Transform spawnObject;	

	void Awake() {
//		bool focusModeSet = CameraDevice.Instance.SetFocusMode( 
//			CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			GameObject go = Instantiate (bulletPrefab, spawnObject.position, spawnObject.rotation) as GameObject;
			//go.transform.parent = transform; // added to make bullet a child of the image target.
			go.GetComponent<Rigidbody> ().AddForce (transform.forward * 100, ForceMode.VelocityChange);
		}
	}
}
