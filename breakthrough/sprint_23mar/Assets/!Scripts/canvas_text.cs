/**
 * ATTACHED TO: text in canvas with a white panel in background
 * 
 * TASKS OF THIS SCRIPT:
 * 1) display values to player. Like bullet_number, enemies_killed.
 * 2) take calls to update text from other scripts (update_canvas)
 * 
 * TO DO:
 * 1) display user's health
 * 2) render it for VR (world space instead of screen overlay)
**/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class canvas_text : MonoBehaviour {

	public Text text;
	public float health;
	[HideInInspector]
	public int bullet_number = 0;
	[HideInInspector]
	public int enemies_killed = 0;
	public string enemyDistance;

	void Awake () {
		bullet_number = 0;
		enemies_killed = 0;
		enemyDistance = "";
		health = GameObject.FindGameObjectWithTag ("imgtgt").GetComponent<gameController> ().health;
	}

	// Use this for initialization
	void Start () {
		text = gameObject.GetComponent<Text> ();
		update_canvas ("health");
	}
	
	// Update is called once per frame
	void Update () {
		update_canvas ("enemydistance");
	}

	public void update_canvas (string a){
		if (a == "bullet")
			bullet_number += 1;
		else if (a == "enemy")
			enemies_killed += 1;
		else if (a == "health")
			health = GameObject.FindGameObjectWithTag ("imgtgt").GetComponent<gameController> ().health;
		else if (a == "enemydistance")
			enemyDistance = EnemyCollisionDetection ();

		text.text = "Bullets Fired: " + bullet_number.ToString () + "\nEnemies Killed: " + enemies_killed.ToString () + "\nHealth: " + health.ToString() + 
			"\nDistance: \n" + enemyDistance;//+ "\n<size=7>DEBUGGING: " + (Application.platform != RuntimePlatform.Android) + "</size>";
	}

	public string EnemyCollisionDetection () {
		GameObject[] gos;
		gos = GameObject.FindGameObjectsWithTag ("enemy");
		string answer;
		answer = "";
		Vector3 position = GameObject.FindGameObjectWithTag("bulletspawn").transform.position;
		foreach (GameObject go in gos) {
			float curDistance = Vector3.Distance (go.transform.position, position);
			answer += go.name.ToString () + " : " + curDistance.ToString () + "\n";
		}
		return answer;
	}

}
