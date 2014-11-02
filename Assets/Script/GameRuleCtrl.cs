using UnityEngine;
using System.Collections;

public class GameRuleCtrl : MonoBehaviour {

	public float timeRemaining = 5.0f * 60.0f;
	public bool bGameOver = false;
	public bool bGameClear=false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (timeRemaining <= 0.0f)
			GameOver ();
	}

    public void GameOver() {
		Debug.Log("Game Over!!!");
		bGameOver = true;
	}

	public void GameClear() {
		Debug.Log ("Game Clear");
		bGameClear = true;
	}
}
