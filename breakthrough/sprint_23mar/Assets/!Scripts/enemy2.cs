/**
 * ATTACHED TO: Army-final prefab
 * 
 * TASKS OF THIS SCRIPT:
 * 1) to decide spawn location randomly out of all locations with tag "enemyspawn" (teleport variable).
 * 2) health of enemy (notify canvas when dead; decrease health when hit by bullet; notify gamecontroller to spawn new enemy)
 * 3) boundary and enemy-enemy collision manage
 * 4) state machine for movement across the terrain
 * 5) added new status 4 for making it look like its shooting the camera.
 * 
 * TO DO:
 * 1) notify game controller when enemy is by chance facing camera and inside a close proximity of camera to shoot it. A probability for shoot/not to shoot.
**/

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
	public int teleport1;
	public string[] p_status = {"WALK_STRAIGHT", "TURN_LEFT", "TURN_RIGHT", "STOP"};
	public int status;
	public int speed;
	GameObject boundary;

	// Use this for initialization
	void Start () {
		status = Random.Range (0,3);
		speed = 10;
		canvas = GameObject.FindGameObjectWithTag ("canvas_text");
		notice = GameObject.FindGameObjectWithTag ("notice");
		plan = GameObject.FindGameObjectWithTag ("imgtgt");
		boundary = GameObject.FindGameObjectWithTag ("boundary");
		target = GameObject.FindGameObjectWithTag ("MainCamera").transform;
		teleport1 = GameObject.FindGameObjectsWithTag ("enemyspawn").GetLength (0);
		teleport = Random.Range (0, teleport1);
		Debug.Log ("Selected location number: " + teleport.ToString());
		gameObject.transform.position = GameObject.FindGameObjectsWithTag ("enemyspawn") [teleport].transform.position;
		StartCoroutine (fsm (2));
	}

	// Update is called once per frame
	void Update () {
		//if boundary is hit OR enemy to enemy collision, the enemy must rotate 180 degrees on y axis.
		if (Mathf.Abs(boundary.transform.position.z - gameObject.transform.position.z) > 15 || Mathf.Abs(boundary.transform.position.x - gameObject.transform.position.x) > 15 || EnemyCollisionDetection()) {
			transform.position -= (transform.forward);
			transform.Rotate (0, 180, 0);
		}

		switch (status) { //enemy's movement routines under various status situations
		case 0:
			{
				//code for walk straight
				transform.position += (transform.forward * Time.deltaTime * speed);
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
		case 4:
			{
				//code for stop and shoot the player
				transform.LookAt (target.transform);
				status = 3;
				break;
			}
		default:
			break;
		}

		if (health == 0) {
			plan.GetComponent<gameController> ().createEnemy ();
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
		if (collision.collider.name == "bullet(Clone)") { //if bullet hits the enemy
			decreaseHealth ("injured");
		}
		//Debug.Log ("it's a hit!" + collision.collider.name);
	}

	IEnumerator fsm (int samplingRate) { //To decide the next enemy move after a fixed amount of "samplingRate" seconds
		//finite state machine goes here
		while (true) {
			changeStatus ();
			//Debug.Log (gameObject.name + " is now " + p_status[status]);
			yield return new WaitForSeconds (samplingRate);
		}
	}

	void changeStatus () { //to decide the next state of enemy movement
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

	//Find the closest enemy; to solve the problem of enemy to enemy collision
	bool EnemyCollisionDetection () {
		GameObject[] gos;
		gos = GameObject.FindGameObjectsWithTag ("enemy");
		GameObject closest = null;
		float distance = Mathf.Infinity;
		Vector3 position = gameObject.transform.position;
		foreach (GameObject go in gos) {
			if (go.name != gameObject.name){
				float curDistance = Vector3.Distance (go.transform.position, position);
				if (curDistance < distance) {
					closest = go;
					distance = curDistance;
				}
			}
		}
		//Debug.Log (distance + " for " + gameObject.name + " from " + closest.name);
		if (distance == 0.0f) {
			//Debug.Log ("Enemy to enemy collision! " + gameObject.name + " COLLIDED WITH " + closest.name);
			return true;
		}
		else
			return false;
	}

}
