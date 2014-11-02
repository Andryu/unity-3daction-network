using UnityEngine;
using System.Collections;

public class SearchArea : MonoBehaviour {
	EnemyCtrl enemyCtrl;

	// Use this for initialization
	void Start () {
		enemyCtrl = transform.root.GetComponent<EnemyCtrl>();
	}

	void OnTriggerEnter(Collider other){
		//Debug.Log("Trigger Enter");
	}

	void OnTriggerStay(Collider other){
		if(other.tag == "Player") {
    		Debug.Log("Trigger Stay");
			enemyCtrl.SetAttackTarget( other.transform );
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
