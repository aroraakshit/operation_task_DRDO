using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class gameController : MonoBehaviour {//, ITrackableEventHandler{

	public GameObject enemy; //what is to be instantiated as enemy
	List<int> iList = new List<int>();

	//public GameObject spawnEnemy;
	//public TrackableBehaviour imgtgt;

	// Use this for initialization
	void Start () {
		iList.Add (1);
		iList.Add (2);
		iList.Add (3);
		iList.Add (4);
		iList.Add (5);
		iList.Add (6);
		iList.Add (7);
		iList.Add (0);

		//imgtgt.GetComponent<ImageTargetAbstractBehaviour> ().RegisterTrackableEventHandler (this);
	}

	// Update is called once per frame
	void Update () {
		
	}

	public void createEnemy (int location) {
		Debug.Log ("I am in create enemy");
		GameObject enemy1 = Instantiate (enemy) as GameObject;
		iList.Add (location);
		//GameObject enemy1 = Instantiate (enemy, spawnEnemy.transform) as GameObject;
	}

	public int newLocation () { //to make sure we do not instantiate on the location previously chosen
		int teleport = Random.Range (0, 7);
		if (!iList.Contains(teleport)) {
			teleport = Random.Range (0, 7);
		}
		iList.Remove (teleport);
		return teleport;
	}

	//public void OnTrackableStateChanged (TrackableBehaviour.Status prevStatus, TrackableBehaviour.Status newState) {
	//Debug.Log (prevStatus.ToString() + "to" + newState.ToString());
	//if (newState.ToString () == "TRACKED") {
	//	GameObject enemy1 = Instantiate (enemy) as GameObject;
	//}
	//}
}
