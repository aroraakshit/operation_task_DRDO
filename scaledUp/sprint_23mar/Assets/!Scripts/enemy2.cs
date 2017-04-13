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
 * 2) r theta model
 * 3) [perfection] improve the way soldier looks. Need new fbx files
 * 4) allowedAngle and allowedDistance need tuning based upon scaling up of the experiment
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
	public string[] p_status = {"WALK_STRAIGHT", "TURN", "STOP", "SHOOT"};
	public int status;
	public int speed;
	public int r;	//magnitude of movement
	public int r_max;	//maximum magnitude for translation of enemy movement
	public int theta;	//angle of movement
	public int allowedAngle;	//to shoot camera
	public float allowedDistance;	//to shoot camera
	public float translation;
	GameObject boundary;
	public float wait;
	[HideInInspector]
	public float wait1;
	private Quaternion targetRotation;
	private Transform deviation;
	private Vector3 loc;
	// Use this for initialization
	void Start () {
		wait = 0.0f;
		wait1 = wait / Time.deltaTime;
		status = 2;
		speed = 8;
		r = 0;
		translation = 10.0f;
		r_max = 8;
		theta = 0;
		allowedAngle = 60;
		allowedDistance = 40.0f;	//needs tuning with scaled up environment
		canvas = GameObject.FindGameObjectWithTag ("canvas_text");
		notice = GameObject.FindGameObjectWithTag ("notice");
		plan = GameObject.FindGameObjectWithTag ("imgtgt");
		boundary = GameObject.FindGameObjectWithTag ("boundary");
		target = GameObject.FindGameObjectWithTag ("bulletspawn").transform;
		teleport1 = GameObject.FindGameObjectsWithTag ("enemyspawn").GetLength (0);
		teleport = Random.Range (0, teleport1);
		//Debug.Log ("Selected location number: " + teleport.ToString());
		loc = GameObject.FindGameObjectsWithTag ("enemyspawn") [teleport].transform.position;
		loc.y = 1959.5f;
		gameObject.transform.position = loc;
		changeStatus ();
		//GetComponent<Animation> ().Play (true);
		//StartCoroutine (fsm (2));
	}

	// Update is called once per frame
	void Update () {
		//if boundary is hit OR enemy to enemy collision, the enemy must rotate 180 degrees on y axis.
		if (Mathf.Abs(boundary.transform.position.z - gameObject.transform.position.z) > 50 || Mathf.Abs(boundary.transform.position.x - gameObject.transform.position.x) > 55 || EnemyCollisionDetection()) {
			transform.position -= (transform.forward);
			Debug.Log (gameObject + " is rotating by 180 " + Mathf.Abs(boundary.transform.position.z - gameObject.transform.position.z) + "  " + Mathf.Abs(boundary.transform.position.x - gameObject.transform.position.x) + "   " + EnemyCollisionDetection ());
			transform.Rotate (0, 180, 0);
		}

		if (canSeePlayer () && status!=3) {
			status = 3;
		}

		if (translation <= 0.0f && status == 0) {
			status = 2;
		}



		switch (status) { //enemy's movement routines under various status situations
		case 0:
			{
				//code for walk straight
				transform.Translate(0, 0, Time.deltaTime * speed);
				//transform.position += (transform.forward * Time.deltaTime * speed); for infinite forward translation, but not valid for r theta model
				translation--;
				GetComponent<Animation> ().Play ("Take 001");
				break;
			}
		case 1:
			{
				//code for turn
				transform.Rotate(0,theta,0);
				status = 0;
				break;
			}
		case 2:
			{
				//code for waiting
				if (wait1 <= 0.0) {
					changeStatus ();
				} else
					wait1--;
				break;
			}
		case 3:
			{
				//code for shoot!
				plan.GetComponent<gameController> ().decreaseHealth ();
				if (!canSeePlayer ()) {
					changeStatus ();
				}
				break;
			}
		default:
			break;
		}

		//if health goes below zero, enemy must get destroyed 
		if (health <= 0) {
			plan.GetComponent<gameController> ().createEnemy ();
			notice.GetComponent<notice_text> ().update_notice ("enemy");
			canvas.GetComponent<canvas_text> ().update_canvas ("enemy");
			Destroy (gameObject);
		}

		//gameObject.transform.LookAt (target);
		//		if (transform.position.y < 1953) {
		//			transform.Translate (0, 1953 - transform.position.y, 0);
		//		}
		//
		//		if (transform.rotation.z < 0) {
		//			transform.Rotate (transform.forward, transform.rotation.z);
		//		}
		//
		//		if (transform.rotation.z > 0) {
		//			transform.Rotate (transform.forward, -transform.rotation.z);
		//		}
		//
		//		if (transform.rotation.x < 0) {
		//			transform.Rotate (transform.right, transform.rotation.x);
		//		}
		//
		//		if (transform.rotation.x > 0) {
		//			transform.Rotate (transform.right, -transform.rotation.x);
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
		if (collision.collider.name == "bullet(Clone)") { //if bullet hits the enemy
			decreaseHealth ("injured");
		}
		Debug.Log ("it's a hit!" + collision.collider.name);
	}

	void changeStatus () { //to decide the next state of enemy movement; based on r theta model now
		float rnd_r = Random.value;
		float rnd_theta = Random.value;

		if (status == 2 || status == 3) {
			theta = Mathf.RoundToInt ((rnd_theta * 360) / 30) * 30;
			r = Mathf.CeilToInt (rnd_r * r_max); //this way magnitude will never be zero
			//Debug.Log ("change status called for: " + gameObject.name + ", r = " + r + ", theta = " + theta);
			translation = r / Time.deltaTime;
			status = 1;
			wait1 = wait / Time.deltaTime;
		}

//		the following code is now irrelevant in r theta model
//		if (status == 0) {
//			//previous state walk straight, next state?
//			if 	(rnd <= 0.2) {
//				status = 0;
//			} else if (rnd <= 0.5) {
//				status = 1;
//			} else if (rnd <= 0.7) {
//				status = 2;
//			} else {
//				status = 3;
//			}
//		} else if (status == 1) {
//			//previous state turn, next state?
//			if (rnd <= 0.2) {
//				status = 0;
//			} else if (rnd <= 0.5) {
//				status = 1;
//			} else if (rnd <= 0.7) {
//				status = 2;
//			} else {
//				status = 3;
//			}
//		} else if (status == 2) {
//			//previous state stop, next state?
//			if (rnd <= 0.2) {
//				status = 0;
//			} else if (rnd <= 0.5) {
//				status = 1;
//			} else if (rnd <= 0.7) {
//				status = 2;
//			} else {
//				status = 3;
//			}
//		}
	}

	//Find the closest enemy; to solve the problem of enemy to enemy collision
	public bool EnemyCollisionDetection () {
		GameObject[] gos;
		gos = GameObject.FindGameObjectsWithTag ("enemy");
		Vector3 position = gameObject.transform.position;
		foreach (GameObject go in gos) {
			if (!go.Equals(gameObject)){
				float curDistance = Vector3.Distance (go.transform.position, gameObject.transform.position);
				if (curDistance < 5.0f) {
					return true;
				}
			}
		}
		return false;
	}

	public bool canSeePlayer() {
		Vector3 rayDirection = GameObject.FindGameObjectWithTag ("bulletspawn").transform.position - transform.position;
		//Debug.Log ("query toh hai" + Vector3.Angle (rayDirection, transform.forward) + " , " + Vector3.Distance(target.transform.position, gameObject.transform.position));
		if (Vector3.Angle (rayDirection, transform.forward) <= allowedAngle && Vector3.Distance(target.transform.position, gameObject.transform.position) <= allowedDistance) {
			//Debug.Log (gameObject.name + "has bullet spawn in the field of view, but it may not be able to see it because camera is behind a wall!");
			RaycastHit hit;
			Vector3 p1 = gameObject.transform.position;
			Debug.Log ("before sphere cast");
			if (Physics.SphereCast (p1,1.0f,rayDirection,out hit,allowedDistance)) {
				if (hit.collider.gameObject.tag == "bulletspawn") {
					Debug.Log (gameObject.name + "can hit me");
					return true;
				}
			}
		}
		return false;
	}

	//	public bool canHitPlayer() { // unable to make this work; if I use lookat to detect whether there is a wall between the enemy and me; I am unable to reverse it; used spherecast instead
