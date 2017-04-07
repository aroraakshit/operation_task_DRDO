//6 April 3:54 PM 
//not be using this functionality for now. Because there are gravity problems with different weights/ regid bodies / colliders.
//One hit anywhere = decrease health by 10; we can come back to this functionality later;

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
		Debug.Log ("body hit");
		if (collision.collider.name == "bullet(Clone)") {
			gameObject.GetComponentInParent<enemy2> ().decreaseHealth ("injured");
		}
	}

}
