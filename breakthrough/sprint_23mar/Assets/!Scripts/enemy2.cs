using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using UnityEngine.UI;

public class enemy2 : MonoBehaviour{

	public GameObject canvas;
	public int health = 100;
	public GameObject notice;
	public GameObject plan;
	public Transform target;
	public int teleport;

	// Use this for initialization
	void Start () {
		canvas = GameObject.FindGameObjectWithTag ("canvas_text");
		notice = GameObject.FindGameObjectWithTag ("notice");
		plan = GameObject.FindGameObjectWithTag ("imgtgt");
		target = GameObject.FindGameObjectWithTag ("MainCamera").transform;

		teleport = plan.GetComponent<gameController> ().newLocation ();;
		Debug.Log ("Selected location number: " + teleport.ToString());
		GameObject[] spe = GameObject.FindGameObjectsWithTag ("enemyspawn");
		gameObject.transform.position = spe [teleport].transform.position;
	}

	// Update is called once per frame
	void Update () {

		gameObject.transform.LookAt (target);

		if (health == 0) {
			plan.GetComponent<gameController> ().createEnemy (teleport);
			notice.GetComponent<notice_text> ().update_notice ("enemy");
			canvas.GetComponent<canvas_text> ().update_canvas ("enemy");
			Destroy (gameObject);
		}

//		if (target != null) {
//			transform.LookAt(target);
//		}

	}

	public void decreaseHealth (string a){
		if (a == "injured") {
			health -= 10;
		} else if (a == "dead"){
			health = 0;
		}
	}

	//6 April 3:54 PM since we are not using separate colliders on single enemy anymore.
	void OnCollisionEnter (Collision collision){
		Debug.Log ("it's a hit!" + collision.collider.name);
		if (collision.collider.name == "bullet(Clone)") {
			decreaseHealth ("injured");
		}
	}

}
