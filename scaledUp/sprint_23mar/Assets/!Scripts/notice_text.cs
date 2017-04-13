/**
 * ATTACHED TO: notice in canvas without a white panel in background
 * 
 * TASKS OF THIS SCRIPT:
 * 1) display notices to player. Like "enemy killed", "I have been shot".
 * 2) take calls to update text from other scripts (update_canvas)
 * 3) make the notice disappear after "wait_notice" seconds.
 * 
 * TO DO:
 * 1) display user's health
 * 2) render it for VR (world space instead of screen overlay)
**/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class notice_text : MonoBehaviour {

	public Text text;
	public float wait_notice = 2.0f;
	public GameObject hurt;
	public bool damaged;
	public Color flashColor = new Color(1f, 0f, 0f, 0.1f);

	// Use this for initialization
	void Start () {
		text = gameObject.GetComponent<Text> ();
		hurt = GameObject.FindGameObjectWithTag ("hurt");
		text.text = "";
		damaged = false;
		hurt.GetComponent<Image> ().color = Color.clear;
	}
	
	// Update is called once per frame
	void Update () {
		if (text.text != "") {
			StartCoroutine (timer (wait_notice));
		}
		if (damaged) {
			hurt.GetComponent<Image> ().color = flashColor;
		} else {
			hurt.GetComponent<Image> ().color = Color.Lerp (hurt.GetComponent<Image> ().color, Color.clear, 5.0f * Time.deltaTime);
		}
//		text.text += "<color=red>You have been shot</color>";
		damaged = false;
	}

	public void update_notice (string a) {
		if (a == "enemy")
			text.text += "<color=green>Enemy Down!</color>";
		else if (a == "shot") {
			damaged = true;
		}
		else //for debugging other scripts
			text.text += a;
	}

	IEnumerator timer (float t){
		yield return new WaitForSeconds (t);
		text.text = "";
	}

}
