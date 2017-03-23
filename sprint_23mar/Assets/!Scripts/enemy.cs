using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

	void OnCollisionEnter (Collision collision){
		Debug.Log ("collided!");
		Destroy (collision.gameObject);
		Destroy (gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