//		if (canSeePlayer ()) {
//			RaycastHit hit;
//			Vector3 rayDirection = (target.position - transform.position);
//			//transform.LookAt (GameObject.FindGameObjectWithTag ("bulletspawn").transform);
//			//transform.Rotate (gameObject.transform.up, -Vector3.Angle (rayDirection, transform.forward));
//			//targetRotation = Quaternion.LookRotation (-rayDirection, Vector3.up);
//			//transform.rotation = Quaternion.Slerp (transform.rotation, targetRotation, Time.deltaTime * 0.5f);
//			//Debug.Log ("can hit player");
//			//transform.rotation = Quaternion.RotateTowards (transform.rotation, target.rotation, -1);
//			//transform.rotation.z = ;
//			//transform.LookAt (GameObject.FindGameObjectWithTag ("bulletspawn").transform);
//			if (Physics.Raycast (transform.position, rayDirection, out hit)) {
//				//if (hit.collider.gameObject.name == "bulletSpawn")
//				//	Debug.Log (hit.distance + " FROM " + hit.collider.gameObject.name + " OF " + gameObject.name);
//				if (hit.collider.gameObject.name == "bulletSpawn" && hit.distance <= allowedDistance) {
//					Debug.Log (gameObject.name + " DISTANCE " + hit.distance);
//					//gameObject.transform.rotation.Set (0, transform.rotation.y, 0, transform.rotation.w);
//					return true;
//				}
//			}
//		}
//		return false;
//	}

	//in r theta model we do not need to call changeStatus after some interval. We simply need to call it when enemy stops.
	//	IEnumerator fsm (int samplingRate) { //To decide the next enemy move after a fixed amount of "samplingRate" seconds
	//		//finite state machine goes here
	//		while (true) {
	//			changeStatus ();
	//			//Debug.Log (gameObject.name + " is now " + p_status[status]);
	//			yield return new WaitForSeconds (samplingRate);
	//		}
	//	}
}
