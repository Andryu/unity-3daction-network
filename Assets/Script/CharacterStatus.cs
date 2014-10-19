using UnityEngine;
using System.Collections;

public class CharacterStatus : MonoBehaviour {

	public int HP = 100;
	public int MaxHp = 100;

	public int Power = 10;

	public GameObject lastAttackTarget = null;

	// GUI お及びネットワーク
	// 名前
	public string CharacterName = "Player";

	// アニメーション
	// 状態
	public bool attcking = false;
	public bool died = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
