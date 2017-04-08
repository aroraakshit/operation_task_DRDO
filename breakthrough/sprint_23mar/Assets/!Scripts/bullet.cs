/**
 * ATTACHED TO: bullet prefab
 * 
 * TASKS OF THIS SCRIPT:
 * 1) destroy bullet when it hits the enemy (or collides with anything).
 * 
 * TO DO:
 * 1) It needs to disappear as soon as it crosses boundaries of plane (can only be done after scaling up of image target since the shooter may not always shoot from inside the plane)
 * 2) [perfection] bullet look.
 * 3) we could make bullet disappear after a certain range, because that's how bullets really work. One needs to be in proximity of enemy in order to significantly harm them.
**/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

	void OnCollisionEnter (Collision collision){
		Destroy (gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
