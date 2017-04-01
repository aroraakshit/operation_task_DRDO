using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class notice_text : MonoBehaviour {

	public Text text;
	public float wait_notice = 2.0f;

	// Use this for initialization
	void Start () {
		text = gameObject.GetComponent<Text> ();
		text.text = "";
	}
	
	// Update is called once per frame
	void Update () {
		if (text.text != "") {
			StartCoroutine (timer (wait_notice));
		}
	}

	public void update_notice (string a) {
		if (a == "enemy")
			text.text = "<color=green>Enemy Down!</color>";
	}

	IEnumerator timer (float t){
		yield return new WaitForSeconds (t);
		text.text = "";
	}

}
