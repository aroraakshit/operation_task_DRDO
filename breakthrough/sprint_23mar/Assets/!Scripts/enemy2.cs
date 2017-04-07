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
	public string[] p_status = {"WALK_STRAIGHT", "TURN_LEFT", "TURN_RIGHT", "STOP"};
	public int status;
	GameObject boundary;

	// Use this for initialization
	void Start () {
		status = Random.Range (0,3);
		canvas = GameObject.FindGameObjectWithTag ("canvas_text");
		notice = GameObject.FindGameObjectWithTag ("notice");
		plan = GameObject.FindGameObjectWithTag ("imgtgt");
		boundary = GameObject.FindGameObjectWithTag ("boundary");
		target = GameObject.FindGameObjectWithTag ("MainCamera").transform;
		teleport = plan.GetComponent<gameController> ().newLocation ();;
		Debug.Log ("Selected location number: " + teleport.ToString());
		GameObject[] spe = GameObject.FindGameObjectsWithTag ("enemyspawn");
		gameObject.transform.position = spe [teleport].transform.position;
		StartCoroutine (fsm (2));
	}

	// Update is called once per frame
	void Update () {
		if (boundary.transform.position.z - gameObject.transform.position.z <= -15 || boundary.transform.position.z - gameObject.transform.position.z >= 15 || boundary.transform.position.x - gameObject.transform.position.x <= -15 || boundary.transform.position.x - gameObject.transform.position.x >= 15) {
			transform.Rotate(0,180,0);
			Debug.Log ("Boundary hit!");
		}

		switch (status) {
		case 0:
			{
				//code for walk straight
				transform.position += (transform.forward * Time.deltaTime * 10);
				break;
			}
		case 1:
			{
				//code for turn left
				transform.Rotate(0,-90 * Time.deltaTime,0);
				status = 0;
				break;
			}
		case 2:
			{
				//code for turn right
				transform.Rotate(0,90,0);
				status = 0;
				break;
			}
		case 3:
			{
				//code for stop
				break;
			}
		default:
			break;
		}

		if (health == 0) {
			plan.GetComponent<gameController> ().createEnemy (teleport);
			notice.GetComponent<notice_text> ().update_notice ("enemy");
			canvas.GetComponent<canvas_text> ().update_canvas ("enemy");
			Destroy (gameObject);
		}

		//gameObject.transform.LookAt (target);

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

	IEnumerator fsm (int samplingRate) {
		//finite state machine goes here
		while (true) {
			changeStatus ();
			//Debug.Log (gameObject.name + " is now " + p_status[status]);
			yield return new WaitForSeconds (samplingRate);
		}
	}

	void changeStatus () {
		float rnd = Random.value;
		if (status == 0) {
			//previous state running, next state?
			if (rnd <= 0.2) {
				status = 0;
			} else if (rnd <= 0.5) {
				status = 1;
			} else if (rnd <= 0.7) {
				status = 2;
			} else {
				status = 3;
			}
		} else if (status == 1) {
			//previous state turn left, next state?
			if (rnd <= 0.2) {
				status = 0;
			} else if (rnd <= 0.5) {
				status = 1;
			} else if (rnd <= 0.7) {
				status = 2;
			} else {
				status = 3;
			}
		} else if (status == 2) {
			//previous state turn right, next state?
			if (rnd <= 0.2) {
				status = 0;
			} else if (rnd <= 0.5) {
				status = 1;
			} else if (rnd <= 0.7) {
				status = 2;
			} else {
				status = 3;
			}
		} else {
			//previous state stop, next state?
			if (rnd <= 0.2) {
				status = 0;
			} else if (rnd <= 0.5) {
				status = 1;
			} else if (rnd <= 0.7) {
				status = 2;
			} else {
				status = 3;
			}
		}
	}
}
