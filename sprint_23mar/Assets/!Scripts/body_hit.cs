using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class body_hit : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter (Collision collision){
		if (collision.collider.name == "bullet(Clone)") {
			gameObject.GetComponentInParent<enemy2> ().decreaseHealth ("injured");
		}
	}

}
