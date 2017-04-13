/**
 * ATTACHED TO: Image Target. 
 *
 * TASKS PERFORMED BY THIS SCRIPT:
 * 1) spawning enemies (redundant, can be shifted to enemy2.cs later depending upon final usage)
 * 2) logging data in files (dont destroy on load) *accelerometer *bullets *enemies killed
 * 3) enemy to shoot the player. Those screen effects and decrease in my own health
 *
 * TO DO:
 * NIL
 * 
 * DATA PRINT IN FILE FORMAT (after every 500ms, i.e. sampling rate of accelerometer) (either comma separated or JSON format; write as you play OR in the end).
 * 		=> SubjectID, Timestamp (of device), Bullets fired till now, Enemy Killed, X, Y, Z, 
**/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using System.IO;
using System;

public class gameController : MonoBehaviour {//, ITrackableEventHandler{

	public GameObject enemy; //what is to be instantiated as enemy
	public int subject, enemiesDown, bullets_fired;
	public float Acc_X, Acc_Y, Acc_Z;
	public DateTime log;
	public float health;
	public int bullet_hits;

//	List<int> iList = new List<int>();
//	public GameObject[] locations;
	//public GameObject spawnEnemy;
	//public TrackableBehaviour imgtgt;

	// Use this for initialization
	void Start () {
		subject = Convert.ToInt16 (UnityEngine.Random.value * 1000);
		enemiesDown = 0;
		bullets_fired = 0;
		Acc_X = 0.0f;
		Acc_Y = 0.0f;
		Acc_Z = 0.0f;
		health = 100.0f;
		bullet_hits = 0;
		StartCoroutine ("reportData");

//		locations = GameObject.FindGameObjectsWithTag ("enemyspawn");
//		for (int i = 0; i < locations.GetLength (0); i++) {
//			iList.Add (i);
//		}
//		iList.Add (1);
//		iList.Add (2);
//		iList.Add (3);
//		iList.Add (4);
//		iList.Add (5);
//		iList.Add (6);
//		iList.Add (7);
//		iList.Add (0);
		//imgtgt.GetComponent<ImageTargetAbstractBehaviour> ().RegisterTrackableEventHandler (this);
	}

	// Update is called once per frame
	void Update () {
		
	}
		
	public void createEnemy () {
		GameObject enemy1 = Instantiate (enemy) as GameObject; //it is redundant, it could be in enemy2.cs too, but prefabs would need to be changed.

		//iList.Add (location);
		//Debug.Log ("I am in create enemy");
		//GameObject enemy1 = Instantiate (enemy, spawnEnemy.transform) as GameObject;
	}

	IEnumerator reportData () {
		//Debug.Log (Application.persistentDataPath); //The value is a directory path where data expected to be kept between runs can be stored. When publishing on iOS and Android, persistentDataPath will point to a public directory on the device. Files in this location won't be erased with each update of the App. 
		while (true) {
			log = DateTime.Now;
			Acc_X = Input.acceleration.x;
			Acc_Y = Input.acceleration.y;
			Acc_Z = Input.acceleration.z;
			enemiesDown = GameObject.FindGameObjectWithTag ("canvas_text").GetComponent<canvas_text> ().enemies_killed;
			bullets_fired = GameObject.FindGameObjectWithTag ("canvas_text").GetComponent<canvas_text> ().bullet_number;
			string text = subject + " , " + log + " , " + enemiesDown + " , " + bullets_fired + " , " + Acc_X + " , " + Acc_Y + " , " + Acc_Z + " , " + bullet_hits + " , " + health + "\n";
			if (Application.platform != RuntimePlatform.Android) {
				using (System.IO.StreamWriter file = new System.IO.StreamWriter (@"C:\Users\akshi\Desktop\logs.txt", true)) { //we can make the path generic to Application.persistentDataPath + "\logs.txt"
					file.WriteLine (text);
					//string filePath = Application.dataPath + "/logs.txt";
					//GameObject.FindGameObjectWithTag ("notice").GetComponent<notice_text> ().update_notice (filePath);
				}
			} else {
				//string filePath = Application.persistentDataPath + "/logs.txt";
				using (System.IO.StreamWriter file = new System.IO.StreamWriter (Application.persistentDataPath + @"/logs.txt", true)) { 
					file.WriteLine (text);

					//another way to write the file
					//File.AppendAllText (filePath, text);
					//debugging
					//GameObject.FindGameObjectWithTag ("notice").GetComponent<notice_text> ().update_notice (filePath);
				}
			}
			yield return new WaitForSeconds (0.5f);
		}
	}

	public void decreaseHealth (){
		bullet_hits += 1;
		health -= 0.1f;
		GameObject.FindGameObjectWithTag ("canvas_text").GetComponent<canvas_text> ().update_canvas ("health");
		GameObject.FindGameObjectWithTag ("notice").GetComponent<notice_text> ().update_notice ("shot");
	}

//	public int newLocation () { //to decide new location of the spawned enemy
//		int teleport = Random.Range (0, 7);
//		if (!iList.Contains(teleport)) {
//			teleport = Random.Range (0, 7);
//		}
//		iList.Remove (teleport);
//		return teleport;
//	}
	//public void OnTrackableStateChanged (TrackableBehaviour.Status prevStatus, TrackableBehaviour.Status newState) {
	//Debug.Log (prevStatus.ToString() + "to" + newState.ToString());
	//if (newState.ToString () == "TRACKED") {
	//	GameObject enemy1 = Instantiate (enemy) as GameObject;
	//}
	//}
}
