using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using UnityEngine.UI;

public class enemy2 : MonoBehaviour {

	public GameObject canvas;
	public int health = 100;
	public GameObject notice;

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		if (health == 0) {
			notice.GetComponent<notice_text> ().update_notice ("enemy");
			canvas.GetComponent<canvas_text> ().update_canvas ("enemy");
			Destroy (gameObject);
		}
	}

	public void decreaseHealth (string a){
		if (a == "injured") {
			health -= 10;
		} else if (a == "dead"){
			health = 0;
		}
	}

}
