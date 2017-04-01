using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class canvas_text : MonoBehaviour {

	public Text text;
	public int bullet_number = 0;
	public int enemies_killed = 0;

	// Use this for initialization
	void Start () {
		text = gameObject.GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void update_canvas (string a){
		if (a == "bullet")
			bullet_number += 1;
		else if (a == "enemy")
			enemies_killed += 1;
		text.text = "Bullets Fired: " + bullet_number.ToString () + "\nEnemies Killed: " + enemies_killed.ToString ();
	}

}
